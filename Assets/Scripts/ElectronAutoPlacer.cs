using System.Collections.Generic;
using UnityEngine;

public class ElectronAutoPlacer : MonoBehaviour
{
    private Vector3 startPosition;

    [Header("Electron Prefabs")]
    public List<GameObject> allElectrons; // Sahnedeki tüm elektronlar

    [Header("Electron Type Flags")]
    public bool isHeElectron;
    public bool isNeElectron;
    public bool isArElectron;

    [Header("Orbital Slot Points")]
    public List<Transform> heSlots;
    public List<Transform> neSlots;
    public List<Transform> arSlots;

    [Header("Yerleşme Hızı")]
    public float moveSpeed = 5f;

    // ========== FONKSİYONLAR ==========

    void Start()
    {
        startPosition = transform.position;
    }

    public void PlaceHeliumElectrons()
    {
        StartCoroutine(AutoPlaceElectrons("He", heSlots));
    }

    public void PlaceNeonElectrons()
    {
        StartCoroutine(AutoPlaceElectrons("Ne", neSlots));
    }

    public void PlaceArgonElectrons()
    {
        StartCoroutine(AutoPlaceElectrons("Ar", arSlots));
    }

    public void ReturnToStartPosition()
    {
        transform.position = startPosition;
    }

    // ===== Yeni eklenen fonksiyon =====
    public void ResetAllElectrons()
    {
        foreach (GameObject e in allElectrons)
        {
            e.transform.position = startPosition;
        }
    }

    // ========== ANA OTOMATİK YERLEŞTİRME KORUTİNİ ========== 
    private System.Collections.IEnumerator AutoPlaceElectrons(string gasType, List<Transform> slots)
    {
        List<GameObject> electronsToMove = new List<GameObject>();

        foreach (GameObject e in allElectrons)
        {
            var comp = e.GetComponent<ElectronAutoPlacer>();
            if (comp == null) continue;

            if ((gasType == "He" && comp.isHeElectron) ||
                (gasType == "Ne" && comp.isNeElectron) ||
                (gasType == "Ar" && comp.isArElectron))
            {
                electronsToMove.Add(e);
            }
        }

        for (int i = 0; i < electronsToMove.Count && i < slots.Count; i++)
        {
            GameObject e = electronsToMove[i];
            Transform target = slots[i];

            float t = 0f;
            Vector3 start = e.transform.position;

            while (t < 1f)
            {
                t += Time.deltaTime * moveSpeed;
                e.transform.position = Vector3.Lerp(start, target.position, t);
                yield return null;
            }

            e.transform.position = target.position;
        }
    }
}
