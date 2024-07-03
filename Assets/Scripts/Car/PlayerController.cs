using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerController : BaseController
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private PlayerHUDController playerHUDController;

    public override void Initialise(PlayerConfiguration playerConfiguration)
    {
        base.Initialise(playerConfiguration);

        inputActions = new PlayerActions();

        playerConfig.Input.onActionTriggered += HandleInput;

        SetupSplitCam();

        playerHUDController.Setup(playerConfig, itemHandler);
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

        if (context.action.name == inputActions.Driving.UseItem.name)
            itemHandler.TryUseItem();
    }
}
