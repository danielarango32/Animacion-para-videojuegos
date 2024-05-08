using System;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class LineRendererTransformSetter : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.positionCount = transforms.Length;
        lineRenderer.SetPositions(transforms.Select(t => t.position).ToArray());
    }
}
