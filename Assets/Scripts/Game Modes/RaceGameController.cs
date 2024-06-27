using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceGameController : BaseGameMode
{
    private RaceGameModeSettings settings;

    private List<TrackCheckpoint> trackCheckpoints = new List<TrackCheckpoint>();
    private List<RacerData> racers = new List<RacerData>();

    public static UnityAction<RacerData> OnPlayerCompletedLap;

    protected override void PostSetup(GameModeSettings gameModeSettings)
    {
        settings = gameModeSettings as RaceGameModeSettings;

        foreach (PlayerConfiguration config in PlayerConfigurationManager.Instance.GetPlayerConfigurations())
        {
            racers.Add(new RacerData(config.PlayerIndex));
        }

        OnPlayerCompletedLap += racer => CheckIfGameComplete();
    }

    public void RegisterCheckpoint(TrackCheckpoint trackCheckpoint)
    {
        trackCheckpoints.Add(trackCheckpoint);
    }

    public int GetTotalLaps() => settings.Laps;


    public Transform GetClosestCheckpointToPlayerLastGrounded(int id)
    {
        Debug.Log($"Trying to find player: {id}");

        RacerData racer = GetRacer(id);

        if (racer.PassedCheckpointIDs.Count > 0)
        {
            int lastPassedCheckpointID = racer.PassedCheckpointIDs[racer.PassedCheckpointIDs.Count - 1];

            foreach (TrackCheckpoint checkpoint in trackCheckpoints)
                if (checkpoint.ID == lastPassedCheckpointID)
                    return checkpoint.transform;
        }

        return trackCheckpoints[0].transform;
    }

    private void CheckIfGameComplete()
    {
        if (IsGameComplete())
            OnGameFinished();
    }

    private bool IsGameComplete()
    {
        foreach (RacerData racer in racers)
        {
            if (racer.LapsCompleted < GetTotalLaps())
                return false;
        }

        return true;
    }

    private RacerData GetRacer(int id)
    {
        foreach (RacerData racer in racers)
        {
            if (racer.PlayerID == id)
            {
                return racer;
            }
        }

        Debug.Log($"Couldn't find racer with ID: {id}");

        return null;
    }

    public void CheckpointPassed(int checkpointID, int playerID)
    {
        foreach (RacerData racer in racers)
        {
            if (racer.PlayerID == playerID)
            {
                racer.PassedCheckpoint(checkpointID);

                // If at start and finished lap
                if (checkpointID == 0 && HasRacerFinishedLap(racer))
                {
                    Debug.Log($"Player {racer.PlayerID} finished lap!");
                    racer.ResetCheckpoints();

                    OnPlayerCompletedLap?.Invoke(racer);

                    if (racer.LapsCompleted >= GetTotalLaps())
                        Debug.Log($"Player {racer.PlayerID} finished race!");
                }

                break;
            }
        }
    }

    private bool HasRacerFinishedLap(RacerData racer)
    {
        return racer.PassedCheckpointIDs.Count == trackCheckpoints.Count;
    }

    public class RacerData
    {
        public int PlayerID;
        public List<int> PassedCheckpointIDs;
        public int LapsCompleted;

        public RacerData(int _PlayerID)
        {
            PlayerID = _PlayerID;
            LapsCompleted = 0;
            PassedCheckpointIDs = new List<int>();
        }

        public void PassedCheckpoint(int ID)
        {
            if (!PassedCheckpointIDs.Contains(ID))
                PassedCheckpointIDs.Add(ID);
        }

        public void ResetCheckpoints()
        {
            PassedCheckpointIDs = new List<int>();
            LapsCompleted++;
        }
    }
}
