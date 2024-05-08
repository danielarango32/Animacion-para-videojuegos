using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponOverrideDriver : MonoBehaviour
{

    [SerializeField][Range(0,1)] private float weaponOverrideState;
    public FloatEvent onOverrideStateChanged;


    [SerializeField] private RigBuilder builder;
    [SerializeField] private MultiParentConstraint[] rightHandFingersWeaponSnap;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;

    [SerializeField] private MultiParentConstraint weaponDriver;

    [SerializeField] private Animator anim;
    [SerializeField] private string layerName;
    
    //When weapon overrides:
    //both arms respond to IK
    //      IK weight
    //      Fingers multi-parent
    //shoulder rotates with stock recoil
    //weapon aim driven with aim in multi referential
    //spine responds to aim



    //When hand overrides:
    //Weapon responds to right hand
    //Left hand responds to weapon
    //stock, muzzle and aim referentials disabled
    //spine responds to animation

    private void Update()
    {
        anim.SetLayerWeight(anim.GetLayerIndex(layerName), weaponOverrideState);
        foreach (MultiParentConstraint constraint in rightHandFingersWeaponSnap)
        {
            constraint.weight = weaponOverrideState;
        }
        rightHandIK.weight = weaponOverrideState;
        weaponDriver.data.sourceObjects.SetWeight(0, 1 - weaponOverrideState);
        weaponDriver.data.sourceObjects.SetWeight(1, weaponOverrideState);
        //if(builder != null)
          //  builder.Build();
    }
}
