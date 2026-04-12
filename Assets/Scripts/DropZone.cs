using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public AtomGame gameManager;
    private List<GameObject> contents = new List<GameObject>();

    private void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<AtomGame>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Draggable")) return;

        Draggable draggable = other.GetComponent<Draggable>();
        if (draggable == null || draggable.isElectron) return; // Sadece proton

        if (!contents.Contains(draggable.gameObject))
        {
            contents.Add(draggable.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Draggable")) return;

        Draggable draggable = other.GetComponent<Draggable>();
        if (draggable == null || draggable.isElectron) return; // Sadece proton

        if (contents.Contains(draggable.gameObject))
            contents.Remove(draggable.gameObject);
    }
    public int GetProtonCount()
    {
        int count = 0;
        foreach (GameObject g in contents)
        {
            Draggable d = g.GetComponent<Draggable>();
            if (d != null)
            {
                count += d.protonValue;
            }
            else
            {
            }
        }
        return count;
    }
}