using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class animationController : MonoBehaviour
{
    private Animator animator;
    private Vector2 currentInput;
    private Vector2 nextInput;
    private Vector2 inputVelocity;
    private Motionstates motionstates = Motionstates.NotInCombat;
    private Vector3 projectedVector;
    private Quaternion desiredRotation;
    private Quaternion currentRotation;
    private float rotationSpeed;
    private bool moving;

    int MotionXId, MotionYId;

    enum Motionstates
    {
        NotInCombat,
        InCombat
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        MotionXId = Animator.StringToHash("MotionX");
        MotionYId = Animator.StringToHash("MotionY");
    }
    public void Move(CallbackContext context)
    {
        if (context.canceled)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }
        Vector2 motionvalue = context.ReadValue<Vector2>();
        Debug.Log(motionvalue);
        nextInput = motionvalue;
        
    }

    public void Update()
    {
        currentInput = Vector2.SmoothDamp(currentInput, nextInput,ref inputVelocity, 0.4f);

        if (motionstates == Motionstates.NotInCombat)
        {
            //Calcular direccin de movimientos
            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraFoward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up,
                Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up)));

            Vector3 cameraRight = cameraTransform.right;
            projectedVector = Vector3.ProjectOnPlane(cameraFoward, transform.up).normalized * currentInput.y + cameraRight * currentInput.x;
            projectedVector = projectedVector.normalized;   


            currentRotation = Quaternion.LookRotation(projectedVector, transform.up);
            transform.rotation = currentRotation;

            //Setear la animación
            animator.SetFloat(MotionYId, projectedVector.magnitude);
        }

        
        if (!moving)
        {
            nextInput = Vector2.zero;
            nextInput.Set(Mathf.Clamp(nextInput.x, -0.1f, 0.1f), Mathf.Clamp(nextInput.y, -0.1f, 0.1f));
        }
    
        
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + projectedVector);
    }
#endif
}
