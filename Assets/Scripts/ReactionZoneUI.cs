using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ReactionZoneUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI chargeText;

    public List<AtomInstance> atomsInZone = new List<AtomInstance>();

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

    // 🔘 BUTONA BAĞLANACAK
    public void Calculate()
    {
        if (atomsInZone.Count == 0)
        {
            nameText.text = "-";
            chargeText.text = "0";
            return;
        }

        // 🚫 NÖTR ATOM KONTROLÜ
        foreach (var atom in atomsInZone)
        {
            if (atom.charge == 0)
            {
                nameText.text = "Nötr atom bileşik yapamaz!";
                chargeText.text = "0";
                return;
            }
        }

        int totalCharge = 0;
        foreach (var atom in atomsInZone)
            totalCharge += atom.charge;

        string formula = BuildFormula(atomsInZone, totalCharge);

        nameText.text = formula;
        chargeText.text = totalCharge.ToString();
    }


    string BuildFormula(List<AtomInstance> atoms, int totalCharge)
    {

        List<ElementInfo> elements = new List<ElementInfo>();
        foreach (var atom in atoms)
            elements.Add(atom.elementInfo);

        // 🔴 Metal + Metal
        if (elements.All(e => e.category == ElementCategory.Metal))
            return "Metal Metal olmaz!";

        // 🟢 OH⁻ özel durumu
        bool hasH = elements.Any(e => e.symbol == "H");
        bool hasO = elements.Any(e => e.symbol == "O");

        if (elements.Count == 2 && hasH && hasO && totalCharge == -1)
            return "OH";

        // 🔹 SIRALAMA
        List<ElementInfo> ordered = new List<ElementInfo>();

        // 1️⃣ Metal varsa önce
        ordered.AddRange(elements.Where(e => e.category == ElementCategory.Metal));

        // 2️⃣ Metal olmayanlar
        var nonMetals = elements.Where(e => e.category != ElementCategory.Metal).ToList();

        // 3️⃣ H varsa öne al
        var hydrogen = nonMetals.FirstOrDefault(e => e.symbol == "H");
        if (hydrogen != null)
        {
            ordered.Add(hydrogen);
            nonMetals.Remove(hydrogen);
        }

        // 4️⃣ Kalanlar
        ordered.AddRange(nonMetals);

        // 🔢 SAYMA
        Dictionary<string, int> counts = new Dictionary<string, int>();
        foreach (var e in ordered)
        {
            if (!counts.ContainsKey(e.symbol))
                counts[e.symbol] = 0;

            counts[e.symbol]++;
        }

        // ✍️ FORMÜL YAZ
        string formula = "";
        HashSet<string> written = new HashSet<string>();

        foreach (var e in ordered)
        {
            if (written.Contains(e.symbol)) continue;

            formula += e.symbol;
            if (counts[e.symbol] > 1)
                formula += counts[e.symbol];

            written.Add(e.symbol);
        }

        return formula;
    }
}
