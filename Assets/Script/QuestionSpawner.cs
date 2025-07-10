using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionSpawner : MonoBehaviour
{
    public GameObject questionPrefab;
    private Vector3 spawnPosition = new Vector3(-0.7f, 1.302f, 7.196f);
    private GameObject currentQuestion;

    [System.Serializable]
    public class QuestionData
    {
        public string questionText;
        public string[] requiredAtoms;
        public string difficulty;
    }

    public List<QuestionData> questions = new List<QuestionData>();

    void Start()
    {
        // Tambahkan pertanyaan-pertanyaan di sini
        questions.Add(new QuestionData
        {
            questionText = "Buat air (H2O) dari 2 Atom H dan 1 Atom O.",
            requiredAtoms = new string[] { "HAtom", "HAtom", "OAtom" },
            difficulty = "Easy"
        });

        questions.Add(new QuestionData
        {
            questionText = "Buat karbon dioksida (CO2) dari 1 Atom C dan 2 Atom O.",
            requiredAtoms = new string[] { "CAtom", "OAtom", "OAtom" },
            difficulty = "Medium"
        });

        questions.Add(new QuestionData
        {
            questionText = "Buat HCl dari 1 Atom H dan 1 Atom Cl.",
            requiredAtoms = new string[] { "HAtom", "ClAtom" },
            difficulty = "Easy"
        });

        questions.Add(new QuestionData
        {
            questionText = "Buat BF3 dari 1 Atom B dan 3 Atom F.",
            requiredAtoms = new string[] { "BAtom", "FAtom", "FAtom", "FAtom" },
            difficulty = "Hard"
        });

        questions.Add(new QuestionData
        {
            questionText = "Buat KBr dari 1 Atom K dan 1 Atom Br.",
            requiredAtoms = new string[] { "KAtom", "BrAtom" },
            difficulty = "Medium"
        });
    }

    public void SpawnRandomQuestion()
    {
        // Hapus pertanyaan sebelumnya
        if (currentQuestion != null)
        {
            Destroy(currentQuestion);
        }

        // Pilih pertanyaan acak
        int index = Random.Range(0, questions.Count);
        QuestionData q = questions[index];

        // Buat prefab pertanyaan
        currentQuestion = Instantiate(questionPrefab, spawnPosition, Quaternion.identity);
        currentQuestion.tag = q.difficulty;

        // Ubah warna berdasarkan tingkat kesulitan
        string hex = GetHexColorFromTag(q.difficulty);
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            Renderer r = currentQuestion.GetComponent<Renderer>();
            if (r != null) r.material.color = color;
        }

        // Tampilkan teks pertanyaan
        TMP_Text tmpText = currentQuestion.GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = q.questionText;
        }

    }

    private string GetHexColorFromTag(string tag)
    {
        switch (tag)
        {
            case "Easy": return "#7CFC00";    // hijau muda
            case "Medium": return "#FFD700";  // emas
            case "Hard": return "#FF4500";    // merah-oranye
            default: return "#D3D3D3";        // abu muda
        }
    }

    public AtomConnector atomConnector; // Drag dari Inspector

    public void CheckAnswer()
    {
        if (currentQuestion == null)
        {
            Debug.LogWarning("Tidak ada pertanyaan yang sedang aktif.");
            return;
        }

        TMP_Text tmpText = currentQuestion.GetComponentInChildren<TMPro.TMP_Text>();
        string currentText = tmpText != null ? tmpText.text : "";

        string currentTag = currentQuestion.tag;
        QuestionData currentData = questions.Find(q =>
            q.difficulty == currentTag && q.questionText == currentText
        );

        if (currentData == null)
        {
            Debug.LogWarning("Pertanyaan tidak ditemukan.");
            return;
        }

        // Ambil tag atom dari semua slot
        List<string> userAtoms = new List<string>();
        foreach (AtomSlotTrigger slot in atomConnector.atomSlots)
        {
            if (slot.currentAtom != null)
            {
                userAtoms.Add(slot.currentAtom.tag);
            }
        }

        // Cek apakah jumlah atom sesuai
        if (userAtoms.Count != currentData.requiredAtoms.Length)
        {
            ShowNotificationOnCube("Jawaban SALAH", false);
            return;
        }

        // Salin list untuk perbandingan
        List<string> requiredAtoms = new List<string>(currentData.requiredAtoms);
        foreach (string atomTag in userAtoms)
        {
            if (requiredAtoms.Contains(atomTag))
            {
                requiredAtoms.Remove(atomTag);
            }
            else
            {
                ShowNotificationOnCube("Jawaban SALAH", false);
                return;
            }
        }

        // Jika semua cocok
        if (requiredAtoms.Count == 0)
        {
            ShowNotificationOnCube("Jawaban BENAR", true);
            Invoke(nameof(SpawnRandomQuestion), 2f); // Ganti soal setelah 2 detik
        }
        else
        {
            ShowNotificationOnCube("Jawaban SALAH", false);
        }
    }

    private void ShowNotificationOnCube(string message, bool correct)
    {
        TMP_Text tmpText = currentQuestion.GetComponentInChildren<TMPro.TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = message;
            tmpText.color = correct ? Color.green : Color.red;
        }
    }


}
