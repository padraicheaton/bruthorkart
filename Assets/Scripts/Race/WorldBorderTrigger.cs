using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldBorderTrigger : MonoBehaviour
{
    public enum WorldBorderAxis
    {
        X,
        Y,
        Z
    }

    [Header("Settings")]
    [SerializeField] private WorldBorderAxis axis;

    public static UnityAction<PlayerConfiguration> OnPlayerPassedWorldBorder;

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        boxCollider.size = GetDimensions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<BaseController>(out BaseController pc))
        {
            OnPlayerPassedWorldBorder?.Invoke(pc.GetConfig());

            Debug.Log($"Player {pc.GetConfig().PlayerIndex} passed world border");
        }
    }

    private Vector3 GetDimensions()
    {
        Vector3 dimensions;

        switch (axis)
        {
            case WorldBorderAxis.X:
                dimensions = new Vector3(0f, 1f, 1f);
                break;
            case WorldBorderAxis.Y:
            default:
                dimensions = new Vector3(1f, 0f, 1f);
                break;
            case WorldBorderAxis.Z:
                dimensions = new Vector3(1f, 1f, 0f);
                break;
        }

        dimensions *= 1000f;
        dimensions += Vector3.one;

        return dimensions;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.Lerp(Color.green, Color.clear, 0.75f);
        Gizmos.DrawCube(transform.position, GetDimensions());
    }
}
