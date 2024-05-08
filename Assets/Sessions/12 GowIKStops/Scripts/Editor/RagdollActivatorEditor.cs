using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RagdollActivator))]
public class RagdollActivatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Activate Ragdoll State"))
        {
            var tg = target as RagdollActivator;
            tg.SetRagdollState(true);
        }
        
        if (GUILayout.Button("Deactivate Ragdoll State"))
        {
            var tg = target as RagdollActivator;
            tg.SetRagdollState(false);
        }
    }
}
