using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PickupRigController))]
public class PickupRigControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tg = target as PickupRigController;
        if (GUILayout.Button("Grab"))
        {
            tg.PickUpNearestObject();
        }
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected, typeof(PickupRigController))]
    public static void DrawGizmos(Component component, GizmoType gizmoType)
    {
        string[] quadrantNames = new[] { "Upper Left", "Upper Right", "Lower Right", "Lower Left" };
        
        PickupRigController tg = (PickupRigController)component;
        Gizmos.matrix = tg.TargetReference.localToWorldMatrix;
        Handles.matrix = tg.TargetReference.localToWorldMatrix;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * tg.MaxReachingDistance);

        Gizmos.color = Color.red;
        for (int i = 0; i < tg.Quadrants.Count; i++)
        {
            Vector3 localDirection = tg.Quadrants[i].localDirection;
            Gizmos.DrawLine(Vector3.zero, localDirection);
            Handles.Label( localDirection, quadrantNames[i]);
        }
    }
}
