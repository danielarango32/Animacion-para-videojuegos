using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Gameplay.ARPG
{
    public class ARPGCharacterVFXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem lockOnVFXPrefab;

        private ParticleSystem lockOnVFX;
        
        public void ActivateLockOn(Transform targetTransform)
        {
            if (lockOnVFX == null)
            {
                lockOnVFX = Instantiate<ParticleSystem>(lockOnVFXPrefab);
                lockOnVFX.gameObject.SetActive(false);
            }
            ParentConstraint constraint = lockOnVFX.GetComponent<ParentConstraint>();
            if (targetTransform != null)
            {
                lockOnVFX.gameObject.SetActive(true);
              
                constraint.constraintActive = false;
                lockOnVFX.transform.position = targetTransform.position;
                constraint.SetSource(0, new ConstraintSource{sourceTransform = targetTransform, weight = 1});
                constraint.constraintActive = true;
            }
            else
            {
                lockOnVFX.Stop(true);
                constraint.constraintActive = false;
                constraint.SetSource(0, new ConstraintSource());
                lockOnVFX.transform.position = Vector3.one * -100;
            }
        }
    }
}
