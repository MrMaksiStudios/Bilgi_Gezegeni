using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AtomInstance : MonoBehaviour
{
    public ElementInfo elementInfo;   // 👈 BU ÇOK KRİTİK
    public int charge;

    public Image atomSprite;
    public TextMeshProUGUI atomText;

    public void Setup(ElementInfo info, int charge)
    {
        elementInfo = info;
        this.charge = charge;

        atomSprite.sprite = info.sprite;
        atomText.text = FormatText(info.symbol, charge);
    }

    string FormatText(string symbol, int charge)
    {
        if (charge == 0) return symbol;
        if (charge > 0) return symbol + charge + "+";
        return symbol + Mathf.Abs(charge) + "-";
    }
}
