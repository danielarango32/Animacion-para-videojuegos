using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReduceIntParameter : StateMachineBehaviour
{
    [SerializeField] private string loopCountParameter;

    private int currentLoopIndex;
    private void UpdateValue(Animator animator)
    {
        int val = animator.GetInteger(loopCountParameter) - 1;
        Debug.Log(val);
        val = val < 0 ? 0 : val;
        animator.SetInteger(loopCountParameter, val);     
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentLoopIndex = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Floor(stateInfo.normalizedTime) > currentLoopIndex)
        {
            currentLoopIndex++;
            UpdateValue(animator);
        }
    }
}
