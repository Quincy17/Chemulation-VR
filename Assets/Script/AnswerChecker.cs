using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerChecker : MonoBehaviour
{
    [Header("Slot")]
    public AtomSlotTrigger[] allSlots;

    [Header("UI")]
    public TMP_Text questionText;
    public TMP_Text checkAtomText;
    public TMP_Text statusText;

    // definisi aturan jumlah atom untuk tiap molekul
    private Dictionary<string, Dictionary<string, int>> moleculeRules = new Dictionary<string, Dictionary<string, int>>()
    {
        { "H2O", new Dictionary<string, int> { { "HAtom", 2 } } },
        { "CO2", new Dictionary<string, int> { { "OAtom", 2 } } },
        { "SO2", new Dictionary<string, int> { { "OAtom", 2 } } },
        { "XeF2", new Dictionary<string, int> { { "FAtom", 2 } } },
        { "BF3", new Dictionary<string, int> { { "FAtom", 3 } } },
        { "ClF3", new Dictionary<string, int> { { "FAtom", 3 } } },
        { "NH3", new Dictionary<string, int> { { "HAtom", 3 } } },
        { "CH4", new Dictionary<string, int> { { "HAtom", 4 } } },
        { "SF4", new Dictionary<string, int> { { "FAtom", 4 } } },
        { "XeF", new Dictionary<string, int> { { "FAtom", 1 } } },
        { "BrF5", new Dictionary<string, int> { { "FAtom", 5 } } },
        { "PCl5", new Dictionary<string, int> { { "ClAtom", 5 } } },
        { "SF6", new Dictionary<string, int> { { "FAtom", 6 } } },
    };

    void Update()
    {
        // cek jawaban setiap frame
        CheckAnswer();
    }

    public void CheckAnswer()
    {
        string targetMolecule = questionText.text; // ambil nama molekul dari text
        if (!moleculeRules.ContainsKey(targetMolecule))
        {
            Debug.LogWarning("No rule found for molecule: " + targetMolecule);
            return;
        }

        // hitung atom yang ada di slot
        Dictionary<string, int> userAtoms = new Dictionary<string, int>();
        foreach (var slot in allSlots)
        {
            if (slot.currentAtom != null)
            {
                string tag = slot.currentAtom.tag;
                if (!userAtoms.ContainsKey(tag)) userAtoms[tag] = 0;
                userAtoms[tag]++;
            }
        }

        // ambil aturan
        var rule = moleculeRules[targetMolecule];

        // cek apakah atom sesuai
        bool atomCorrect = true;
        foreach (var kv in rule)
        {
            if (!userAtoms.ContainsKey(kv.Key) || userAtoms[kv.Key] != kv.Value)
            {
                atomCorrect = false;
                break;
            }
        }

        // cek kalau user punya atom asing (tidak ada di aturan)
        foreach (var kv in userAtoms)
        {
            if (!rule.ContainsKey(kv.Key))
            {
                atomCorrect = false;
                break;
            }
        }

        // update UI
        checkAtomText.text = atomCorrect ? "Correct" : "Incorrect";
        checkAtomText.color = atomCorrect ? Color.green : Color.red;

        if (atomCorrect)
        {
            statusText.text = "Passed";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "On Going";
            statusText.color = new Color(0.96f, 0.64f, 0.03f); // orange
        }
    }
}
