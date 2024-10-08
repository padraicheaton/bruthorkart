using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class RaceGameController : BaseGameMode
{
    private List<TrackCheckpoint> trackCheckpoints = new List<TrackCheckpoint>();
    private List<RacerData> racers = new List<RacerData>();

    public static UnityAction<RacerData> OnPlayerCompletedLap;

    private RaceGameModeSettings RaceSettings => settings as RaceGameModeSettings;

    protected override void PostSetup(GameModeSettings gameModeSettings)
    {
        racers = new List<RacerData>();

        foreach (PlayerConfiguration config in PlayerConfigurationManager.Instance.GetPlayerConfigurations())
        {
            racers.Add(new RacerData(config.PlayerIndex));
        }

        OnPlayerCompletedLap += OnRacerCompletedLap;
    }

    private void OnDestroy()
    {
        OnPlayerCompletedLap -= OnRacerCompletedLap;
    }

    public void RegisterCheckpoint(TrackCheckpoint trackCheckpoint)
    {
        trackCheckpoints.Add(trackCheckpoint);

        trackCheckpoints.Sort((A, B) => A.ID.CompareTo(B.ID));
    }

    public List<TrackCheckpoint> GetTrackCheckpoints() => trackCheckpoints;

    public int GetTotalLaps() => RaceSettings.Laps;

    private void OnRacerCompletedLap(RacerData racer)
    {
        CheckIfGameComplete();
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

                    if (racer.LapsCompleted >= GetTotalLaps())
                        RecordPlayerPoints(racer);

                    OnPlayerCompletedLap?.Invoke(racer);
                }

                break;
            }
        }
    }

    private bool HasRacerFinishedLap(RacerData racer)
    {
        return racer.PassedCheckpointIDs.Count == trackCheckpoints.Count;
    }

    private void RecordPlayerPoints(RacerData racer)
    {
        int points = characterRanks.Count > 0 ? characterRanks[characterRanks.Count - 1].Points - 1 : 10;

        characterRanks.Add(new CharRankData(racer.PlayerID, points));
    }

    public override Transform GetResetTransform(int playerID)
    {
        RacerData racer = GetRacer(playerID);

        if (racer.PassedCheckpointIDs.Count > 0)
        {
            int lastPassedCheckpointID = racer.PassedCheckpointIDs[racer.PassedCheckpointIDs.Count - 1];

            foreach (TrackCheckpoint checkpoint in trackCheckpoints)
                if (checkpoint.ID == lastPassedCheckpointID)
                    return checkpoint.transform;
        }

        return trackCheckpoints[0].transform;
    }

    [System.Serializable]
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
