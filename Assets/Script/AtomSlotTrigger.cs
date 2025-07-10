using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomSlotTrigger : MonoBehaviour
{
    public GameObject currentAtom;
    private List<string> validAtomTags = new List<string> {
        "SAtom", "XeAtom", "NAtom", "OAtom", "KAtom",
        "ClAtom", "FAtom", "HAtom", "BAtom", "BrAtom", "CAtom"
    };

    public float detectionRadius = 0.03f;

    void Update()
    {
        currentAtom = null;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (validAtomTags.Contains(hit.tag))
            {
                currentAtom = hit.gameObject;
                break;
            }
        }
    }
}