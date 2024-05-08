using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.TextCore.Text;

namespace Gameplay.ARPG
{
    [RequireComponent(typeof(ARPGCharacter))]
    public class ARPGThirdPersonCharacterCameraRig : GameCharacterVirtualCameraRig, IARPGCharacterComponent
    {
        private Transform rigRotationReference;
        private Transform rigLookAtReference;
        private CinemachineVirtualCamera normalCamera;
        private CinemachineVirtualCamera lockOnCamera;
        protected override void ConfigureRig(GameObject spawnedCameraRig)
        {
            rigRotationReference = spawnedCameraRig.transform.Q("RigRotationReference");

            ParentConstraint parentConstraint = rigRotationReference.GetComponent<ParentConstraint>();

            parentConstraint.constraintActive = false;
            parentConstraint.SetSource(0, new ConstraintSource{sourceTransform = Character.transform, weight = 1.0f});
            parentConstraint.constraintActive = true;

            rigLookAtReference = spawnedCameraRig.transform.Q("RigLookAtReference");
            normalCamera = spawnedCameraRig.transform.Q<CinemachineVirtualCamera>("NormalVirtualCamera");
            lockOnCamera = spawnedCameraRig.transform.Q<CinemachineVirtualCamera>("LockOnVirtualCamera");
            
            RegisterVirtualCamera(normalCamera);
            RegisterVirtualCamera(lockOnCamera);
            SetLiveVirtualCamera(normalCamera);
        }

        public Transform RigRotationReference => rigRotationReference;
        public Transform RigLookAtReference => rigLookAtReference;
        public CinemachineVirtualCamera NormalCamera => normalCamera;
        public CinemachineVirtualCamera LockOnCamera => lockOnCamera;
        public ARPGCharacter Character => GetComponent<ARPGCharacter>();
    }
}
