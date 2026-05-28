using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> items = new();
    public List<string> collectedPickupableIDs = new(); // Track which pickupables have been collected

    public System.Action onInventoryChanged;

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemData item, string pickupableID = "")
    {
        items.Add(item);

        // Track which pickupable was collected
        if (!string.IsNullOrEmpty(pickupableID))
        {
            collectedPickupableIDs.Add(pickupableID);
        }

        PickupUIManager.Instance.ShowPickupText(item.itemName);

        onInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            onInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(string itemID)
    {
        foreach (var item in items)
        {
            if (item.itemID == itemID)
                return true;
        }
        return false;
    }
}