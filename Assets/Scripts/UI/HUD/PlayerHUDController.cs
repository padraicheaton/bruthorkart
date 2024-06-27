using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup resettingScreen;

    private PlayerConfiguration playerConfig;

    public void Setup(PlayerConfiguration config)
    {
        playerConfig = config;

        WorldBorderTrigger.OnPlayerPassedWorldBorder += OnPlayerFallen;

        GameObject modeSpecificHUD = BaseGameMode.Instance.GetModeHUD();

        if (modeSpecificHUD != null)
        {
            GameObject hud = Instantiate(modeSpecificHUD, transform);
            hud.GetComponent<IModeHUDController>().Setup(config);
        }
    }

    private void OnPlayerFallen(PlayerConfiguration config)
    {
        if (playerConfig.PlayerIndex == config.PlayerIndex)
            StartCoroutine(ShowScreenWhilstResetting());
    }

    private IEnumerator ShowScreenWhilstResetting()
    {
        while (resettingScreen.alpha < 1f)
        {
            resettingScreen.alpha += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(GlobalSettings.PlayerResetDelay);

        while (resettingScreen.alpha > 0f)
        {
            resettingScreen.alpha -= Time.deltaTime;
            yield return null;
        }
    }
}
