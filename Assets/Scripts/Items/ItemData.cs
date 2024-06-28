using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Bruthor/Item")]
public class ItemData : ScriptableObject
{
    public enum Effect
    {
        SpeedBoost,
        HyperBoost
    }

    public new string name;
    public Sprite icon;
    public Effect effect;

    public void OnItemUsed(CarController carController, Vector3 forward)
    {
        Debug.Log($"Using Item: {name}");

        switch (effect)
        {
            default:
            case Effect.SpeedBoost:
                ApplySpeedBoost(carController, 15, 2);
                break;
            case Effect.HyperBoost:
                ApplySpeedBoost(carController, 20, 10);
                break;
        }
    }

    private void ApplySpeedBoost(CarController car, float amount, float duration)
    {
        car.BoostSpeed(amount, duration);
    }
}
