using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [SerializeField] private List<ItemData> allItems = new();

    private Dictionary<string, ItemData> itemsByID = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDatabase()
    {
        itemsByID.Clear();
        foreach (var item in allItems)
        {
            if (!itemsByID.ContainsKey(item.itemID))
            {
                itemsByID.Add(item.itemID, item);
            }
        }
    }

    public ItemData GetItemByID(string itemID)
    {
        if (itemsByID.TryGetValue(itemID, out var item))
        {
            return item;
        }
        Debug.LogWarning($"Item with ID '{itemID}' not found in database!");
        return null;
    }

    public List<ItemData> GetAllItems()
    {
        return allItems;
    }
}
