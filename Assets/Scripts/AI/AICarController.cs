using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : BaseController
{
    [Header("Settings")]
    [SerializeField] private float checkRayLength;
    [SerializeField] private float sideCheckRayAngle;
    [SerializeField] private Vector3 checkRayOffset;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float passedWaypointThreshold;

    private List<Transform> waypoints = new List<Transform>();
    private int nextWaypointIndex = 0;

    private Transform EngineTransform => carController ? carController.GetPos() : transform;
    private Transform VisualsTransform => carController ? carController.GetVisuals() : transform;

    private RaceGameController RaceController => RaceGameController.Instance as RaceGameController;

    public override void Initialise(PlayerConfiguration playerConfiguration)
    {
        base.Initialise(playerConfiguration);

        RaceController.OnGameBegun += RegisterWaypoints;
    }

    private void RegisterWaypoints()
    {
        waypoints = new List<Transform>();

        foreach (TrackCheckpoint checkpoint in RaceController.GetTrackCheckpoints())
        {
            waypoints.Add(checkpoint.transform);
        }
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Vector2 input = Vector2.zero;

        bool forwardCheck = Physics.Linecast(GetRayOrigin(), GetForwardCheckDestination(), whatIsGround);
        bool rightCheck = Physics.Linecast(GetRayOrigin(), GetRightCheckDestination(), whatIsGround);
        bool leftCheck = Physics.Linecast(GetRayOrigin(), GetLeftCheckDestination(), whatIsGround);

        input.y = forwardCheck ? -1f : 1f;

        if (Vector3.Distance(EngineTransform.position, waypoints[nextWaypointIndex].position) < passedWaypointThreshold)
        {
            nextWaypointIndex++;

            if (nextWaypointIndex >= waypoints.Count)
                nextWaypointIndex = 0;
        }

        Vector3 directionToWaypoint = waypoints[nextWaypointIndex].position - EngineTransform.position;

        if (Vector3.Dot(VisualsTransform.right, directionToWaypoint) > 0)
        {
            // To the right
            input.x = 1;
        }
        else
        {
            // To the left
            input.x = -1;
        }

        // This  is so that, when reversing, the car remains turning the correct way
        input.x *= input.y;

        if (rightCheck)
            input.x = -1;
        if (leftCheck)
            input.x = 1;

        carController.UpdateAccelerateInput(input.y);
        carController.UpdateSteeringInput(input.x);

        // If has an item, use it immediately
        if (itemHandler.HasItem())
            itemHandler.TryUseItem();
    }

    private Vector3 GetRayOrigin() => EngineTransform.position + checkRayOffset;
    private Vector3 GetForwardCheckDestination() => GetRayOrigin() + VisualsTransform.forward * checkRayLength;
    private Vector3 GetRightCheckDestination() => GetRayOrigin() + (VisualsTransform.forward + VisualsTransform.right * sideCheckRayAngle) * checkRayLength;
    private Vector3 GetLeftCheckDestination() => GetRayOrigin() + (VisualsTransform.forward - VisualsTransform.right * sideCheckRayAngle) * checkRayLength;

    private void OnDrawGizmos()
    {
        foreach (Vector3 point in new Vector3[] { GetForwardCheckDestination(), GetRightCheckDestination(), GetLeftCheckDestination() })
        {
            Gizmos.DrawLine(GetRayOrigin(), point);
        }

        Gizmos.color = Color.yellow;
        if (waypoints.Count > 0)
        {
            Gizmos.DrawLine(EngineTransform.position, waypoints[nextWaypointIndex].position);
        }
    }
}
