using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ReactionZoneController : MonoBehaviour
{
    public TextMeshProUGUI formulaText;
    public TextMeshProUGUI chargeText;

    private List<AtomInstance> atomsInZone = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collider Entered Reaction Zone");
        AtomInstance atom = other.GetComponent<AtomInstance>();
        if (atom != null)
        {
            AddAtom(atom);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AtomInstance atom = other.GetComponent<AtomInstance>();
        if (atom != null)
        {
            RemoveAtom(atom);
        }
    }

    // BUNU MANUEL / UI / TRIGGER / DROP ile çağırabilirsin
    public void AddAtom(AtomInstance atom)
    {
        if (!atomsInZone.Contains(atom))
            atomsInZone.Add(atom);
    }

    public void RemoveAtom(AtomInstance atom)
    {
        if (atomsInZone.Contains(atom))
            atomsInZone.Remove(atom);
    }

    // 🔘 BUTONA BAĞLANAN FONKSİYON
    public void AnalyzeZone()
    {
        if (atomsInZone.Count == 0)
        {
            formulaText.text = "-";
            chargeText.text = "0";
            return;
        }

        Dictionary<string, int> counts = new();
        int totalCharge = 0;

        foreach (var atom in atomsInZone)
        {
            string symbol = atom.elementInfo.symbol;

            if (!counts.ContainsKey(symbol))
                counts[symbol] = 0;

            counts[symbol]++;
            totalCharge += atom.charge;   // 👈 ÖNEMLİ
        }

        string formula = "";
        foreach (var kvp in counts)
        {
            formula += kvp.Key;
            if (kvp.Value > 1)
                formula += kvp.Value;
        }

        formulaText.text = formula;
        chargeText.text = totalCharge.ToString();
    }
}
