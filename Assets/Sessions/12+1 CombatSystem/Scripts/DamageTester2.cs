using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DamageDebugger))]

public class DamageTester2 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Do Damage"))
        {
            var tg= target as DamageDebugger;
            tg.DoDamage();
        }
    }
}
