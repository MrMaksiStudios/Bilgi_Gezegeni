using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    [Header("Slots (Assign in Inspector)")]
    public List<Image> slots = new();

    void Start()
    {
        inventoryPanel.SetActive(false);
        InventoryManager.Instance.onInventoryChanged += RefreshUI;
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        RefreshUI();
    }

    void RefreshUI()
    {
        // Clear all slots
        foreach (var slot in slots)
        {
            slot.sprite = null;
            slot.enabled = false;
        }

        // Fill slots in order
        var items = InventoryManager.Instance.items;

        for (int i = 0; i < items.Count && i < slots.Count; i++)
        {
            slots[i].sprite = items[i].icon;
            slots[i].enabled = true;
        }
    }
}