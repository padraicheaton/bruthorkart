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

    public void Initialise(PlayerConfiguration playerConfiguration)
    {
        playerConfig = playerConfiguration;

        carController = GetComponent<CarController>();

        carController.SetSettings(playerConfig.CarSettings);

        inputActions = new PlayerActions();

        playerConfig.Input.onActionTriggered += HandleInput;

        SetupSplitCam();

        playerHUDController.Setup(playerConfig);

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

        Transform resetPos = (RaceGameController.Instance as RaceGameController).GetClosestCheckpointToPlayerLastGrounded(playerConfig.PlayerIndex);
        Debug.Log(resetPos);
        carController.OverridePosition(resetPos.position + Vector3.up * 5f, resetPos.rotation);
    }

    public PlayerConfiguration GetConfig() => playerConfig;
}
