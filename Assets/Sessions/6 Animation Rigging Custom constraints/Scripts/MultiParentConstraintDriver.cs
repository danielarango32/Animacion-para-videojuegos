using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MultiParentConstraintDriver : MonoBehaviour
{
    [Serializable]
    public struct MultiParentConstraintActivationSettings
    {
        public MultiParentConstraint constraint;
        public bool editMainWeight;
        public bool editPerTransformWeights;
        public int[] perTransformWeights;

        public bool isValid => constraint != null;

        public void SetWeight(float val)
        {
            if (!isValid) return;

            if(editMainWeight) constraint.weight = val;
            if (editPerTransformWeights)
            {
                foreach (int i in perTransformWeights)
                {
                    try
                    {
                        constraint.data.sourceObjects.SetWeight(i, val);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
    
    [SerializeField] private MultiParentConstraintActivationSettings[] directValueConstraints;
    [SerializeField] private MultiParentConstraintActivationSettings[] inverseValueConstraints;
    
    
    [SerializeField][Range(0,1)] private float weight;

    public void SetValue(float val)
    {
        foreach (MultiParentConstraintActivationSettings directValueConstraint in directValueConstraints)
        {
            directValueConstraint.SetWeight( val);
        }

        foreach (MultiParentConstraintActivationSettings inverseValueConstraint in inverseValueConstraints)
        {
            inverseValueConstraint.SetWeight(1 - val);
        }
    }
    
    private void OnValidate()
    {
        SetValue(weight);
    }
}
