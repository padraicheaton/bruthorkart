using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI lapCountTxt;

    private PlayerConfiguration playerConfig;

    public void Setup(PlayerConfiguration config)
    {
        playerConfig = config;

        RaceManager.OnPlayerCompletedLap += OnPlayerCompletedLap;
    }

    public void OnPlayerCompletedLap(RaceManager.RacerData player)
    {
        if (playerConfig.PlayerIndex == player.PlayerID)
        {
            lapCountTxt.text = player.LapsCompleted.ToString();
        }
    }
}
