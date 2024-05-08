using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody[] rigidbodies;

    IEnumerator ActivateRigidBodies(bool activated)
    {
        yield return new WaitForEndOfFrame();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !activated;
        }
    }
    
    public void SetRagdollState(bool activated)
    {
        anim.enabled = !activated;
        StartCoroutine(ActivateRigidBodies(activated));
    }
}
