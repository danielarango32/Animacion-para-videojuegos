using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoseCopy))]
public class PoseCopyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("CopyPose"))
        {
            var tg = target as PoseCopy;
            tg.CopyPose();
        }
    }
}
