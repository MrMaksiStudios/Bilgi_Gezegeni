using UnityEngine;

public enum ElementCategory
{
    Metal,
    NonMetal,
    Metalloid
}

public class ElementInfo : MonoBehaviour
{
    [Header("Basic Info")]
    public string elementName;      // "Oksijen"
    public string symbol;           // "O"
    public ElementCategory category;

    [Header("Atomic Structure")]
    public int protons;
    public int electrons;

    [Header("Charge")]
    public int charge;              // -1, 0, +1 gibi

    [Header("Visual")]
    public Sprite sprite;        // 👈 BU ŞART
}
