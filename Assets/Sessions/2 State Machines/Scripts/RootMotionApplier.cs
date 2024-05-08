using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionApplier : MonoBehaviour
{
    private void OnAnimatorMove()
    {
        Animator anim = GetComponent<Animator>();
        float motionMagnitude = anim.GetFloat("RootMotion");
        transform.Translate(Vector3.forward * motionMagnitude, Space.Self);
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
