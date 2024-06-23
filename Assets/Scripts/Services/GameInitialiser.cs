using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawns;
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        List<PlayerConfiguration> playerConfigurations = PlayerConfigurationManager.Instance.GetPlayerConfigurations();

        for (int i = 0; i < playerConfigurations.Count; i++)
        {
            GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation);

            player.GetComponent<PlayerController>().Initialise(playerConfigurations[i]);
        }
    }
}
