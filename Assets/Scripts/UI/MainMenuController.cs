using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button defaultMenuFirstSelect;
    [SerializeField] private Button modeSelectFirstSelect;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField] private CanvasGroup modeSelectParent;

    private bool isFirstPlayerSetup = false;

    private void Awake()
    {
        PlayerConfigurationManager.OnPlayerJoined += OnPlayerJoined;

        OnModeSelectClose();
    }

    private void OnPlayerJoined(PlayerConfiguration playerConfiguration)
    {
        if (isFirstPlayerSetup)
            return;

        playerConfiguration.Input.uiInputModule = inputSystemUIInputModule;
        defaultMenuFirstSelect.Select();

        isFirstPlayerSetup = true;
    }

    public void OnModeSelectOpen()
    {
        modeSelectParent.alpha = 1f;
        modeSelectParent.interactable = modeSelectParent.blocksRaycasts = true;
        modeSelectFirstSelect.Select();
    }

    public void OnModeSelectClose()
    {
        modeSelectParent.alpha = 0f;
        modeSelectParent.interactable = modeSelectParent.blocksRaycasts = false;
        defaultMenuFirstSelect.Select();
    }

    public void OnPlayBtnPressed(int playerCount)
    {
        PlayerConfigurationManager.Instance.SetPlayerCount(playerCount);
        SceneController.Instance.TransitionScene(SceneController.Level.CharSelection);
    }

    public void OnExitBtnPressed()
    {

    }
}
