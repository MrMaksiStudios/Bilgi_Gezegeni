using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text;

    [Header("Mission Trigger (Optional)")]
    public bool triggerEvent;
    public string eventName; // 🔥 IMPORTANT: uses GameEvents

    [Header("Item Reward (Optional)")]
    public bool givesItem;
    public ItemData itemToGive;
}