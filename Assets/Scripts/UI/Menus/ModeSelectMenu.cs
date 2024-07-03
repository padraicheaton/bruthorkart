using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ModeSelectMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button firstSelect;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;

    private int modeIndex = 0;

    private void Awake()
    {
        PlayerConfiguration playerOne = PlayerConfigurationManager.Instance.GetPlayerConfigurations()[0];

        playerOne.Input.uiInputModule = inputSystemUIInputModule;
        firstSelect.Select();

        DisplayChosenMode();
    }

    public void PageLeft()
    {
        modeIndex = GameModeDatabase.Instance.Previous(modeIndex);

        DisplayChosenMode();
    }

    public void PageRight()
    {
        modeIndex = GameModeDatabase.Instance.Next(modeIndex);

        DisplayChosenMode();
    }

    public void ConfirmSelection()
    {
        PlayerConfigurationManager.Instance.chosenMode = GameModeDatabase.Instance.Get(modeIndex);

        PlayerConfigurationManager.Instance.chosenLevel = Extensions.GetRandom(GameModeDatabase.Instance.Get(modeIndex).compatibleLevels);

        //TODO Change this
        PlayerConfigurationManager.Instance.PopulateRemainingPlayersWithAI();

        SceneController.Instance.TransitionScene(PlayerConfigurationManager.Instance.chosenLevel.level);
    }

    private void DisplayChosenMode()
    {
        nameTxt.text = GameModeDatabase.Instance.Get(modeIndex).name;
        descriptionTxt.text = GameModeDatabase.Instance.Get(modeIndex).description;
    }
}
