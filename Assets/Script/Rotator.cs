using System.Collections.Generic;
using UnityEngine;
using TMPro; // penting untuk TMP_Text

public class Rotator : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject targetObject;
    public float rotationSpeed = 50f;

    private float currentDirection = 0f;
    private Quaternion initialRotation;

    [Header("Hint Settings")]
    public Material hintMaterial;

    // TMP_Text untuk nama molekul yang sedang ditampilkan
    public TMP_Text questionText;       

    // daftar hint fleksibel per molekul
    public List<MoleculeHint> moleculeHints = new List<MoleculeHint>();

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();

    void Start()
    {
        if (targetObject != null)
        {
            initialRotation = targetObject.transform.rotation;
        }
    }

    void Update()
    {
        if (targetObject != null && currentDirection != 0f)
        {
            targetObject.transform.Rotate(Vector3.up, currentDirection * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    public void RotateLeft() => currentDirection = 1f;
    public void RotateRight() => currentDirection = -1f;
    public void StopRotation() => currentDirection = 0f;

    public void ResetRotation()
    {
        if (targetObject != null)
        {
            targetObject.transform.rotation = Quaternion.Euler(0f, 359.033295f, 0f);
            currentDirection = 0f;
        }
    }

    public void ShowHint()
    {
        ClearHint(); // bersihkan hint sebelumnya

        string currentQuestion = questionText.text.Trim(); // buang spasi
        MoleculeHint hintData = moleculeHints.Find(m => 
            m.moleculeName.Equals(currentQuestion, System.StringComparison.OrdinalIgnoreCase)
        );

        if (hintData == null)
        {
            Debug.LogWarning("No hint found for molecule: " + currentQuestion);
            return;
        }

        foreach (GameObject slot in hintData.hintSlots)
        {
            if (slot != null)
            {
                // cari renderer di slot atau anak-anaknya
                Renderer rend = slot.GetComponent<Renderer>();
                if (rend == null) rend = slot.GetComponentInChildren<Renderer>();

                if (rend != null)
                {
                    if (!originalMaterials.ContainsKey(slot))
                    {
                        originalMaterials[slot] = rend.material;
                    }
                    rend.material = hintMaterial;
                }
                else
                {
                    Debug.LogWarning("No Renderer found in slot: " + slot.name);
                }
            }
        }
    }



    public void ClearHint()
    {
        foreach (var kv in originalMaterials)
        {
            if (kv.Key != null)
            {
                Renderer rend = kv.Key.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material = kv.Value;
                }
            }
        }
        originalMaterials.Clear();
    }
}
