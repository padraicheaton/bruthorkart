using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private PlayerHUDController playerHUDController;
    private PlayerConfiguration playerConfig;
    private PlayerActions inputActions;

    private CarController carController;

    public void Initialise(PlayerConfiguration playerConfiguration)
    {
        playerConfig = playerConfiguration;

        carController = GetComponent<CarController>();

        carController.SetSettings(playerConfig.CarSettings);

        inputActions = new PlayerActions();

        playerConfig.Input.onActionTriggered += HandleInput;

        SetupSplitCam();

        playerHUDController.Setup(playerConfig);
    }

    private Rect[] TwoPlayerCamSettings = {
        new Rect(0, 0, 0.5f, 1f), // Left
        new Rect(0.5f, 0, 0.5f, 1f) // Right
     };

    private Rect[] ThreePlayerCamSettings = {
        new Rect(0, 0.5f, 0.5f, 0.5f), // Top, Left
        new Rect(0.5f, 0.5f, 0.5f, 0.5f), // Top, Right
        new Rect(0.25f, 0f, 0.5f, 0.5f), // Bottom, Middle
    };

    private Rect[] FourPlayerCamSettings = {
        new Rect(0, 0.5f, 0.5f, 0.5f), // Top, Left
        new Rect(0.5f, 0.5f, 0.5f, 0.5f), // Top, Right
        new Rect(0f, 0f, 0.5f, 0.5f), // Bottom, Left
        new Rect(0.5f, 0f, 0.5f, 0.5f) // Bottom, Right
    };
    private void SetupSplitCam()
    {
        int playerCount = PlayerConfigurationManager.Instance.GetPlayerCount();

        if (playerCount == 1)
            return;

        else if (playerCount == 2)
            playerCam.rect = TwoPlayerCamSettings[playerConfig.PlayerIndex];

        else if (playerCount == 3)
            playerCam.rect = ThreePlayerCamSettings[playerConfig.PlayerIndex];

        else
            playerCam.rect = FourPlayerCamSettings[playerConfig.PlayerIndex];
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (context.action.name == inputActions.Driving.Accelerate.name)
            carController.UpdateAccelerateInput(context.ReadValue<float>());

        if (context.action.name == inputActions.Driving.Steering.name)
            carController.UpdateSteeringInput(context.ReadValue<float>());
    }

    public PlayerConfiguration GetConfig() => playerConfig;
}
