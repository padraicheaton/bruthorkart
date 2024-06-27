using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI lapCountTxt;
    [SerializeField] private CanvasGroup resettingScreen;

    private PlayerConfiguration playerConfig;

    public void Setup(PlayerConfiguration config)
    {
        playerConfig = config;

        RaceGameController.OnPlayerCompletedLap += OnPlayerCompletedLap;

        WorldBorderTrigger.OnPlayerPassedWorldBorder += OnPlayerFallen;

        lapCountTxt.text = "0/" + (RaceGameController.Instance as RaceGameController).GetTotalLaps();
    }

    public void OnPlayerCompletedLap(RaceGameController.RacerData player)
    {
        if (playerConfig.PlayerIndex == player.PlayerID)
        {
            lapCountTxt.text = player.LapsCompleted + "/" + (RaceGameController.Instance as RaceGameController).GetTotalLaps();
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
