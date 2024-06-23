using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    private void Start()
    {
        confirmBtn.gameObject.SetActive(false);
    }

    public void SetupPlayer(PlayerConfiguration config)
    {
        config.Input.uiInputModule = inputSystemUIInputModule;
        SetPlayerIndex(config.PlayerIndex);
    }

    public void SetPlayerIndex(int playerIndex)
    {
        PlayerIndex = playerIndex;
        titleText.text = $"Player {PlayerIndex + 1}";

        StartCoroutine(EnableInputAfterDelay());
    }

    IEnumerator EnableInputAfterDelay()
    {
        yield return new WaitForSeconds(ignoreInputTime);

        inputEnabled = true;
    }

    public void SetCar(CarSettings carSettings)
    {
        if (!inputEnabled)
            return;

        PlayerConfigurationManager.Instance.SetPlayerCar(PlayerIndex, carSettings);

        confirmBtn.gameObject.SetActive(true);
        confirmBtn.Select();
    }

    public void ReadyPlayer()
    {
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        confirmBtn.gameObject.SetActive(false);

        readyPanel.SetActive(true);
    }
}
