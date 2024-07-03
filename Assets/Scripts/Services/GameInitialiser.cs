using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawns;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject aiPrefab;

    void Start()
    {
        GameModeSettings chosenGameMode = PlayerConfigurationManager.Instance.chosenMode;

        GameObject modeManagerObj = Instantiate(chosenGameMode.managerPrefab, transform.position, Quaternion.identity);

        SceneController.Instance.MoveObjToScene(modeManagerObj, SceneController.Level.MainGame);

        modeManagerObj.GetComponent<BaseGameMode>().Setup(chosenGameMode);

        List<PlayerConfiguration> playerConfigurations = PlayerConfigurationManager.Instance.GetPlayerConfigurations();

        for (int i = 0; i < playerConfigurations.Count; i++)
        {
            GameObject player = Instantiate(playerConfigurations[i].IsPlayer ? playerPrefab : aiPrefab, playerSpawns[i].position, playerSpawns[i].rotation);

            SceneController.Instance.MoveObjToScene(player, SceneController.Level.MainGame);

            player.GetComponent<BaseController>().Initialise(playerConfigurations[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Transform spawnPoint in playerSpawns)
        {
            Gizmos.DrawSphere(spawnPoint.position, 0.5f);
        }
    }
}
