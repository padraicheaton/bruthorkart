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

    private void SetupSplitCam()
    {
        PlayerConfigurationManager.Instance.SetPlayerCam(playerConfig.PlayerIndex, playerCam);
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
