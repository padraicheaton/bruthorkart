using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoint : MonoBehaviour
{
    [SerializeField] public int ID;

    private void Start()
    {
        (RaceGameController.Instance as RaceGameController).RegisterCheckpoint(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            (RaceGameController.Instance as RaceGameController).CheckpointPassed(ID, pc.GetConfig().PlayerIndex);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < ID + 1; i++)
            Gizmos.DrawWireCube(transform.position + Vector3.up * i, Vector3.one);
    }
}
