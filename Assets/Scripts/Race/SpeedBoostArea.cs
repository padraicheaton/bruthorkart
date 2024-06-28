using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<CarController>(out CarController controller))
        {
            controller.BoostSpeed(GlobalSettings.SmallSpeedBoost, GlobalSettings.ShortPowerUpDuration);
        }
    }
}
