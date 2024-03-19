using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FottIkRootSolver : MonoBehaviour
{
    [SerializeField] private Transform characterRoot;
    [SerializeField] private float readjustmentThreshold;
    [SerializeField] private float readAdjustmentSpeed = 15.0f;
    [SerializeField] private Rigidbody rigidbody;
    private List<float> heightOffsets = new List<float>();

    private Vector3 rootTarget;
    private Vector3 currentRootPosition;
    private void OnAnimatorMove()
    {
        if (heightOffsets.Count >= 2)
        {
            float minimumOffset = MathF.Min(heightOffsets[0], heightOffsets[1]);
            if (minimumOffset > readjustmentThreshold)
            {
                rootTarget = characterRoot.TransformPoint(new Vector3(0, minimumOffset, 0));
                rigidbody.isKinematic = true;
            }
            else
            {
                rigidbody.isKinematic = false;
                rootTarget = characterRoot.position;
            }
        }
        else
        {
            rigidbody.isKinematic = false;
            rootTarget = characterRoot.position;
        }
        

        currentRootPosition = Vector3.Lerp(currentRootPosition, rootTarget, Time.deltaTime * readAdjustmentSpeed);
        characterRoot.position=currentRootPosition;
        heightOffsets.Clear();
    }

    public void UpdateTargetOffset(float heightValue)
    {
        heightOffsets.Add(heightValue);
    }

    public Vector3 RootTarget => rootTarget;
}
