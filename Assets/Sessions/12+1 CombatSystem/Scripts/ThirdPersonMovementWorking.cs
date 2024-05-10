using System.Collections;
using System.Collections.Generic;
using ThirdPersonShooter;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof (PlayerInput))]
public class ThirdPersonMovementWorking : ThirdPersonShooterPlayerScript
{
    private readonly Vector2Dampener inputDampener = new Vector2Dampener(0.5f, 3f);
    private ThirdPersonShooterCameraManager cameraManager;
    private Animator animator;

    public void Move(InputAction.CallbackContext context)
    {
        this.inputDampener.TargetValue = context.ReadValue<Vector2>();
    }

    private Vector3 GetDirectionVector()
    {
        Transform cameraTransform = this.cameraManager.CameraTransform;
        return (Vector3.ProjectOnPlane(Vector3.Lerp(cameraTransform.forward, cameraTransform.up, Mathf.Abs(Vector3.Dot(cameraTransform.forward, this.transform.up))), this.transform.up) * this.inputDampener.CurrentValue.y + cameraTransform.right * this.inputDampener.CurrentValue.x).normalized;
    }

    private void Awake()
    {
        this.cameraManager = this.GetComponent<ThirdPersonShooterCameraManager>();
        this.animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        this.inputDampener.Update();
        if (this.playerData.State == ThirdPersonShooterPlayerData.PlayerState.NormalMode)
        {
            this.transform.rotation = Quaternion.LookRotation(this.GetDirectionVector(), this.transform.up);
            this.animator.SetFloat("MotionY", this.inputDampener.CurrentValue.magnitude);
            this.animator.SetFloat("MotionX", 0.0f);
        }
        else
        {
            this.animator.SetFloat("MotionY", this.inputDampener.CurrentValue.y);
            this.animator.SetFloat("MotionX", this.inputDampener.CurrentValue.x);
        }
    }
}
