using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Bruthor/Level")]
public class LevelData : ScriptableObject
{
    public new string name;
    public Sprite previewImg;
    public SceneController.Level level;
}
