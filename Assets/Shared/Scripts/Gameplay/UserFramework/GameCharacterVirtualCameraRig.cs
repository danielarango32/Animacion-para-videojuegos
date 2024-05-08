using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Acts as a proxy to access virtual cameras registered in the current owner Game User, if the owner is an AI User then the camera controls will be ignored silently
/// </summary>
public class GameCharacterVirtualCameraRig : MonoBehaviour
{
    [SerializeField] private GameObject cameraRigPrefab;
    protected GameObject cameraRig;
    
    protected List<CinemachineVirtualCamera> virtualCameras = new();
    private CinemachineVirtualCamera liveVirtualCamera;
    public virtual CinemachineVirtualCamera LiveVirtualCamera => liveVirtualCamera;

    protected virtual void ConfigureRig(GameObject spawnedCameraRig)
    {
        
    }
    
    protected virtual void RegisterVirtualCamera(CinemachineVirtualCamera virtualCamera)
    {
        if (virtualCameras.Contains(virtualCamera)) return;
        virtualCameras.Add(virtualCamera);
    }

    public void SetLiveVirtualCamera(CinemachineVirtualCamera virtualCamera ,bool failNotRegistered = false)
    {
        if (LiveVirtualCamera == virtualCamera) return;
        if (!virtualCameras.Contains(virtualCamera))
        {
            if(failNotRegistered) return;
            RegisterVirtualCamera(virtualCamera);
        }
        virtualCamera.gameObject.SetActive(true);
        foreach (CinemachineVirtualCamera cinemachineVirtualCamera in virtualCameras)
        {
            if (cinemachineVirtualCamera == virtualCamera) continue;
            cinemachineVirtualCamera.gameObject.SetActive(false);
        }

        liveVirtualCamera = virtualCamera;
    }

    public void SetLiveVirtualCamera(int virtualCameraIndex)
    {
        if (virtualCameraIndex < 0 || virtualCameraIndex > virtualCameras.Count - 1)
        {
            #if UNITY_EDITOR
                //Debug.LogError($"Virtual camera at index ({virtualCameraIndex}) is not valid in dhis camera manager");
            #endif
            return;
        }
        CinemachineVirtualCamera virtualCamera = virtualCameras[virtualCameraIndex];
        SetLiveVirtualCamera(virtualCamera);
    }

    public void SpawnCameraRig()
    {
        if (cameraRig != null) return;
        if (cameraRigPrefab == null) return;
        cameraRig = Instantiate(cameraRigPrefab);
        ConfigureRig(cameraRig);
    }

    /// <summary>
    /// Called when an AI GameUser takes ownership of the attached Game Character 
    /// </summary>
    public void RemoveCameraRig()
    {
        if (cameraRig != null)
        {
            Destroy(cameraRig);
        }
    }
}
