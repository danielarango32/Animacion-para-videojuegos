using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionApplier : MonoBehaviour
{
    private void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();
        float motionMangnitude=animator.GetFloat("RootSpeed");
        transform.Translate(Vector3.forward * motionMangnitude, Space.Self);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
