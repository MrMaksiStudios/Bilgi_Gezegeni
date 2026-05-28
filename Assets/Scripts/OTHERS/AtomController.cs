using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AtomController : MonoBehaviour
{
    public GameObject atomPrefab;
    public Transform atomParent;

    [Header("Element Database")]
    public List<ElementInfo> elementDatabase;

    [Header("UI")]
    public Slider protonSlider;
    public Slider electronSlider;
    public TextMeshProUGUI protonValueText;
    public TextMeshProUGUI electronValueText;

    [Header("Preview")]
    public Image atomSprite;
    public TextMeshProUGUI atomText;

    [Header("Sprites (1–18 sırayla)")]
    public Sprite[] elementSprites;

    string[] elementNames =
    {
        "H","He","Li","Be","B","C","N","O","F","Ne",
        "Na","Mg","Al","Si","P","S","Cl","Ar"
    };

    void Start()
    {
        UpdateAtom();
    }

    public void UpdateAtom()
    {
        int protons = (int)protonSlider.value;
        int electrons = (int)electronSlider.value;

        protonValueText.text = protons.ToString();
        electronValueText.text = electrons.ToString();

        CreateAtom(protons, electrons);
    }

    void CreateAtom(int protons, int electrons)
    {
        if (protons < 1 || protons > 18) return;

        string element = elementNames[protons - 1];
        int charge = protons - electrons;

        // Sprite
        atomSprite.sprite = elementSprites[protons - 1];

        // Text
        atomText.text = FormatAtomText(element, charge);
    }

    public void CreateAtomButton()
    {
        int p = (int)protonSlider.value;
        int e = (int)electronSlider.value;

        ElementInfo info = elementDatabase
            .Find(el => el.protons == p);

        if (info == null) return;

        int charge = p - e;

        GameObject atomObj = Instantiate(atomPrefab, atomParent);
        AtomInstance atom = atomObj.GetComponent<AtomInstance>();
        atom.Setup(info, charge);
    }



    string FormatAtomText(string element, int charge)
    {
        if (charge == 0)
            return element;

        if (charge > 0)
            return element + charge + "+";

        return element + Mathf.Abs(charge) + "-";
    }
}
