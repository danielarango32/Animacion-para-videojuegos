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
    private Vector3 proyectedVector;

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
            Transform cameraTransform =Camera.main.transform;
            Vector3 cameraFoward = Vector3.Lerp(cameraTransform.forward, cameraTransform. up, Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up)));
            proyectedVector = Vector3.ProjectOnPlane(cameraFoward, transform.up);

            //Rotar la direccion de movimiento
            //Setear la animación
            animator.SetFloat(MotionYId, currentInput.y);
        }
    
        
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + proyectedVector);
    }
#endif
}
