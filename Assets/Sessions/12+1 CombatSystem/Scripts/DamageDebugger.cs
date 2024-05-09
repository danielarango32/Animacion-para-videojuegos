using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDebugger : MonoBehaviour
{

    [SerializeField] CharacterDamage characterDamage;
    [SerializeField] private float baseDamage;
    public void DoDamage()
    {
        Vector3 characterSpacePosition = characterDamage.transform.InverseTransformPoint(transform.position).normalized;
        float x = characterSpacePosition.x;
        x = Mathf.Sign(x)*Mathf.Ceil(Mathf.Abs(x));

        float y = MathF.Abs(x)>=MathF.Abs(characterSpacePosition.z) ? 0: characterSpacePosition.z;
        y = Mathf.Sign(y)*Mathf.Ceil(Mathf.Abs(y));

        characterDamage.RecieveDamage(baseDamage, new Vector2(x,y));
    }

}
