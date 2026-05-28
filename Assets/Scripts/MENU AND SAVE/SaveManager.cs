using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public bool HasSave()
    {
        return PlayerPrefs.HasKey("PlayerX");
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SaveGame(Transform player,List<ZoneInfo> zones)
    {
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        for (int i = 0; i < zones.Count; i++)
        {
            PlayerPrefs.SetInt("Zone_" + i, zones[i].discovered ? 1 : 0);
        }

        PlayerPrefs.SetFloat("MasterVolume", AudioManager3D.Instance.masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", AudioManager3D.Instance.musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager3D.Instance.sfxVolume);

        PlayerPrefs.Save();

        //saveData.activeMissions = MissionManager.Instance.GetActiveMissionIDs();
        //saveData.completedMissions = MissionManager.Instance.GetCompletedMissionIDs();

         // --- SAVE MISSIONS ---

        var active = MissionManager.Instance.GetActiveMissionIDs();
        var completed = MissionManager.Instance.GetCompletedMissionIDs();

        // Convert to string
        string activeString = string.Join(",", active);
        string completedString = string.Join(",", completed);

        PlayerPrefs.SetString("ActiveMissions", activeString);
        PlayerPrefs.SetString("CompletedMissions", completedString);

        // --- SAVE INVENTORY ---
        SaveInventory();

        //Debug.Log("Game Saved");
    }

    public void LoadGame(Transform player, List<ZoneInfo> zones)
    {
        if (!PlayerPrefs.HasKey("PlayerX")) return;

        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat("PlayerX"),
            PlayerPrefs.GetFloat("PlayerY"),
            PlayerPrefs.GetFloat("PlayerZ")
        );

        player.position = pos;

        //MissionManager.Instance.LoadMissions(saveData.activeMissions, saveData.completedMissions);

        AudioManager3D.Instance.masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioManager3D.Instance.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        AudioManager3D.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        for (int i = 0; i < zones.Count; i++)
        {
            int value = PlayerPrefs.GetInt("Zone_" + i, 0);
            zones[i].discovered = (value == 1);

            if (zones[i].mapIcon != null)
                zones[i].mapIcon.SetActive(zones[i].discovered);
        }

        // --- LOAD MISSIONS ---

        string activeString = PlayerPrefs.GetString("ActiveMissions", "");
        string completedString = PlayerPrefs.GetString("CompletedMissions", "");

        // Convert back to lists
        List<string> active = new List<string>();
        List<string> completed = new List<string>();

        if (!string.IsNullOrEmpty(activeString))
            active = activeString.Split(',').ToList();

        if (!string.IsNullOrEmpty(completedString))
            completed = completedString.Split(',').ToList();

        MissionManager.Instance.LoadMissions(active, completed);

        // --- LOAD INVENTORY ---
        LoadInventory();

        //Debug.Log("Game Loaded");
    }

    public void SaveUI(List<UIEditable> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var rect = elements[i].GetComponent<RectTransform>();
            var canvas = elements[i].GetComponent<CanvasGroup>();

            PlayerPrefs.SetFloat("UI_PosX_" + i, rect.anchoredPosition.x);
            PlayerPrefs.SetFloat("UI_PosY_" + i, rect.anchoredPosition.y);
            PlayerPrefs.SetFloat("UI_Scale_" + i, rect.localScale.x);
            PlayerPrefs.SetFloat("UI_Alpha_" + i, canvas.alpha);
        }
    }

    public void LoadUI(List<UIEditable> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var rect = elements[i].GetComponent<RectTransform>();
            var canvas = elements[i].GetComponent<CanvasGroup>();

            rect.anchoredPosition = new Vector2(
                PlayerPrefs.GetFloat("UI_PosX_" + i, rect.anchoredPosition.x),
                PlayerPrefs.GetFloat("UI_PosY_" + i, rect.anchoredPosition.y)
            );

            float scale = PlayerPrefs.GetFloat("UI_Scale_" + i, 1f);
            rect.localScale = Vector3.one * scale;

            canvas.alpha = PlayerPrefs.GetFloat("UI_Alpha_" + i, 1f);
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        //Debug.Log("Save Deleted");
    }

    public void ClearSpawnPointSave()
    {
        PlayerPrefs.DeleteKey("LastSpawnPointX");
        PlayerPrefs.DeleteKey("LastSpawnPointY");
        PlayerPrefs.DeleteKey("LastSpawnPointZ");
        PlayerPrefs.DeleteKey("LastSpawnPointRotX");
        PlayerPrefs.DeleteKey("LastSpawnPointRotY");
        PlayerPrefs.DeleteKey("LastSpawnPointRotZ");
        PlayerPrefs.DeleteKey("LastSpawnPointRotW");
        PlayerPrefs.Save();
    }

    void SaveInventory()
    {
        var items = InventoryManager.Instance.items;
        List<string> itemIDs = new List<string>();

        foreach (var item in items)
        {
            itemIDs.Add(item.itemID);
        }

        string inventoryString = string.Join(",", itemIDs);
        PlayerPrefs.SetString("Inventory", inventoryString);
        
        // Save collected pickupables
        string collectedString = string.Join(",", InventoryManager.Instance.collectedPickupableIDs);
        PlayerPrefs.SetString("CollectedPickupables", collectedString);
        
        PlayerPrefs.Save();
    }

    void LoadInventory()
    {
        if (ItemDatabase.Instance == null)
        {
            Debug.LogError("ItemDatabase not found! Cannot load inventory. Make sure ItemDatabase exists in the scene.");
            return;
        }

        InventoryManager.Instance.items.Clear();
        InventoryManager.Instance.collectedPickupableIDs.Clear();

        string inventoryString = PlayerPrefs.GetString("Inventory", "");

        if (string.IsNullOrEmpty(inventoryString))
            return;

        List<string> itemIDs = new List<string>(inventoryString.Split(','));

        foreach (var itemID in itemIDs)
        {
            if (string.IsNullOrEmpty(itemID))
                continue;

            ItemData item = ItemDatabase.Instance.GetItemByID(itemID);
            if (item != null)
            {
                InventoryManager.Instance.items.Add(item);
            }
        }

        // Load collected pickupables
        string collectedString = PlayerPrefs.GetString("CollectedPickupables", "");
        if (!string.IsNullOrEmpty(collectedString))
        {
            InventoryManager.Instance.collectedPickupableIDs = new List<string>(collectedString.Split(','));
        }

        // Destroy pickupables that were already collected
        Pickupable[] allPickupables = FindObjectsOfType<Pickupable>();
        foreach (var pickupable in allPickupables)
        {
            if (InventoryManager.Instance.collectedPickupableIDs.Contains(pickupable.GetPickupableID()))
            {
                Destroy(pickupable.gameObject);
            }
        }

        InventoryManager.Instance.onInventoryChanged?.Invoke();
    }
}