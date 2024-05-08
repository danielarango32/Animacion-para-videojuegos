using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FootIkRootSolver_Mazamorra : MonoBehaviour
{
    [SerializeField] private bool enableSolver;
    [SerializeField] private Transform characterRoot;
    [SerializeField] private float readjustmentThreshold;
    [SerializeField] private float readjustmentSpeed = 15.0f;
    [SerializeField] private Rigidbody rigidBody;

    [SerializeField]private List<float> heightOffsets = new List<float>();
    private Animator animator;

    private Vector3 rootTarget;
    private Vector3 currentRootPosition;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentRootPosition = characterRoot.position;
    }

    private void OnAnimatorMove()
    {

        float minimumOffset = heightOffsets.Count >= 2 ? heightOffsets.Min(x => x) : 0.0f;
        rigidBody.isKinematic = true;
        animator.applyRootMotion = true;
        if (minimumOffset > readjustmentThreshold)
        {
            rootTarget = characterRoot.TransformPoint(new Vector3(0, minimumOffset, 0));
        }
        else if (minimumOffset < 0)
        {
            rigidBody.isKinematic = false;
            animator.ApplyBuiltinRootMotion();
        }
        else
        {
            rootTarget = characterRoot.position;
        }

        currentRootPosition = Vector3.Lerp(currentRootPosition, rootTarget, Time.deltaTime * readjustmentSpeed);
        characterRoot.position = currentRootPosition;
        heightOffsets.Clear();
    }

    public void UpdateTargetOffset(float heightValue)
    {
        heightOffsets.Add(heightValue);
    }



    public Vector3 RootTarget => rootTarget;
}
