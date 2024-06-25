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

    public void RegisterPlayerLastGrounded(int playerID, Vector3 pos)
    {
        foreach (RacerData racer in racers)
            if (racer.PlayerID == playerID)
            {
                racer.UpdateLastGrounded(pos);
                break;
            }
    }

    public int GetTotalLaps() => NumLaps;


    public Transform GetClosestCheckpointToPlayerLastGrounded(int id)
    {
        Vector3 lastGroundedPos = GetRacer(id).LastPosGrounded;

        float distance = float.MaxValue;
        Transform resetTform = trackCheckpoints[0].transform;

        foreach (TrackCheckpoint checkpoint in trackCheckpoints)
        {
            float chkDist = Vector3.Distance(lastGroundedPos, checkpoint.transform.position);

            if (chkDist < distance)
            {
                distance = chkDist;
                resetTform = checkpoint.transform;
            }
        }

        return resetTform;
    }

    private RacerData GetRacer(int id)
    {
        foreach (RacerData racer in racers)
            if (racer.PlayerID == id)
            {
                return racer;
            }

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
        public Vector3 LastPosGrounded; // used for resetting players when needed

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

        public void UpdateLastGrounded(Vector3 pos)
        {
            LastPosGrounded = pos;
            Debug.Log($"Player {PlayerID} last ground {pos}");
        }
    }
}
