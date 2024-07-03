using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject visualsObj;

    [Header("Settings")]
    [SerializeField] private Vector2 respawnMinMax;

    private bool hasBeenPickedUp = false;

    private CoinRunnerGameController Controller => BaseGameMode.Instance as CoinRunnerGameController;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenPickedUp)
            return;

        if (other.transform.parent.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            Controller.PlayerPickedUpCoin(pc.GetConfig().PlayerIndex);

            StartCoroutine(ResetCooldown());
        }
    }

    private IEnumerator ResetCooldown()
    {
        visualsObj.SetActive(false);
        hasBeenPickedUp = true;

        float duration = Random.Range(respawnMinMax.x, respawnMinMax.y);

        yield return new WaitForSeconds(duration);

        visualsObj.SetActive(true);
        hasBeenPickedUp = false;
    }
}
