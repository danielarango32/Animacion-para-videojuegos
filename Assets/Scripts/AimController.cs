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
    [SerializeField] private Camera camera;
    [SerializeField] private Vector2 verticalThreshold; //Normalizar
    [SerializeField] private Vector2 horizontalThreshold;
    [SerializeField] private Transform aimCameraRig;

    Animator animator;

    public void Aim(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        if (cameraRotationTarget == null)
            return;
        cameraRotationTarget.RotateAround(transform.position, transform.up, inputValue.x * Time.deltaTime * 1.3f);
    }

    public void Aiming(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        Vector2 normalizedPointerPosition = inputValue / new Vector2(camera.pixelWidth, camera.pixelHeight);
        bool shouldRotateHorizontal = normalizedPointerPosition.x < horizontalThreshold.x ||
                                      normalizedPointerPosition.x > horizontalThreshold.y;
        bool shouldRotateVertical = normalizedPointerPosition.y < verticalThreshold.x ||
                                    normalizedPointerPosition.y > verticalThreshold.y;
        Vector2 direcctionalPointerPosition = normalizedPointerPosition * Vector2.one - Vector2.one;
        if (shouldRotateHorizontal)
        {
            aimCameraRig.RotateAround(transform.position, transform.up, direcctionalPointerPosition.x * lookSpeed * Time.deltaTime * 1.3f);
        }
        Vector3 projectedPoint = camera.ScreenToWorldPoint(new Vector3(inputValue.x, inputValue.y, 10));
        Debug.DrawLine(camera.transform.position, projectedPoint, Color.cyan, 0.3f);
        aimTarget.position = projectedPoint;
        
        
        

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
