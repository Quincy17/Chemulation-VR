using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestionSpawner : MonoBehaviour
{
    public TMP_Text moleculeNameText; // text untuk pertanyaan

    private List<string> molecules;

    void Start()
    {
        // daftar molekul yang bisa muncul
        molecules = new List<string>
        {
            "H2O",
            "CO2",
            "SO2",
            "XeF2",
            "BF3",
            "ClF3",
            "NH3",
            "CH4",
            "SF4",
            "XeF",
            "BrF5",
            "PCl5",
            "SF6"
        };
    }

    // fungsi ini dipanggil saat button ditekan
    public void GenerateQuestion()
    {
        string chosen = molecules[Random.Range(0, molecules.Count)];
        moleculeNameText.text = chosen;
    }
}
