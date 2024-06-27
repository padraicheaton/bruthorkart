using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawns;
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        GameModeSettings chosenGameMode = PlayerConfigurationManager.Instance.chosenMode;

        GameObject modeManagerObj = Instantiate(chosenGameMode.managerPrefab, transform.position, Quaternion.identity);

        SceneController.Instance.MoveObjToScene(modeManagerObj, SceneController.Level.MainGame);

        modeManagerObj.GetComponent<BaseGameMode>().Setup(chosenGameMode);

        List<PlayerConfiguration> playerConfigurations = PlayerConfigurationManager.Instance.GetPlayerConfigurations();

        for (int i = 0; i < playerConfigurations.Count; i++)
        {
            GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation);

            SceneController.Instance.MoveObjToScene(player, SceneController.Level.MainGame);

            player.GetComponent<PlayerController>().Initialise(playerConfigurations[i]);
        }
    }
}
