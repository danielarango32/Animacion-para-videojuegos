using UnityEngine;
using Unity.Mathematics;
public class AutoAddFloatParameter : StateMachineBehaviour
{
    [SerializeField] private string targetParameter;
    [SerializeField] private string sourceParameter;
    [SerializeField] private float sourceThreshold;
    [SerializeField] private float exitValue;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float sourceValue = animator.GetFloat(sourceParameter);
        if (sourceValue > sourceThreshold)
        {
            Debug.Log("Should reset speed");
            animator.SetFloat(targetParameter, sourceValue);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(targetParameter, exitValue);
    }
}
