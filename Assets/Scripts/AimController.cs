using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonShooter;
using UnityEngine.InputSystem;
using PlayerState = ThirdPersonShooter.ThirdPersonShooterPlayerData.PlayerState;
using Cinemachine;

[RequireComponent(typeof(Animator))]
public class AimController : ThirdPersonShooterPlayerScript
{
    [SerializeField] private Transform cameraRotationTarget;
    [SerializeField] private float lookSpeed = 30;
    [SerializeField] CinemachineVirtualCamera aimingCamera;
    [SerializeField] private Transform aimTarget;

    Animator animator;

    public void Aim(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        if (cameraRotationTarget == null)
            return;
        cameraRotationTarget.RotateAround(transform.position, transform.up, inputValue.x * Time.deltaTime * 1.3f);
        aimTarget.position = Vector3.ProjectOnPlane(aimTarget.position + aimingCamera.transform.right * inputValue.x + aimingCamera.transform.right * inputValue.y, aimingCamera.transform.forward * 10);

    }
    public void ToogleAim(InputAction.CallbackContext context)
    {
        float inputValue = context.ReadValue<float>();
        animator.SetBool("TargetAim", inputValue > 0);
        aimingCamera.gameObject.SetActive(inputValue > 0);
        //playerData.State = inputValue > 0 
        //    ? ThirdPersonShooterPlayerData.PlayerState.AimingMode 
        //    : ThirdPersonShooterPlayerData.PlayerState.NormalMode;
    }

    //protected override void OnStateChanged(ThirdPersonShooterPlayerData.PlayerState state)
    //{
    //    //animator.SetBool("TargetAim", state == PlayerState.AimingMode);
    //}

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
