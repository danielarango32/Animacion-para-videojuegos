using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PickupController))]
public class PickupControllerEditor : Editor

{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("PickUp"))
        {
            var tg = target as PickupController;
            tg.PickUpNearest();
        }
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected, typeof(PickupController))]
    public static void DrawGizmos(Component component, GizmoType gizmoType)
    {
        var tg = component as PickupController;
        var localToWorldMatrix = tg.TorsoReference.localToWorldMatrix;
        Gizmos.matrix = localToWorldMatrix;
        Handles.matrix = localToWorldMatrix;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward);
        Handles.color = Color.cyan;

        for (int i = 0; i < tg.SphereQuarterDirections.Count; i++)
        {
            Color c = Color.red;
            if (i >= 0 && i == tg.CurrentNearestQuarterDirection)
            {
                c = Color.green;
            }

            Gizmos.color = c;

            PickupController.SphereQuarterData data = tg.SphereQuarterDirections[i];
            
            Gizmos.DrawLine(Vector3.zero, data.localDirection);
            Handles.Label(data.localDirection, data.quarterId.ToString());
        }
    }
}
