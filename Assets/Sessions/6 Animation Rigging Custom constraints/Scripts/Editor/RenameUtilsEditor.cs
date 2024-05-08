using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RenameUtils))]
public class RenameUtilsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tg = target as RenameUtils;

        if (GUILayout.Button("RenamePrefixes"))
        {
            tg.RenamePrefixes();
        }
        
        
        if (GUILayout.Button("RenameSuffixes"))
        {
            tg.RenameSuffixes();
        }
    }
}
