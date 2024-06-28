using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup resettingScreen;
    [SerializeField] private Image itemIconImg;

    private PlayerConfiguration playerConfig;

    public void Setup(PlayerConfiguration config, ItemHandler itemHandler)
    {
        playerConfig = config;

        WorldBorderTrigger.OnPlayerPassedWorldBorder += OnPlayerFallen;

        GameObject modeSpecificHUD = BaseGameMode.Instance.GetModeHUD();

        if (modeSpecificHUD != null)
        {
            GameObject hud = Instantiate(modeSpecificHUD, transform);
            hud.GetComponent<IModeHUDController>().Setup(config);
        }

        itemHandler.OnHeldItemUpdated += UpdateItemIcon;

        UpdateItemIcon(null);
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

    private void UpdateItemIcon(ItemData item)
    {
        itemIconImg.color = item != null ? Color.white : Color.clear;

        if (item != null)
            itemIconImg.sprite = item.icon;
    }
}
