using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class AtomGame : MonoBehaviour
{
    [System.Serializable]
    public class Element
    {
        public string name;
        public int protonCount;

        public Element(string name, int protonCount)
        {
            this.name = name;
            this.protonCount = protonCount;
        }
    }
    private int totalElectronValue = 0;
    private Element currentElement;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI resultText;
    public DropZone protonDropZone;
    public Rotation rotation;

    public GameObject dvalu;
    public GameObject svalu;
    public GameObject pvalu;

    public Transform regularProtonArea;
    public Transform fiveProtonArea;
    public Transform tenProtonArea;

    public GameObject protonPhaseUI;
    public GameObject electronPhaseUI;

    public GameObject dropZone;
    public GameObject orbitalPrefab;
    public OrbitalZone oneSZone;
    public OrbitalZone twoSZone;
    public OrbitalZone twoPZone;
    public OrbitalZone threeSZone;
    public OrbitalZone threePZone;
    public OrbitalZone fourSZone;
    public OrbitalZone threeDZone;
    public OrbitalZone fourPZone;
    public GameObject s1;
    public GameObject s2;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;
    public GameObject d1;
    public GameObject d2;
    public GameObject d3;
    public GameObject d4;
    public GameObject d5;
    public GameObject d6;
    public GameObject d7;
    public GameObject d8;
    public GameObject d9;
    public GameObject d10;

    private List<Element> allElements = new List<Element>()
    {
        new Element("Hidrojen", 1),
        new Element("Helyum", 2),
        new Element("Lityum", 3),
        new Element("Berilyum", 4),
        new Element("Bor", 5),
        new Element("Karbon", 6),
        new Element("Azot", 7),
        new Element("Oksijen", 8),
        new Element("Flor", 9),
        new Element("Neon", 10),
        new Element("Sodyum", 11),
        new Element("Magnezyum", 12),
        new Element("Alüminyum", 13),
        new Element("Silikon", 14),
        new Element("Fosfor", 15),
        new Element("Kükürt", 16),
        new Element("Klor", 17),
        new Element("Argon", 18),
        new Element("Potasyum", 19),
        new Element("Kalsiyum", 20),
        new Element("Skandiyum", 21),
        new Element("Titanyum", 22),
        new Element("Vanadyum", 23),
        new Element("Krom", 24),
        new Element("Mangan", 25),
        new Element("Demir", 26),
        new Element("Kobalt", 27),
        new Element("Nikel", 28),
        new Element("Bakır", 29),
        new Element("Çinko", 30),
        new Element("Galyum", 31),
        new Element("Germanyum", 32),
        new Element("Arsenik", 33),
        new Element("Selenyum", 34),
        new Element("Brom", 35),
        new Element("Kripton", 36),
    };

    public List<Element> selectedElements;
    public int currentQuestionIndex = 0;
    private int lives = 3;
    public GameObject electronvalue1;
    public GameObject electronvalue2;
    public GameObject electronvalue3;
    public GameObject electronvalue4;
    public GameObject electronvalue5;
    public GameObject electronvalue6;
    public GameObject electronvalue7;
    public GameObject electronvalue8;
    public TextMeshProUGUI protontext1;
    public TextMeshProUGUI protontext2;
    public TextMeshProUGUI protontext3;
    public TextMeshProUGUI electrontext1;
    public TextMeshProUGUI electrontext2;
    public TextMeshProUGUI electrontext3;
    public GameObject button;
    public GameObject orbitelec;
    public GameObject rotattyext;
    public GameObject Cekirdek;

    void Start()
    {
        selectedElements = allElements.OrderBy(e => Random.value).Take(10).ToList();
        livesText.text = "Can: " + lives;
        rotattyext.SetActive(false);
        DisplayNextQuestion();
        ResetProtonsToStarter();
        ResetElectrons();
        svalu.SetActive(false);
        pvalu.SetActive(false);
        dvalu.SetActive(false);
        s1.SetActive(false);
        s2.SetActive(false);
        p1.SetActive(false);
        p2.SetActive(false);
        p3.SetActive(false);
        p4.SetActive(false);
        p5.SetActive(false);
        p6.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(false);
        d3.SetActive(false);
        d4.SetActive(false);
        d5.SetActive(false);
        d6.SetActive(false);
        d7.SetActive(false);
        d8.SetActive(false);
        d9.SetActive(false);
        d10.SetActive(false);
        Cekirdek.SetActive(true);
        orbitelec.SetActive(false);
        protonPhaseUI.SetActive(true);
        electronPhaseUI.SetActive(false);
        orbitalPrefab.SetActive(false);
        dropZone.SetActive(true);
        electronvalue1.SetActive(false);
        electronvalue2.SetActive(false);
        electronvalue3.SetActive(false);
        electronvalue4.SetActive(false);
        electronvalue5.SetActive(false);
        electronvalue6.SetActive(false);
        electronvalue7.SetActive(false);
        electronvalue8.SetActive(false);
        protontext1.text = "Proton: x1";
        protontext2.text = "Proton: x5";
        protontext3.text = "Proton: x10";
        electrontext1.text = "";
        electrontext2.text = "";
        electrontext3.text = "";
        button.SetActive(true);
    }
    void DisplayNextQuestion()
    {
        if (currentQuestionIndex < selectedElements.Count)
        {
            questionText.text = selectedElements[currentQuestionIndex].name;
            resultText.text = "";
        }
        else
        {
            SwitchToElectronPhase();
        }
        ResetProtonsToStarter();
    }
    public void CheckProtonAnswer()
    {
        if (currentQuestionIndex >= selectedElements.Count)
            return;

        int correctCount = selectedElements[currentQuestionIndex].protonCount;
        int placedCount = protonDropZone.GetProtonCount();

        if (placedCount == correctCount)
        {
            resultText.text = "Doğru!";
            currentQuestionIndex++;
            DisplayNextQuestion();
        }
        else
        {
            lives--;
            if (lives <= 0)
            {
                livesText.text = "Can: 0";
                resultText.text = "Yanlış! Kaybettiniz!";
                questionText.text = "Oyun Bitti";
                protonPhaseUI.SetActive(false);
            }
            else
            {
                resultText.text = "Yanlış! Tekrar deneyin.";
                livesText.text = "Can: " + lives;
                ResetProtonsToStarter();
            }
        }
    }
    void ResetProtonsToStarter()
    {
        GameObject[] protons = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject proton in protons)
        {
            Draggable draggable = proton.GetComponent<Draggable>();
            if (draggable != null)
            {
                if (draggable.protonValue == 1)
                {
                    proton.transform.position = regularProtonArea.position;
                }
                else if (draggable.protonValue == 5)
                {
                    proton.transform.position = fiveProtonArea.position;
                }
                else if (draggable.protonValue == 10)
                {
                    proton.transform.position = tenProtonArea.position;
                }
            }
        }
    }
    void SwitchToElectronPhase()
    {
        Cekirdek.SetActive(false);
        protonPhaseUI.SetActive(false);
        electronPhaseUI.SetActive(true);
        orbitalPrefab.SetActive(true);
        dropZone.SetActive(false);
        electronvalue1.SetActive(true);
        electronvalue2.SetActive(true);
        electronvalue3.SetActive(true);
        electronvalue4.SetActive(true);
        electronvalue5.SetActive(true);
        electronvalue6.SetActive(true);
        electronvalue7.SetActive(true);
        electronvalue8.SetActive(true);
        protontext1.text = "";
        protontext2.text = "";
        protontext3.text = "";
        electrontext1.text = "Elektron: x1";
        electrontext2.text = "Elektron: x2";
        electrontext3.text = "Elektron: x4";
        button.SetActive(false);

        currentQuestionIndex = 0;
        DisplayElectronQuestion();
    }

    void DisplayElectronQuestion()
    {
        if (currentQuestionIndex < selectedElements.Count)
        {
            currentElement = selectedElements[currentQuestionIndex];
            questionText.text = currentElement.name;
            ResetElectrons();
        }
        else
        {
            sOrbitalPhase();
        }
    }
    void ResetElectrons()
    {
        totalElectronValue = 0;
        oneSZone.Clear();
        twoSZone.Clear();
        twoPZone.Clear();
        threeSZone.Clear();
        threePZone.Clear();
        fourSZone.Clear();
        threeDZone.Clear();
        fourPZone.Clear();

        resultText.text = "";

        GameObject[] protons = GameObject.FindGameObjectsWithTag("Draggable");
        foreach (GameObject electron in protons)
        {
            Draggable draggable = electron.GetComponent<Draggable>();
            if (draggable != null)
            {
                if (draggable.electronValue == 1)
                {
                    electron.transform.position = regularProtonArea.position;
                }
                else if (draggable.electronValue == 2)
                {
                    electron.transform.position = fiveProtonArea.position;
                }
                else if (draggable.electronValue == 4)
                {
                    electron.transform.position = tenProtonArea.position;
                }
            }
        }

    }
    public void LoseLifeAndResetElectron(GameObject electron)
    {
        lives--;
        livesText.text = "Can: " + lives;

        if (lives <= 0)
        {
            questionText.text = "Oyun Bitti";
            resultText.text = "Yanlış! Oyunu Kaybettin!";
            electronPhaseUI.SetActive(false);
            return;
        }

        resultText.text = "Yanlış! Tekrar deneyin.";

        Invoke(nameof(ResetElectrons), 1f);
    }
    public void OnElectronPlaced(GameObject electron)
    {
        Draggable draggable = electron.GetComponent<Draggable>();
        if (draggable != null)
        {
            totalElectronValue += draggable.electronValue;
        }

        int correctCount = selectedElements[currentQuestionIndex].protonCount;

        if (totalElectronValue == correctCount)
        {
            currentQuestionIndex++;
            resultText.text = "Doğru!";
            Invoke(nameof(DisplayElectronQuestion), 1f);
        }
        else if (totalElectronValue > correctCount)
        {
            LoseLifeAndResetElectron(electron);
        }
    }
    public void sOrbitalPhase()
    {
        electrontext1.text = "elektronlar";
        electrontext2.text = "";
        electrontext3.text = "";
        rotattyext.SetActive(true);
        svalu.SetActive(true);
        electronPhaseUI.SetActive(false);
        orbitelec.SetActive(true);
        s1.SetActive(true);
        s2.SetActive(true);
        ResetElectrons();
        orbitalPrefab.SetActive(false);
        electronvalue1.SetActive(false);
        electronvalue2.SetActive(false);
        electronvalue3.SetActive(false);
        electronvalue4.SetActive(false);
        electronvalue5.SetActive(false);
        electronvalue6.SetActive(false);
        electronvalue7.SetActive(false);
        electronvalue8.SetActive(false);
        questionText.text = "s orbitalini oluşturun";
    }
    public void pOrbitalPhase()
    {
        pvalu.SetActive(true);
        svalu.SetActive(false);
        ResetElectrons();
        s1.SetActive(false);
        s2.SetActive(false);
        p1.SetActive(true);
        p2.SetActive(true);
        p3.SetActive(true);
        p4.SetActive(true);
        p5.SetActive(true);
        p6.SetActive(true);
        questionText.text = "p orbitalini oluşturun";
    }
    public void dOrbitalPhase()
    {
        dvalu.SetActive(true);
        pvalu.SetActive(false);
        ResetElectrons();
        p1.SetActive(false);
        p2.SetActive(false);
        p3.SetActive(false);
        p4.SetActive(false);
        p5.SetActive(false);
        p6.SetActive(false);
        questionText.text = "d orbitalini oluşturun";
        d1.SetActive(true);
        d2.SetActive(true);
        d3.SetActive(true);
        d4.SetActive(true);
        d5.SetActive(true);
        d6.SetActive(true);
        d7.SetActive(true);
        d8.SetActive(true);
        d9.SetActive(true);
        d10.SetActive(true); 
    }
}