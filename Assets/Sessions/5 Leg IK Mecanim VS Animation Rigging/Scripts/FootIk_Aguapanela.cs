
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatEvent : UnityEvent<float>
{
    
}

[RequireComponent(typeof(Animator))]
public class FootIk_Aguapanela : MonoBehaviour
{
    [SerializeField] private Transform detectionReference; //Caderas
    [SerializeField] private Transform foot; //Pie https://es.wikipedia.org/wiki/Pie
    [SerializeField] private Transform root; //Raiz 
    [SerializeField][Range(0,1)] private float detectionRange;//interpolante para punto intermedio entre cadera y pie con ejes x,z en 0s
    [SerializeField] private float maxDetectionDistance; //Maximo rango de rayo para detectar superficies
    [SerializeField] private AvatarIKGoal ikGoal;
    [SerializeField] private Vector2 snapOffsets; //x: desfase en reposo, y: desfase al levantar el pie
    [SerializeField] private string snapOffsetParameter;
    [SerializeField] private float snapSpeed = 5;
    [SerializeField] private Vector3 snapRotationOffset; //Desfase para ajustar rotacion del pie

    public FloatEvent onIkSolved;
    public UnityEvent onIkNotSolved;

    private Animator animator;
    
    private bool hasTarget; 
    private RaycastHit ikTarget;

    private Vector3 currentIkPosition;
    private bool waitOneFrame;
    
    /// <summary>
    /// Obtener el punto inicial desde el cual se lanzara el rayo para detectar superficies
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDetectionStartPosition()
    {
        Vector3 referenceSpacePosition = detectionReference.InverseTransformPoint(foot.position);
        Vector3 ret = new Vector3(referenceSpacePosition.x, referenceSpacePosition.y * detectionRange, referenceSpacePosition.z);
        return detectionReference.TransformPoint(ret);
    }

    /// <summary>
    /// Detectar y actualizar posiciones emnm superficies que intersecten con el rayo
    /// </summary>
    private bool GetTargetPosition()
    {
        return Physics.Raycast(GetDetectionStartPosition(), -detectionReference.up, out ikTarget, maxDetectionDistance);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentIkPosition = foot.position;
    }

    /// <summary>
    /// Detectar superficies, TODO: aplicar posiciones a huesos ik
    /// </summary>
    /// <param name="layerIndex"></param>
    private void OnAnimatorIK(int layerIndex)
    {
        hasTarget = GetTargetPosition();

        currentIkPosition = Vector3.Lerp(currentIkPosition, hasTarget ? foot.position : ikTarget.point, Time.deltaTime * snapSpeed);

        if (hasTarget)
        {
            animator.SetIKPositionWeight(ikGoal, 1.0f);
            float snapInterpolator = animator.GetFloat(snapOffsetParameter);
            float solvedSnapOffset = Mathf.Lerp(snapOffsets.x, snapOffsets.y, snapInterpolator);
            animator.SetIKPosition(ikGoal, currentIkPosition + detectionReference.up * solvedSnapOffset);
            animator.SetIKRotationWeight(ikGoal, snapInterpolator);
            Quaternion rot = Quaternion.LookRotation(ikTarget.normal) * Quaternion.Euler(snapRotationOffset);
            animator.SetIKRotation(ikGoal, rot);
            Vector3 characterSpaceFoot = root.InverseTransformPoint(ikTarget.point);
            onIkSolved?.Invoke(characterSpaceFoot.y);

        }
        else
            onIkSolved?.Invoke(-1);
    }

    private void FixedUpdate()
    {
    }

    public Transform DetectionReference => detectionReference;
    public float MaxDetectionDistance => maxDetectionDistance;

    public bool HasTarget => hasTarget;
}
