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
    public TMP_Text checkLocationText;
    public TMP_Text statusText;

    // Aturan jumlah atom untuk tiap molekul
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

    // Aturan lokasi atom untuk tiap molekul (opsional, bisa kosong)
    private Dictionary<string, List<List<string>>> moleculeLocations = new Dictionary<string, List<List<string>>>()
    {
        // Cek lokasi H2O
        { "H2O", new List<List<string>>
            {
                new List<string> { "Atom 12", "Atom 10" },
                new List<string> { "Atom 15", "Atom 17" }
            }
        }
    };

    void Update()
    {
        CheckAnswer();
    }

    public void CheckAnswer()
    {
        string targetMolecule = questionText.text;
        if (!moleculeRules.ContainsKey(targetMolecule))
        {
            Debug.LogWarning("No rule found for molecule: " + targetMolecule);
            return;
        }

        // hitung atom yang ada di slot
        Dictionary<string, int> userAtoms = new Dictionary<string, int>();
        Dictionary<string, List<string>> atomLocations = new Dictionary<string, List<string>>();

        foreach (var slot in allSlots)
        {
            if (slot.currentAtom != null)
            {
                string tag = slot.currentAtom.tag;
                if (!userAtoms.ContainsKey(tag)) userAtoms[tag] = 0;
                userAtoms[tag]++;

                // simpan lokasi atom
                if (!atomLocations.ContainsKey(tag))
                    atomLocations[tag] = new List<string>();
                atomLocations[tag].Add(slot.tag);
            }
        }

        // Ambil aturan jumlah atom
        var rule = moleculeRules[targetMolecule];
        bool atomCorrect = true;

        foreach (var kv in rule)
        {
            if (!userAtoms.ContainsKey(kv.Key) || userAtoms[kv.Key] != kv.Value)
            {
                atomCorrect = false;
                break;
            }
        }

        // cek kalau user punya atom asing
        foreach (var kv in userAtoms)
        {
            if (!rule.ContainsKey(kv.Key))
            {
                atomCorrect = false;
                break;
            }
        }

        // Cek lokasi atom
        bool locationCorrect = true;
        if (moleculeLocations.ContainsKey(targetMolecule))
        {
            locationCorrect = false; // default salah, sampai terbukti benar
            var validLocationSets = moleculeLocations[targetMolecule];

            foreach (var validSet in validLocationSets)
            {
                // bandingkan dengan lokasi user untuk HAtom
                if (atomLocations.ContainsKey("HAtom"))
                {
                    var userSet = new HashSet<string>(atomLocations["HAtom"]);
                    var targetSet = new HashSet<string>(validSet);

                    if (userSet.SetEquals(targetSet))
                    {
                        locationCorrect = true;
                        break;
                    }
                }
            }
        }

        // Update UI
        checkAtomText.text = atomCorrect ? "Correct" : "Incorrect";
        checkAtomText.color = atomCorrect ? Color.green : Color.red;

        checkLocationText.text = locationCorrect ? "Correct" : "Incorrect";
        checkLocationText.color = locationCorrect ? Color.green : Color.red;

        if (atomCorrect && locationCorrect)
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
