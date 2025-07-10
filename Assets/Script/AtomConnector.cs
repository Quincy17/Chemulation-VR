using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomConnector : MonoBehaviour
{
    public AtomSlotTrigger[] atomSlots;
    public Material lineMaterial;
    private List<LineRenderer> activeLines = new List<LineRenderer>();

    void Update()
    {
        UpdateLines();
    }

    void UpdateLines()
    {
        foreach (LineRenderer line in activeLines)
        {
            Destroy(line.gameObject);
        }
        activeLines.Clear();

        foreach (AtomSlotTrigger slot in atomSlots)
        {
            if (slot.currentAtom != null)
            {
                GameObject lineObj = new GameObject("LineToAtom");
                LineRenderer lr = lineObj.AddComponent<LineRenderer>();

                lr.material = lineMaterial;
                lr.startWidth = 0.02f;
                lr.endWidth = 0.02f;
                lr.positionCount = 2;
                lr.useWorldSpace = true;
                lr.SetPosition(0, transform.position);            // Titik awal = posisi Inti Atom
                lr.SetPosition(1, slot.transform.position);       // Titik akhir = posisi slot

                activeLines.Add(lr);
            }
        }
    }
}
