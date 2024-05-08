using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public struct FootIkJob : IWeightedAnimationJob
{
    private ReadWriteTransformHandle footBone;
    private ReadOnlyTransformHandle detectionReference;
    
    public void ProcessAnimation(AnimationStream stream)
    {
        throw new NotImplementedException();
    }

    public void ProcessRootMotion(AnimationStream stream)
    {
        throw new NotImplementedException();
    }

    public FloatProperty jobWeight { get; set; }
}

[Serializable]
public struct FootIkData : IAnimationJobData
{
    public Transform detectionReference;
    public Transform footBone;

    public float detectionStart;
    
    public bool IsValid()
    {
        return detectionReference != null && footBone != null;
    }

    public void SetDefaultValues()
    {
        detectionReference = null;
        footBone = null;
        detectionStart = 0.5f;
    }
}

public class FootIkBinder : AnimationJobBinder<FootIkJob, FootIkData>
{
    public override FootIkJob Create(Animator animator, ref FootIkData data, Component component)
    {
        throw new NotImplementedException();
    }

    public override void Update(FootIkJob job, ref FootIkData data)
    {
        base.Update(job, ref data);
        
    }

    public override void Destroy(FootIkJob job)
    {
        throw new NotImplementedException();
    }
}

[DisallowMultipleComponent, AddComponentMenu("FootIkConstraint")]
public class FootIkConstraint : RigConstraint<FootIkJob, FootIkData, FootIkBinder>
{
    
}
