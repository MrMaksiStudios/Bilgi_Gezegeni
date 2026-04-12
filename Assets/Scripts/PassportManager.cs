using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PassportGameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startButton;
    public GameObject leftPassport;
    public GameObject rightPassport;
    public TMP_Text nameText;
    public TMP_Text formulaText;
    public Image passportImage;

    public GameObject elementButton;
    public GameObject bilesikButton;
    public TMP_Text resultText;

    [Header("Hearts")]
    public List<Image> hearts;

    [Header("Chemical Data")]
    public List<ChemicalData> allChemicals = new List<ChemicalData>();
    private List<ChemicalData> currentList = new List<ChemicalData>();

    private int currentIndex = 0;
    private int lives = 3;

    void Start()
    {
        leftPassport.SetActive(false);
        rightPassport.SetActive(false);
        resultText.text = "";
        elementButton.SetActive(false);
        bilesikButton.SetActive(false);

        elementButton.GetComponent<Button>().onClick.AddListener(() => OnAnswer("Element"));
        bilesikButton.GetComponent<Button>().onClick.AddListener(() => OnAnswer("Bileşik"));
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        leftPassport.SetActive(true);
        rightPassport.SetActive(true);
        elementButton.SetActive(true);
        bilesikButton.SetActive(true);

        currentList = allChemicals.OrderBy(x => Random.value).Take(5).ToList();
        currentIndex = 0;
        lives = 3;
        UpdateHearts();
        ShowPassport();
    }

    void ShowPassport()
    {
        if (currentIndex >= currentList.Count)
        {
            EndGame();
            return;
        }

        ChemicalData data = currentList[currentIndex];

        nameText.text = data.name;
        formulaText.text = data.type == "Element" || data.type == "Molekül Element"
            ? "Sembol: " + data.formula
            : "Formül: " + data.formula;
        passportImage.sprite = data.image;
    }

    void OnAnswer(string city)
    {
        ChemicalData current = currentList[currentIndex];
        bool isElement = current.type == "Element" || current.type == "Molekül Element";

        bool correct = (isElement && city == "Element") || (!isElement && city == "Bileşik");

        if (correct)
        {
            resultText.text = "Doğru!";
            resultText.color = Color.green;
            StartCoroutine(NextPassport());
        }
        else
        {
            resultText.text = "Yanlış!";
            resultText.color = Color.red;
            lives--;
            UpdateHearts();
        }

        
    }

    IEnumerator NextPassport()
    {
        yield return new WaitForSeconds(1f);
        currentIndex++;
        resultText.text = "";

        if (lives <= 0)
        {
            EndGame();
        }
        else
        {
            ShowPassport();
        }
    }

    void EndGame()
    {
        elementButton.SetActive(false);
        bilesikButton.SetActive(false);
        startButton.SetActive(true);
        leftPassport.SetActive(false);
        rightPassport.SetActive(false);

        resultText.text = (lives > 0) ? "Oyun Bitti! Tebrikler!" : "Tüm canlar bitti!";
        resultText.color = (lives > 0) ? Color.yellow : Color.red;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].color = (i < lives) ? Color.red : Color.gray;
        }
    }
}

[System.Serializable]
public class ChemicalData
{
    public string name;
    public string formula;
    public string type; // "Element", "Molekül Element", "Bileşik"
    public Sprite image;

    public ChemicalData(string n, string f, string t, Sprite i)
    {
        name = n;
        formula = f;
        type = t;
        image = i;
    }
}
