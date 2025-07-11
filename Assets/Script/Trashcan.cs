using Oculus.Interaction.HandGrab;
using Oculus.Interaction;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsAtom(other.gameObject)) return;

        DisableInteraction(other.gameObject);

        StartCoroutine(DestroyAfterFrame(other.gameObject));
    }

    private bool IsAtom(GameObject obj)
    {
        string[] validTags = {
            "SAtom", "XeAtom", "NAtom", "OAtom", "KAtom", "ClAtom",
            "FAtom", "HAtom", "BAtom", "BrAtom", "CAtom"
        };

        foreach (var tag in validTags)
        {
            if (obj.CompareTag(tag)) return true;
        }
        return false;
    }

    private void DisableInteraction(GameObject obj)
    {
        var grabbable = obj.GetComponent<Grabbable>();
        if (grabbable != null) grabbable.enabled = false;

        var handGrab = obj.GetComponent<HandGrabInteractable>();
        if (handGrab != null) handGrab.enabled = false;

        var grabInteractable = obj.GetComponent<GrabInteractable>();
        if (grabInteractable != null) grabInteractable.enabled = false;

        var handGrabChild = obj.transform.Find("ISDK_HandGrabInteraction");
        if (handGrabChild != null) handGrabChild.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator DestroyAfterFrame(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        Destroy(obj);
    }
}