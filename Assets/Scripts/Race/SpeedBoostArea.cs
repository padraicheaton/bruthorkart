using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float amount;
    [SerializeField] private float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<CarController>(out CarController controller))
        {
            controller.BoostSpeed(amount, duration);
        }
    }
}
