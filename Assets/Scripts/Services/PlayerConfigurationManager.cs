using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : Singleton<PlayerConfigurationManager>
{
    private List<PlayerConfiguration> playerConfigs;

    [Header("Settings")]
    [SerializeField] private int maxPlayers;

    protected override void Awake()
    {
        base.Awake();

        playerConfigs = new List<PlayerConfiguration>();
    }

    public List<PlayerConfiguration> GetPlayerConfigurations() => playerConfigs;

    public void SetPlayerCar(int index, CarSettings settings)
    {
        playerConfigs[index].CarSettings = settings;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        if (playerConfigs.Count == maxPlayers && playerConfigs.All(player => player.IsReady))
        {
            SceneController.Instance.LoadScene(SceneController.Level.LevelOne);
        }
    }

    public void HandlePlayerJoin(PlayerInput input)
    {
        Debug.Log($"Player Joined: {input.playerIndex}");

        if (!playerConfigs.Any(player => player.PlayerIndex == input.playerIndex))
        {
            // Move the object to under this one, so that the input comes across scenes
            input.transform.SetParent(transform);

            playerConfigs.Add(new PlayerConfiguration(input));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput input)
    {
        PlayerIndex = input.playerIndex;
        Input = input;
    }

    public PlayerInput Input;
    public int PlayerIndex;
    public bool IsReady;
    public CarSettings CarSettings;
}