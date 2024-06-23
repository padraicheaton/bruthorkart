using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
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

        // NEED TO CHANGE THIS LATER
        if (playerConfig.PlayerIndex == 0)
        {
            playerCam.rect = new Rect(0, 0, 0.5f, 1f);
        }
        else if (playerConfig.PlayerIndex == 1)
        {
            playerCam.rect = new Rect(0.5f, 0, 0.5f, 1f);
        }
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (context.action.name == inputActions.Driving.Accelerate.name)
            carController.UpdateAccelerateInput(context.ReadValue<float>());

        if (context.action.name == inputActions.Driving.Steering.name)
            carController.UpdateSteeringInput(context.ReadValue<float>());
    }

    // public void OnAccelerateInput(InputAction.CallbackContext context)
    // {
    //     if (carController)
    //         carController.UpdateAccelerateInput(context.ReadValue<float>());
    // }

    // public void OnSteeringInput(InputAction.CallbackContext context)
    // {
    //     if (carController)
    //         carController.UpdateSteeringInput(context.ReadValue<float>());
    // }
}
