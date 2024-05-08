using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Session3
{
    public class AnimationController : MonoBehaviour
    {
        enum MotionState
        {
            NotInCombat,
            InCombat
        }
        
        private Animator animator;
        private Vector2 currentInput;
        private Vector2 nextInput;
        private Vector2 inputVelocity;
        private MotionState motionState = MotionState.NotInCombat;

        private int motionXId, motionYId;

        private Vector3 projectedVector;
        private Quaternion desiredRotation;
        private Quaternion currentRotation;
        private float rotationSpeed;

        private bool moving;
        
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            motionXId = Animator.StringToHash("MotionX");
            motionYId = Animator.StringToHash("MotionY");
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
            Vector2 motionValue = context.ReadValue<Vector2>();
            Debug.Log(motionValue);
            nextInput = motionValue;
        }

        private void Update()
        {
            currentInput = Vector2.SmoothDamp(currentInput, nextInput, ref inputVelocity, 0.5f);
            if (motionState == MotionState.NotInCombat)
            {
                //Calcular direccion de movimiento
                Transform cameraTransform = Camera.main.transform;
                Vector3 cameraForward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up,
                    Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up)));

                Vector3 cameraRight = cameraTransform.right;
                
                projectedVector = Vector3.ProjectOnPlane(cameraForward, transform.up).normalized * currentInput.y + cameraRight * currentInput.x;
                projectedVector = projectedVector.normalized;

                currentRotation = Quaternion.LookRotation(projectedVector,transform.up);
                
                transform.rotation = currentRotation;

                //Setear la animacion
                animator.SetFloat(motionYId, projectedVector.magnitude);
            }

            //Si no se esta moviendo, limitar la cantidad que se resta el input
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
}
