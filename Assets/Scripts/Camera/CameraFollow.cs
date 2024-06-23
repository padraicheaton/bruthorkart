using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    enum UpdateMethods
    {
        Update,
        FixedUpdate
    }

    [SerializeField] private UpdateMethods updateMethod;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;

    private void Update()
    {
        if (updateMethod == UpdateMethods.Update)
            UpdateCamPosition(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (updateMethod == UpdateMethods.FixedUpdate)
            UpdateCamPosition(Time.fixedDeltaTime);
    }

    private void UpdateCamPosition(float delta)
    {
        Vector3 targetPosition = target.position + Vector3.up * offset.y;
        targetPosition += target.forward * offset.z;
        targetPosition += target.right * offset.x;

        transform.position = Vector3.Lerp(transform.position, targetPosition, delta * speed);

        transform.LookAt(target.position);
    }
}
