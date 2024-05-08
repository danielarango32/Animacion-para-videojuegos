using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UpdateParamBasedOnAnotherParam : StateMachineBehaviour
{
    [SerializeField][Range(0,1)] private float updateThreshold;
    [SerializeField] private string sourceParameter;
    [SerializeField] private string targetParameter;
    
    private float startTargetValue;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        startTargetValue = animator.GetFloat(targetParameter);
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float sourceValue = animator.GetFloat(sourceParameter);
        if (sourceValue > updateThreshold)
        {
            animator.SetFloat(targetParameter, sourceValue);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(targetParameter, startTargetValue);
    }
}
