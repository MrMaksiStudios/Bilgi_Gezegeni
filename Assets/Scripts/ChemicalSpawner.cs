using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChemicalSpawner : MonoBehaviour
{
    [Header("Metaller (5 adet) - sahnedeki GameObjectler")]
    public GameObject[] metals;

    [Header("Ametaller (5 adet) - sahnedeki GameObjectler")]
    public GameObject[] nonMetals;

    [Header("UI")]
    public TextMeshProUGUI taskText1;
    public TextMeshProUGUI taskText2;
    public TextMeshProUGUI taskText3;

    [Header("Oyun")]
    public int lives = 3;
    private int currentLevel = 0;
    private int correctBonds = 0;

    private string[] tasks = { "Kovalent bağ oluşturun", "İyonik bağ oluşturun", "Metalik bağ oluşturun" };

    void Start()
    {
        ActivateRandomObjects(metals, 3);
        ActivateRandomObjects(nonMetals, 3);
        UpdateTasks();
    }

    void UpdateTasks()
    {
        if (correctBonds >= 3)
        {
            taskText1.text = "Oyun Bitti!";
            taskText2.text = "";
            taskText3.text = "";
            return;
        }
        taskText1.text = tasks[currentLevel % tasks.Length];
        taskText2.text = "";
        taskText3.text = "";
    }

    void ActivateRandomObjects(GameObject[] objects, int count)
    {
        foreach (GameObject obj in objects)
            if (obj != null) obj.SetActive(false);

        List<GameObject> list = new List<GameObject>(objects);

        for (int i = 0; i < count && list.Count > 0; i++)
        {
            int r = Random.Range(0, list.Count);
            if (list[r] != null) list[r].SetActive(true);
            list.RemoveAt(r);
        }
    }

    // DragDrop çağıracak
    public void CheckBond(GameObject obj1, GameObject obj2)
    {
        if (correctBonds >= 3) return;
        if (obj1 == null || obj2 == null) return;

        // ElementInfo'ları al
        ElementInfo e1 = obj1.GetComponent<ElementInfo>();
        ElementInfo e2 = obj2.GetComponent<ElementInfo>();

        string cat1 = e1 != null ? e1.category.ToString() : "Unknown";
        string cat2 = e2 != null ? e2.category.ToString() : "Unknown";

        string task = tasks[currentLevel % tasks.Length];

        Debug.Log($"CheckBond çağrıldı: {obj1.name} [{cat1}] + {obj2.name} [{cat2}]  (Görev: {task})");

        bool correct = false;

        // Eğer komponent yoksa hemen false ve uyarı
        if (e1 == null || e2 == null)
        {
            Debug.LogWarning("Bir veya iki element üzerinde ElementInfo bulunamadı. Prefablara/objelere ElementInfo ekleyin.");
            correct = false;
        }
        else if (task.Contains("Kovalent"))
        {
            // Her iki element de ametal olmalı
            if (e1.category == ElementCategory.NonMetal && e2.category == ElementCategory.NonMetal)
                correct = true;
        }
        else if (task.Contains("İyonik"))
        {
            // Bir metal bir ametal olmalı
            if ((e1.category == ElementCategory.Metal && e2.category == ElementCategory.NonMetal) ||
                (e1.category == ElementCategory.NonMetal && e2.category == ElementCategory.Metal))
                correct = true;
        }
        else if (task.Contains("Metalik"))
        {
            // İkisi de metal olmalı
            if (e1.category == ElementCategory.Metal && e2.category == ElementCategory.Metal)
                correct = true;
        }

        if (correct)
        {
            Debug.Log($"Doğru bağ: {obj1.name} + {obj2.name} ({task})");
            obj1.SetActive(false);
            obj2.SetActive(false);
            correctBonds++;
            currentLevel++;
            UpdateTasks();
        }
        else
        {
            lives--;
            Debug.Log($"Yanlış bağ! Kalan can: {lives}");
            // Yanlışta kısa uyarı
            if (lives <= 0)
            {
                taskText1.text = "Oyun Bitti!";
                taskText2.text = "";
                taskText3.text = "";
                Debug.Log("Oyun Bitti - canlar bitti.");
            }
        }
    }
}
