using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button firstSelectBtn;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;

    private bool isFirstPlayerSetup = false;

    private void Awake()
    {
        PlayerConfigurationManager.OnPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerConfiguration playerConfiguration)
    {
        if (isFirstPlayerSetup)
            return;

        playerConfiguration.Input.uiInputModule = inputSystemUIInputModule;
        firstSelectBtn.Select();

        isFirstPlayerSetup = true;
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
