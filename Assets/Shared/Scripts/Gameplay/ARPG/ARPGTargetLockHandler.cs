using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Gameplay.ARPG
{
    public class ARPGTargetLockHandler : MonoBehaviour, IARPGCharacterComponent, IInitializableGameCharacterComponent
    {
        public ARPGCharacter Character => GetComponent<ARPGCharacter>();

        public TransformUnityEvent OnTargetLocked;

        [SerializeField] private float lockDistance = 10f;
        [SerializeField] private LayerMask lockLayerMask;
        private Transform lockedObject;

        private Vector3 startLookAtTargetPoistion;
        
        private void UnlockTarget()
        {
            lockedObject = null;
            ARPGThirdPersonCharacterCameraRig rig = (ARPGThirdPersonCharacterCameraRig)(Character.CameraRig);
            Transform cameraRigRotationReference = rig.RigRotationReference;
            
            
            cameraRigRotationReference.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            rig.SetLiveVirtualCamera(0);
            rig.RigLookAtReference.parent = cameraRigRotationReference;
            rig.RigLookAtReference.localPosition = startLookAtTargetPoistion;
            OnTargetLocked?.Invoke(null);
        }
        
        private void LockTarget()
        {
            ARPGThirdPersonCharacterCameraRig rig = (ARPGThirdPersonCharacterCameraRig)(Character.CameraRig);
            Collider[] colliders = Physics.OverlapSphere(transform.position, lockDistance, lockLayerMask);
            if (colliders.Length == 0)
            {
                UnlockTarget();
                return;
            }

            Camera userCamera = Character.Owner.CameraManager.MainCamera;
            
            lockedObject = colliders.OrderByDescending(col =>
                Mathf.Abs(Vector3.Dot((col.transform.position - userCamera.transform.position).normalized,
                    userCamera.transform.forward)) - 1).First().transform;

            rig.RigLookAtReference.parent = lockedObject;
            rig.RigLookAtReference.position = lockedObject.position;
            
            rig.SetLiveVirtualCamera(1);
            OnTargetLocked?.Invoke(lockedObject);
        }

        public void OnOwned()
        {
            ARPGThirdPersonCharacterCameraRig rig = (ARPGThirdPersonCharacterCameraRig)(Character.CameraRig);
            startLookAtTargetPoistion = rig.RigLookAtReference.localPosition;
        }

        private void Update()
        {
            if (lockedObject != null && Vector3.Distance(transform.position, lockedObject.position) > lockDistance)
            {
                UnlockTarget();
            }
        }

        public void PerformLock(CallbackContext buttonContext)
        {
            if (lockedObject != null)
            {
                UnlockTarget();
                return;
            }
            LockTarget();
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lockDistance);
        }
#endif
    }
    
    
}
