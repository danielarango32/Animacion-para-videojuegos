using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TwoBoneIkConstraintDriver : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint[] directValueConstraints;
    [SerializeField] private TwoBoneIKConstraint[] inverseValueConstraints;

    [SerializeField] [Range(0, 1)] private float weight;
    public void SetValue(float val)
    {
        foreach (TwoBoneIKConstraint directValueConstraint in directValueConstraints)
        {
            if(directValueConstraint == null) continue;
            directValueConstraint.weight = val;
        }
        
        foreach (TwoBoneIKConstraint inverseValueConstraint in inverseValueConstraints)
        {
            if(inverseValueConstraint == null) continue;
            inverseValueConstraint.weight = 1-val;
        }
    }

    private void OnValidate()
    {
        SetValue(weight);
    }
}
