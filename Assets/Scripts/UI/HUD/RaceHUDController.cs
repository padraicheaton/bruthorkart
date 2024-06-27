using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceHUDController : MonoBehaviour, IModeHUDController
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI lapCountTxt;

    private PlayerConfiguration playerConfig;

    public void Setup(PlayerConfiguration config)
    {
        playerConfig = config;

        RaceGameController.OnPlayerCompletedLap += OnPlayerCompletedLap;

        lapCountTxt.text = "0/" + (RaceGameController.Instance as RaceGameController).GetTotalLaps();
    }

    public void OnPlayerCompletedLap(RaceGameController.RacerData player)
    {
        if (playerConfig.PlayerIndex == player.PlayerID)
        {
            lapCountTxt.text = player.LapsCompleted + "/" + (RaceGameController.Instance as RaceGameController).GetTotalLaps();
        }
    }
}
