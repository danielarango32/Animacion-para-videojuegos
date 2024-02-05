using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class animationController : MonoBehaviour
{
    Animator animator;
    Vector2 currentInput;
    Vector2 nextInput;
    Vector2 inputVelocity;



    int MotionXId, MotionYId;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        MotionXId = Animator.StringToHash("MotionX");
        MotionYId = Animator.StringToHash("MotionY");
    }
    public void Move(CallbackContext context)
    {
        Vector2 motionvalue = context.ReadValue<Vector2>();
        Debug.Log(motionvalue);
        nextInput = motionvalue;
    }

    public void Update()
    {
        currentInput = Vector2.SmoothDamp(currentInput, nextInput,ref inputVelocity, 0.4f);
        animator.SetFloat(MotionXId, currentInput.x);
        animator.SetFloat(MotionYId, currentInput.y);
    }
}
