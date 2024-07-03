using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;
    private CarController carController;
    private ItemData heldItem;

    public UnityAction<ItemData> OnHeldItemUpdated;

    public void Setup(PlayerConfiguration config)
    {
        playerConfig = config;

        BaseGameMode.Instance.OnItemPickedUp += OnItemPickedUp;

        carController = GetComponent<CarController>();
    }

    private void OnItemPickedUp(ItemData item, int playerID)
    {
        if (playerConfig.PlayerIndex != playerID)
            return;

        if (heldItem != null)
            return;

        SetHeldItem(item);
    }

    public void TryUseItem()
    {
        if (heldItem == null)
            return;

        heldItem.OnItemUsed(carController, carController.transform.forward);

        SetHeldItem(null);
    }

    private void SetHeldItem(ItemData item)
    {
        heldItem = item;

        OnHeldItemUpdated?.Invoke(heldItem);
    }

    public bool HasItem() => heldItem != null;
}
