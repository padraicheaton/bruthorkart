using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BaseGameMode : Singleton<BaseGameMode>
{
    [Header("References")]
    [SerializeField] private GameObject modeSpecificHUDPrefab;

    protected List<CharRankData> characterRanks;
    protected GameModeSettings settings;

    // Takes in the item being picked up and the player ID who picked it up
    public UnityAction<ItemData, int> OnItemPickedUp;

    public void Setup(GameModeSettings gameModeSettings)
    {
        settings = gameModeSettings;

        characterRanks = new List<CharRankData>();

        PostSetup(gameModeSettings);
    }


    protected abstract void PostSetup(GameModeSettings gameModeSettings);

    public abstract Transform GetResetTransform(int playerID);

    public GameObject GetModeHUD() => modeSpecificHUDPrefab;

    public List<CharRankData> GetRankedCharacters()
    {
        if (characterRanks == null)
            return null;

        characterRanks.Sort((charRankA, charRankB) => charRankB.Points.CompareTo(charRankA.Points));

        return characterRanks;
    }

    protected void OnGameFinished()
    {
        SceneController.Instance.AddScene(SceneController.Level.Results);
    }

    public ItemData GetRandomItem()
    {
        if (settings.items.Count == 0)
            return null;

        return Extensions.GetRandom(settings.items);
    }
}
