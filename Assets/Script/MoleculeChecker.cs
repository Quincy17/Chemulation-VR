using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoleculeChecker : MonoBehaviour
{
    public AtomSlotTrigger[] allSlots;
    public TMP_Text checkAtomText;
    public TMP_Text checkLocationText;
    public TMP_Text statusText;

    void Update()
    {
        CheckAnswer();
    }

    void CheckAnswer()
    {
        int hydrogenCount = 0;
        bool hIn12 = false;
        bool hIn10 = false;
        bool hIn15 = false;  
        bool hIn17 = false; 
        bool anyHInWrongSlot = false;
        bool hasInvalidAtom = false;

        foreach (var slot in allSlots)
        {
            GameObject atom = slot.currentAtom;

            if (atom != null)
            {
                if (atom.CompareTag("HAtom"))
                {
                    hydrogenCount++;

                    if (slot.CompareTag("Atom12"))
                        hIn12 = true;
                    else if (slot.CompareTag("Atom10"))
                        hIn10 = true;
                    else if (slot.CompareTag("Atom15"))  // Cek Atom10
                        hIn15 = true;
                    else if (slot.CompareTag("Atom17"))  // Cek Atom22
                        hIn17 = true;
                    else
                        anyHInWrongSlot = true;
                }
                else
                {
                    hasInvalidAtom = true;
                }
            }
        }

        
        bool locationCorrect = ((hIn12 && hIn10) || (hIn15 && hIn17)) && !anyHInWrongSlot;
        bool atomCorrect = (hydrogenCount == 2) && !hasInvalidAtom;

       
        checkLocationText.text = locationCorrect ? "Correct" : "Incorrect";
        checkLocationText.color = locationCorrect ? Color.green : Color.red;

    
        checkAtomText.text = atomCorrect ? "Correct" : "Incorrect";
        checkAtomText.color = atomCorrect ? Color.green : Color.red;

        if (locationCorrect && atomCorrect)
        {
            statusText.text = "Passed";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "On Going";
            statusText.color = new Color(0.9607844f, 0.6431373f, 0.03137255f); // Orange
        }
    }
}