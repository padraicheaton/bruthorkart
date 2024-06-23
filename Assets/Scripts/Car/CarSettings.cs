using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Bruthor/Car")]
public class CarSettings : ScriptableObject
{
    public new string name;
    public GameObject carModel;

    [Header("Settings")]
    public float fowardAccel;
    public float reverseAccel;
    public float maxSpeed;
    public float turnStrength;
    public float groundDrag;
    public float airDrag;
}
