using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class PassportItem
{
    public string name;
    public string formula;
    public Sprite image;
    public string type; // "Element", "Molekül Element", "Bileşik"
}

public class PassportPeopleGame : MonoBehaviour
{
    [Header("UI - Passport Panel (tek panel kullanıyoruz)")]
    public GameObject passportPanel;
    public TMP_Text nameText;
    public TMP_Text formulaText;
    public Image passportImage;
    public TMP_Text feedbackText;

    [Header("Person Buttons (sahnedeki 6 kişi)")]
    public GameObject[] personButtons;    // Her birinin üzerinde Image olmalı

    [Header("Image Holder (büyük resim göstermek için)")]
    public Image imageHolder; // Inspector’a UI Image objesi atayacaksın

    [Header("Seçim Butonları (GameObject olarak)")]
    public GameObject elementCityButtonGo;
    public GameObject compoundCityButtonGo;

    [Header("Oyun ayarları")]
    public List<PassportItem> allItems = new List<PassportItem>();
    public int pickCount = 6;
    public int startingLives = 3;
    
    [Header("Manual Selection")]
    public bool useManualSelection = false; // true = inspector'dan seç, false = random
    public List<PassportItem> manuallySelectedItems = new List<PassportItem>(); // inspector'dan seçim için

    public GameObject butonholder;

    [Header("Oyun Sonu")]
    public GameObject restartButtonGo;
    
    [Header("Başarı Sahnesi (tüm oyun tamamlanınca)")]
    public GameObject successObject1;
    public GameObject successObject2;

    private List<PassportItem> chosenItems = new List<PassportItem>();
    private int lives;
    private int currentViewedPersonIndex = -1;
    private bool[] personAnswered;

    void Start()
    {
        // Don't auto-load; only populate allItems if using manual selection
        if (allItems.Count == 0)
            Debug.LogWarning("PassportPeopleGame: allItems boş! Inspector'dan element/bileşik ekleyin.");

        butonholder.SetActive(true);

        passportPanel.SetActive(false);
        elementCityButtonGo.SetActive(false);
        compoundCityButtonGo.SetActive(false);
        restartButtonGo.SetActive(false);
        feedbackText.text = "";

        personAnswered = new bool[personButtons.Length];

        for (int i = 0; i < personButtons.Length; i++)
        {
            int idx = i;
            if (personButtons[i] == null) continue;
            Button b = personButtons[i].GetComponent<Button>();
            if (b == null) b = personButtons[i].GetComponentInChildren<Button>();
            if (b != null)
            {
                b.onClick.AddListener(() => OnPersonClicked(idx));
            }
        }

        PrepareNewRound();
    }

    void LoadDefaultItems()
    {
        // Bu method artık inspector'dan manuel yapılan seçimi desteklemek için isteğe bağlı
        // Eğer allItems boşsa ve useManualSelection false ise, burada default öğeler ekleyebilirsin
        if (allItems.Count > 0) return; // zaten dolu ise, yapma
        
        // Opsiyonel: default öğeler (resimsiz örnek)
        allItems.Add(new PassportItem { name = "Helyum", formula = "He", type = "Element" });
        allItems.Add(new PassportItem { name = "Neon", formula = "Ne", type = "Element" });
        allItems.Add(new PassportItem { name = "Su", formula = "H2O", type = "Bileşik" });
        allItems.Add(new PassportItem { name = "Tuz", formula = "NaCl", type = "Bileşik" });
    }

    public void PrepareNewRound()
    {
        restartButtonGo.SetActive(false);
        if (successObject1 != null) successObject1.SetActive(false);
        if (successObject2 != null) successObject2.SetActive(false);
        
        passportPanel.SetActive(false);
        imageHolder.sprite = null;
        imageHolder.gameObject.SetActive(false);

        elementCityButtonGo.SetActive(false);
        compoundCityButtonGo.SetActive(false);

        lives = startingLives;
        for (int i = 0; i < personAnswered.Length; i++) personAnswered[i] = false;
        foreach (var go in personButtons) if (go != null) go.SetActive(true);

        // Manual seçim veya random seçim
        if (useManualSelection && manuallySelectedItems.Count > 0)
        {
            chosenItems = new List<PassportItem>(manuallySelectedItems);
        }
        else if (allItems.Count > 0)
        {
            chosenItems = allItems.OrderBy(x => Random.value).Take(pickCount).ToList();
        }
        else
        {
            Debug.LogError("PassportPeopleGame: Hiçbir öğe bulunmuyor! Inspector'dan öğe ekleyin.");
            chosenItems = new List<PassportItem>();
        }

        // 🔹 Her butonun üstüne kendi resmini koy
        for (int i = 0; i < personButtons.Length && i < chosenItems.Count; i++)
        {
            Image img = personButtons[i].GetComponent<Image>();
            if (img != null)
            {
                img.sprite = chosenItems[i].image;
                img.enabled = true;
            }
        }

        currentViewedPersonIndex = -1;
        UpdateLivesUI();
    }

    void UpdateLivesUI()
    {
        feedbackText.text = $"Can: {lives}";
    }

    public void OnPersonClicked(int personIndex)
    {
        butonholder.SetActive(false);
        if (personIndex < 0 || personIndex >= chosenItems.Count) return;
        if (personAnswered[personIndex]) return;

        currentViewedPersonIndex = personIndex;
        PassportItem it = chosenItems[personIndex];

        passportPanel.SetActive(true);
        elementCityButtonGo.SetActive(true);
        compoundCityButtonGo.SetActive(true);

        nameText.text = it.name;
        formulaText.text = (IsElementType(it.type) ? "Sembol: " : "Formül: ") + it.formula;
        passportImage.sprite = it.image;

        // 🔹 ImageHolder’da büyük resmi göster
        if (imageHolder != null)
        {
            imageHolder.sprite = it.image;
            imageHolder.gameObject.SetActive(true);
        }
    }

    public void OnAnswer(bool isElementCity)
    {
        if (currentViewedPersonIndex < 0) return;

        PassportItem it = chosenItems[currentViewedPersonIndex];
        bool itIsElement = IsElementType(it.type);

        if ((itIsElement && isElementCity) || (!itIsElement && !isElementCity))
        {
            feedbackText.text = "✅ Doğru!";
            passportPanel.SetActive(false);
            elementCityButtonGo.SetActive(false);
            compoundCityButtonGo.SetActive(false);
            butonholder.SetActive(true);

            // 🔹 Resim de kaybolsun
            if (imageHolder != null) imageHolder.gameObject.SetActive(false);

            if (personButtons[currentViewedPersonIndex] != null)
                personButtons[currentViewedPersonIndex].SetActive(false);

            personAnswered[currentViewedPersonIndex] = true;
            currentViewedPersonIndex = -1;

            if (AllAnswered())
                OnAllCompleted();
        }
        else
        {
            lives--;
            if (lives <= 0)
            {
                feedbackText.text = "Tüm canlar tükendi. Oyun bitti.";
                passportPanel.SetActive(false);
                foreach (var go in personButtons) if (go != null) go.SetActive(false);
                elementCityButtonGo.SetActive(false);
                compoundCityButtonGo.SetActive(false);
                restartButtonGo.SetActive(true);
                imageHolder.gameObject.SetActive(false);
            }
            else feedbackText.text = $"Yanlış! Kalan can: {lives}";
        }
        UpdateLivesUI();
    }

    bool IsElementType(string type)
    {
        if (string.IsNullOrWhiteSpace(type)) return false;
        string normalized = type.Trim().ToLower();
        return normalized.Contains("element");

        // "molekül element" de element olarak kabul edilir
        //return normalized.Contains("element");
    }

    bool AllAnswered()
    {
        for (int i = 0; i < personAnswered.Length && i < chosenItems.Count; i++)
            if (!personAnswered[i]) return false;
        return true;
    }

    void OnAllCompleted()
    {
        feedbackText.text = "🎉 Tebrikler! Herkes doğru şehre yerleşti!";
        elementCityButtonGo.SetActive(false);
        compoundCityButtonGo.SetActive(false);
        
        // Başarı nesnelerini aktif et
        if (successObject1 != null) successObject1.SetActive(true);
        if (successObject2 != null) successObject2.SetActive(true);
    }

    public void OnRestartButtonClicked()
    {
        PrepareNewRound();
    }
}
