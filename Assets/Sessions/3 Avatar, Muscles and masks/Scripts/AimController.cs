using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ThirdPersonShooter;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerState = ThirdPersonShooter.ThirdPersonShooterPlayerData.PlayerState;

[RequireComponent(typeof(Animator))]
public class AimController : ThirdPersonShooterPlayerScript
{
    [SerializeField] private Transform cameraRotationTarget;
    [SerializeField] private float lookSpeed = 30;
    [SerializeField] private CinemachineVirtualCamera aimingCamera;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Vector2 verticalThreshold; //Normalized screen coords
    [SerializeField] private Vector2 horizontalThreshold; //Normalized screen coords
    [SerializeField] private Transform aimCameraRig;

    private Animator animator;
    
    public void Look(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        if (cameraRotationTarget == null) return;
        
        cameraRotationTarget.RotateAround(transform.position, transform.up, inputValue.x * lookSpeed * Time.deltaTime);
    }

    public void Aim(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>(); //Posicion del mouse en la pantalla (en pixeles)
        //Proyeccion por debajo:
        // 1. se normaliza punto en pantalla 
        // 2. InverseProjection * punto
        // 3. InverseView * Punto
        Vector2 normalizedPointerPosition = inputValue / new Vector2(camera.pixelWidth, camera.pixelHeight);
        
        bool shouldRotateHorizontal = normalizedPointerPosition.x < horizontalThreshold.x ||
                                      normalizedPointerPosition.x > horizontalThreshold.y;
        bool shouldRotateVertical = normalizedPointerPosition.y < verticalThreshold.x ||
                                    normalizedPointerPosition.y > verticalThreshold.y;
        Vector2 directionalPointerPosition = normalizedPointerPosition * Vector2.one * 2 - Vector2.one; //Remapear pantalla de 0 a 1 a -1 a 1
        if(shouldRotateHorizontal)
            aimCameraRig.RotateAround(transform.position, transform.up, directionalPointerPosition.x * lookSpeed * Time.deltaTime);
        Vector3 projectedPoint = camera.ScreenToWorldPoint(new Vector3(inputValue.x, inputValue.y, 10));
        Debug.DrawLine(camera.transform.position, projectedPoint, shouldRotateHorizontal || shouldRotateVertical ? Color.red : Color.cyan, 0.2f);
        aimTarget.position = projectedPoint;
    }

    public void ToggleAim(InputAction.CallbackContext context)
    {
        float inputValue = context.ReadValue<float>();
        animator.SetBool("ToggleAim", inputValue > 0);
        aimingCamera.gameObject.SetActive(inputValue > 0);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
