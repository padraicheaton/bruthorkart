using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BruthorEditorUtility : MonoBehaviour
{
    [MenuItem("Bruthor/Play From Top")]
    public static void PlayFromTop()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Services.unity");
        EditorApplication.EnterPlaymode();
    }
}
