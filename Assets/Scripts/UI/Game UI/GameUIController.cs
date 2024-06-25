using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform splitScreenParent;
    [SerializeField] private GameObject playerScreenPrefab;

    private void Awake()
    {
        PlayerConfigurationManager.OnPlayerCamSetup += CreateScreenForPlayer;
    }

    private void CreateScreenForPlayer(PlayerConfiguration player)
    {
        GameObject playerScreen = Instantiate(playerScreenPrefab, splitScreenParent);

        if (playerScreen.TryGetComponent<SplitScreenRend>(out SplitScreenRend splitScreenRend))
        {
            splitScreenRend.Setup(player, PlayerConfigurationManager.Instance.GetPlayerCount());
        }
    }
}
