using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [Header("References")]
    [SerializeField] private CanvasGroup loadingScreenCG;

    [Header("Settings")]
    [SerializeField] private float loadingScreenFadeSpeed;

    public enum Level
    {
        Services,
        MainMenu,
        MainGame,
        ModeSelection,
        CharSelection,
        LevelOne,
        Sandbox
    }

    private void Start()
    {
        LoadScene(Level.MainMenu);
    }

    public void TransitionScene(Level scene)
    {
        StartCoroutine(LoadScreenLoadScene(scene));
    }

    public void LoadScene(Level scene)
    {
        List<Level> scenesToLoad = new List<Level>() { scene };

        scenesToLoad.AddRange(GetPrerequisiteScenesFor(scene));

        UnloadUnnecessaryScenes(scenesToLoad);

        foreach (Level level in scenesToLoad)
        {
            if (SceneManager.GetSceneByBuildIndex((int)level).isLoaded)
                continue;

            SceneManager.LoadScene((int)level, LoadSceneMode.Additive);
        }
    }

    public List<Level> GetPrerequisiteScenesFor(Level scene)
    {
        List<Level> scenes = new List<Level>() { Level.Services };

        switch (scene)
        {
            case Level.LevelOne:
            case Level.Sandbox:
                scenes.Add(Level.MainGame);
                break;
            default:
                break;
        }

        return scenes;
    }

    public void UnloadUnnecessaryScenes(List<Level> requiredScenes)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.isLoaded && !requiredScenes.Contains((Level)scene.buildIndex))
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    private IEnumerator LoadScreenLoadScene(Level scene)
    {
        while (loadingScreenCG.alpha < 1f)
        {
            loadingScreenCG.alpha += Time.deltaTime * loadingScreenFadeSpeed;

            yield return null;
        }

        //LoadScene(scene);

        List<Level> scenesToLoad = new List<Level>();

        scenesToLoad.AddRange(GetPrerequisiteScenesFor(scene));
        scenesToLoad.Add(scene);

        UnloadUnnecessaryScenes(scenesToLoad);

        foreach (Level level in scenesToLoad)
        {
            if (SceneManager.GetSceneByBuildIndex((int)level).isLoaded)
                continue;

            SceneManager.LoadScene((int)level, LoadSceneMode.Additive);

            yield return null;
        }

        while (loadingScreenCG.alpha > 0f)
        {
            loadingScreenCG.alpha -= Time.deltaTime * loadingScreenFadeSpeed;

            yield return null;
        }
    }

    public void MoveObjToScene(GameObject obj, Level scene)
    {
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByBuildIndex((int)scene));
    }
}
