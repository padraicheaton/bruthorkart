using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected PlayerConfiguration playerConfig;
    protected PlayerActions inputActions;

    protected CarController carController;
    protected ItemHandler itemHandler;

    public PlayerConfiguration GetConfig() => playerConfig;

    public virtual void Initialise(PlayerConfiguration playerConfiguration)
    {
        playerConfig = playerConfiguration;

        carController = GetComponent<CarController>();
        itemHandler = GetComponent<ItemHandler>();

        itemHandler.Setup(playerConfiguration);

        carController.SetSettings(playerConfig.CarSettings);

        WorldBorderTrigger.OnPlayerPassedWorldBorder += OnTriggeredWorldBorder;
    }

    private void OnDestroy()
    {
        WorldBorderTrigger.OnPlayerPassedWorldBorder -= OnTriggeredWorldBorder;
    }

    private void OnTriggeredWorldBorder(PlayerConfiguration config)
    {
        StartCoroutine(ResetPlayerAfterDelay(config));
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
}
