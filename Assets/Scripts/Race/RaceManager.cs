using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceManager : Singleton<RaceManager>
{
    [Header("Settings")]
    [SerializeField] private int NumLaps;

    private List<TrackCheckpoint> trackCheckpoints = new List<TrackCheckpoint>();
    private List<RacerData> racers = new List<RacerData>();

    public static UnityAction<RacerData> OnPlayerCompletedLap;


    private void Start()
    {
        foreach (PlayerConfiguration config in PlayerConfigurationManager.Instance.GetPlayerConfigurations())
        {
            racers.Add(new RacerData(config.PlayerIndex));
        }
    }

    public void RegisterCheckpoint(TrackCheckpoint trackCheckpoint)
    {
        trackCheckpoints.Add(trackCheckpoint);
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

                    if (racer.LapsCompleted >= NumLaps)
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
