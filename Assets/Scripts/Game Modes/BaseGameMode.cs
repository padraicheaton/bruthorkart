using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BaseGameMode : Singleton<BaseGameMode>
{
    protected List<CharRankData> characterRanks;

    public void Setup(GameModeSettings gameModeSettings)
    {
        characterRanks = new List<CharRankData>();

        foreach (PlayerConfiguration playerConfiguration in PlayerConfigurationManager.Instance.GetPlayerConfigurations())
        {
            characterRanks.Add(new CharRankData(playerConfiguration.PlayerIndex));
        }

        PostSetup(gameModeSettings);
    }


    protected abstract void PostSetup(GameModeSettings gameModeSettings);

    public List<CharRankData> GetRankedCharacters()
    {
        if (characterRanks == null)
            return null;

        characterRanks.Sort((charRankA, charRankB) => charRankA.Points.CompareTo(charRankB));

        return characterRanks;
    }

    protected void OnGameFinished()
    {
        //! For now, just load the main menu again
        SceneController.Instance.TransitionScene(SceneController.Level.MainMenu);

        // Load the results screen (make sure to make the MainGame scene a prereq of it)

        // Pass through the character ranks to the results screen
    }
}
