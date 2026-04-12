using System.Collections.Generic;
using UnityEngine;

public class OrbitalZone : MonoBehaviour
{
    public enum SpecificOrbital
    {
        OneS,
        TwoS,
        TwoP,
        ThreeS,
        ThreeP,
        FourS,
        ThreeD,
        FourP
    }
    public SpecificOrbital orbitalName;
    [HideInInspector]
    public int capacity;
    public OrbitalZone previousOrbital;
    public AtomGame atomGame;
    private List<Draggable> electronsInZone = new List<Draggable>();

    private void Start()
    {
        SetCapacityByOrbital();
    }
    private void SetCapacityByOrbital()
    {
        switch (orbitalName)
        {
            case SpecificOrbital.OneS:
            case SpecificOrbital.TwoS:
            case SpecificOrbital.ThreeS:
            case SpecificOrbital.FourS:
                capacity = 2;
                break;
            case SpecificOrbital.TwoP:
            case SpecificOrbital.ThreeP:
            case SpecificOrbital.FourP:
                capacity = 6;
                break;
            case SpecificOrbital.ThreeD:
                capacity = 10;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Draggable")) return;
        Draggable d = other.GetComponent<Draggable>();
        if (d == null || !d.isElectron) return;
        if (previousOrbital != null && previousOrbital.HasSpace())
        {
            atomGame.LoseLifeAndResetElectron(d.gameObject);
            return;
        }
        if (GetElectronTotalValue() + d.electronValue > capacity)
        {
            atomGame.LoseLifeAndResetElectron(d.gameObject);
            return;
        }
        if (!electronsInZone.Contains(d))
        {
            electronsInZone.Add(d);
        }
        atomGame.OnElectronPlaced(d.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Draggable")) return;
        Draggable d = other.GetComponent<Draggable>();
        if (d != null && electronsInZone.Contains(d))
        {
            electronsInZone.Remove(d);
        }
    }
    public int GetElectronTotalValue()
    {
        int total = 0;
        foreach (var d in electronsInZone)
        {
            if (d != null)
                total += d.electronValue;
        }
        return total;
    }
    public bool IsFull()
    {
        return GetElectronTotalValue() >= capacity;
    }
    public bool HasSpace()
    {
        return GetElectronTotalValue() < capacity;
    }
    public void Clear()
    {
        foreach (Draggable d in electronsInZone)
        {
            d.ResetToStart();
        }
        electronsInZone.Clear();
    }
}