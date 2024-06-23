using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform setupMenuContainer;
    [SerializeField] private GameObject setupMenuPrefab;

    private void Start()
    {
        PlayerConfigurationManager.OnPlayerJoined += AddNewSelectionMenu;

        foreach (PlayerConfiguration config in PlayerConfigurationManager.Instance.GetPlayerConfigurations())
        {
            AddNewSelectionMenu(config);
        }
    }

    private void AddNewSelectionMenu(PlayerConfiguration playerConfiguration)
    {
        GameObject setupMenu = Instantiate(setupMenuPrefab, setupMenuContainer);
        setupMenu.GetComponent<PlayerSetupMenuController>().SetupPlayer(playerConfiguration);
    }
}
