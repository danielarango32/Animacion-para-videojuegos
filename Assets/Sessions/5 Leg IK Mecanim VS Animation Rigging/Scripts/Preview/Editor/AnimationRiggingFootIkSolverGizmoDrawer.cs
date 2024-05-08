using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AnimationRiggingFootIkSolverGizmoDrawer
{

    private static void DrawDataForFoot(HumanoidFootIkSolver solver, AvatarIKGoal goal)
    {
        Vector3 detectionStartPoint = solver.GetDetectionStartPoint(goal);
        Vector3 detectionEndPoint = detectionStartPoint - solver.DetectionReference.up * solver.MaxDetectionDistance;
        Gizmos.color = solver.QuerySnapForFoot(goal) ? Color.green : Color.red;
        Handles.color = Gizmos.color;
        Handles.DrawWireDisc(detectionStartPoint,solver.DetectionReference.up, 0.05f);
        Handles.DrawWireDisc(detectionEndPoint,solver.DetectionReference.up, 0.05f);
        Gizmos.DrawLine(detectionStartPoint, detectionEndPoint);
        Gizmos.DrawSphere(solver.GetSnapData(goal).point, 0.05f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(solver.GetFoot(goal).position, solver.GetFoot(goal).position - solver.DetectionReference.up * solver.SurfaceOffset);
    }
    [DrawGizmo(GizmoType.Active | GizmoType.NonSelected,typeof(HumanoidFootIkSolver))]
    public static void DrawGizmos(Component component, GizmoType gizmoType)
    {
        HumanoidFootIkSolver solver = component as HumanoidFootIkSolver;
        if (solver == null) return;
        DrawDataForFoot(solver, AvatarIKGoal.LeftFoot);
        DrawDataForFoot(solver, AvatarIKGoal.RightFoot);
    }
}
