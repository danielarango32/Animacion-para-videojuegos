using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.ARPG
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    
    public class ARPGThirdPersonMovementController :MonoBehaviour,IARPGCharacterComponent
    {
        //TODO: Implement gameplay framework for camera and input enabling
        [SerializeField] private float rotationSpeed = 180f;
        
        private readonly AnimatorParamHandle motionSpeedX = new AnimatorParamHandle("MotionSpeedX");
        private readonly AnimatorParamHandle motionSpeedY = new AnimatorParamHandle("MotionSpeedY");
        private readonly AnimatorParamHandle motionMagnitude = new AnimatorParamHandle("MotionMagnitude");
        private readonly AnimatorParamHandle attack = new AnimatorParamHandle("Attack");
        private readonly AnimatorParamHandle dodgeDirectionX = new AnimatorParamHandle("DodgeDirectionX");
        private readonly AnimatorParamHandle dodgeDirectionY = new AnimatorParamHandle("DodgeDirectionY");
        private readonly AnimatorParamHandle dodge = new AnimatorParamHandle("Dodge");
        
        private Animator animator;

        private Vector2 motionInputValue;
        private Vector2 nextMotionInputValue;
        private Vector2 motionInputValueVelocity;
        
        private Vector3 motionVector;
        private Vector3 forwardVector;

        private Rigidbody rigidbody;
        
        public ARPGCharacter Character => GetComponent<ARPGCharacter>();

        #region LazyReferences
        private Animator Animator
        {
            get
            {
                if (animator == null)
                {
                    animator = GetComponent<Animator>();
                }

                return animator;
            }
        }
        private Rigidbody Rigidbody
        {
            get
            {
                if (rigidbody == null)
                {
                    rigidbody = GetComponent<Rigidbody>();
                }

                return rigidbody;
            }
        }
        #endregion

        private Transform lockTarget;

        public void OnMove(InputAction.CallbackContext stickValue)
        {
            nextMotionInputValue = stickValue.ReadValue<Vector2>();
            Animator.SetFloat(dodgeDirectionX.hash, nextMotionInputValue.x);
            Animator.SetFloat(dodgeDirectionY.hash, nextMotionInputValue.y);
        }

        public void OnAttack(InputAction.CallbackContext buttonValue)
        {
            if (!buttonValue.action.IsPressed()) return;
            Animator.SetTrigger(attack.hash);
        }

        public void OnDodge(InputAction.CallbackContext buttonValue)
        {
            if (!buttonValue.action.IsPressed()) return;
            Animator.SetTrigger(dodge.hash);
        }

        public void SetLockTarget(Transform t)
        {
            lockTarget = t;
            Animator.SetLayerWeight(1, t == null ? 0 : 1);
        }

        //Essentially fixed update
        private void OnAnimatorMove()
        {
            Animator.ApplyBuiltinRootMotion();
            
            Transform userCameraTransform = Character.Owner.CameraManager.MainCamera.transform;
            var forward = userCameraTransform.forward;
            forwardVector = Vector3.Lerp(forward, userCameraTransform.up,
                Mathf.Abs(Vector3.Dot(forward, transform.up)));
            motionVector = Vector3.ProjectOnPlane(
                forwardVector * motionInputValue.y + userCameraTransform.right * motionInputValue.x,
                transform.up);

            if (lockTarget == null)
            {
                if (motionInputValue.magnitude > 0.1f)
                {
                   
                    Rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,
                        Quaternion.LookRotation(motionVector.normalized),
                        rotationSpeed * Time.deltaTime));
                }
            }
            else
            {
                Debug.Log(lockTarget);
                Vector3 projectedAimVector =
                    Vector3.ProjectOnPlane(lockTarget.position - transform.position, transform.up).normalized;
                Rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(projectedAimVector,Vector3.up),
                    rotationSpeed * Time.deltaTime));
            }
        }

        private void Update()
        {
            motionInputValue = Vector2.SmoothDamp(motionInputValue, nextMotionInputValue, ref motionInputValueVelocity,
                0.2f, Mathf.Infinity);
            Animator.SetFloat(motionSpeedX.hash, motionInputValue.x);
            Animator.SetFloat(motionSpeedY.hash, motionInputValue.y);
            Animator.SetFloat(motionMagnitude.hash, motionVector.magnitude);
        }
        

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Camera userCamera = Character.Owner.CameraManager.MainCamera;
            Vector3 nextForwardVector = Vector3.Lerp(userCamera.transform.forward, userCamera.transform.up,
                Mathf.Abs(Vector3.Dot(userCamera.transform.forward, transform.up)));
            Vector3 nextMotionVector = Vector3.ProjectOnPlane(
                nextForwardVector * nextMotionInputValue.y + userCamera.transform.right * nextMotionInputValue.x,
                transform.up);
            Gizmos.DrawLine(transform.position, transform.position + nextMotionVector * 2);
            Gizmos.color = new Color(0, 1, 1, 0.5f);

            Gizmos.DrawLine(transform.position, transform.position + motionVector * 2);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + forwardVector * 2);
        }
#endif
    }
}
