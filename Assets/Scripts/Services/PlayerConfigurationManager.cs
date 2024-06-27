using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : Singleton<PlayerConfigurationManager>
{
    [Header("References")]
    [SerializeField] private PlayerInputManager playerInputManager;

    [Header("Settings")]
    [SerializeField] private int maxPlayers;

    [Header("Game Config")]
    public GameModeSettings chosenMode;
    public LevelData chosenLevel;

    private List<PlayerConfiguration> playerConfigs;

    public static UnityAction<PlayerConfiguration> OnPlayerJoined;
    public static UnityAction<PlayerConfiguration> OnPlayerCamSetup;

    protected override void Awake()
    {
        base.Awake();

        playerConfigs = new List<PlayerConfiguration>();
    }

    public List<PlayerConfiguration> GetPlayerConfigurations() => playerConfigs;

    public void SetPlayerCount(int count)
    {
        maxPlayers = count;
    }

    public int GetPlayerCount()
    {
        return playerConfigs.Count;
    }

    public void SetPlayerCar(int index, CarSettings settings)
    {
        playerConfigs[index].CarSettings = settings;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        if (playerConfigs.Count == maxPlayers && playerConfigs.All(player => player.IsReady))
        {
            SceneController.Instance.TransitionScene(chosenLevel.level);
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

            if (playerConfigs.Count == maxPlayers)
                playerInputManager.DisableJoining();

            OnPlayerJoined?.Invoke(playerConfigs[playerConfigs.Count - 1]);
        }
    }

    public void SetPlayerCam(int index, Camera cam)
    {
        playerConfigs[index].Camera = cam;

        OnPlayerCamSetup?.Invoke(playerConfigs[index]);
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
    public Camera Camera;
}