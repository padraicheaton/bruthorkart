using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeSettings : ScriptableObject
{
    public new string name;
    public GameObject managerPrefab;
    public List<LevelData> compatibleLevels;
    [TextArea(4, 4)] public string description;
}
