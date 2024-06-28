using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private PlayerHUDController playerHUDController;
    private PlayerConfiguration playerConfig;
    private PlayerActions inputActions;

    private CarController carController;
    private ItemHandler itemHandler;

    public void Initialise(PlayerConfiguration playerConfiguration)
    {
        playerConfig = playerConfiguration;

        carController = GetComponent<CarController>();
        itemHandler = GetComponent<ItemHandler>();

        itemHandler.Setup(playerConfiguration);

        carController.SetSettings(playerConfig.CarSettings);

        inputActions = new PlayerActions();

        playerConfig.Input.onActionTriggered += HandleInput;

        SetupSplitCam();

        playerHUDController.Setup(playerConfig, itemHandler);

        WorldBorderTrigger.OnPlayerPassedWorldBorder += config => StartCoroutine(ResetPlayerAfterDelay(config));
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

    private IEnumerator ResetPlayerAfterDelay(PlayerConfiguration config)
    {
        yield return new WaitForSeconds(GlobalSettings.PlayerResetDelay);

        ResetPlayerPosition(config);
    }

    private void ResetPlayerPosition(PlayerConfiguration config)
    {
        if (config.PlayerIndex != playerConfig.PlayerIndex)
            return;

        Transform resetPos = BaseGameMode.Instance.GetResetTransform(playerConfig.PlayerIndex);
        carController.OverridePosition(resetPos.position + Vector3.up * 5f, resetPos.rotation);
    }

    public PlayerConfiguration GetConfig() => playerConfig;
}
