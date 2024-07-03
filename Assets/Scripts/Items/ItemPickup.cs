using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pickupContainer;
    [SerializeField] private float inactiveDuration;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pickup Triggered");

        if (!pickupContainer.gameObject.activeInHierarchy)
            return;

        if (other.transform.parent.TryGetComponent<BaseController>(out BaseController pc))
        {
            BaseGameMode.Instance.OnItemPickedUp?.Invoke(BaseGameMode.Instance.GetRandomItem(), pc.GetConfig().PlayerIndex);

            Debug.Log("Picked Item Up");

            StartCoroutine(ResetCooldown());
        }
    }

    private IEnumerator ResetCooldown()
    {
        pickupContainer.gameObject.SetActive(false);

        yield return new WaitForSeconds(inactiveDuration);

        pickupContainer.gameObject.SetActive(true);
    }
}
