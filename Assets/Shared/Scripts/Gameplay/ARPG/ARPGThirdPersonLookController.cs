using Gameplay.ARPG;
using UnityEngine;
using UnityEngine.InputSystem;

public class ARPGThirdPersonLookController : MonoBehaviour, IARPGCharacterComponent
{

    [SerializeField] private bool invertLookYAxis = true;
    [SerializeField] private float lookSpeed = 180f;
    public ARPGCharacter Character => GetComponent<ARPGCharacter>();

    private Transform lockTarget;
    
    private Vector2 lookInputValue;
    private Vector2 nextLookInputValue;
    private Vector2 lookInputValueVelocity;
    
    public void OnLook(InputAction.CallbackContext stickValue)
    {
        if (lockTarget != null) return;
        nextLookInputValue = stickValue.ReadValue<Vector2>();
    }

    public void SetLockTarget(Transform t)
    {
        lockTarget = t;
    }

    private void Update()
    {
        ARPGThirdPersonCharacterCameraRig rig = Character.CameraRig as ARPGThirdPersonCharacterCameraRig;
        Transform rigRotationReference = rig.RigRotationReference;
        float xRotation = rigRotationReference.localEulerAngles.x > 180
            ? 360 - rigRotationReference.localEulerAngles.x
            : rigRotationReference.localEulerAngles.x;

        if (xRotation > -30 || xRotation < 30)
        {
            rigRotationReference.Rotate(new Vector3(lookInputValue.y * (invertLookYAxis? -1:1) * lookSpeed * Time.deltaTime,lookInputValue.x * lookSpeed * Time.deltaTime, 0), Space.Self);
        }
        
        lookInputValue = Vector2.SmoothDamp(lookInputValue, nextLookInputValue, ref lookInputValueVelocity, 0.2f,
            Mathf.Infinity);
    }
}
