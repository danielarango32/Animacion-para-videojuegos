using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameUserCameraManager))]
public class GameUser : MonoBehaviour
{
    [SerializeField] private bool registerOnAwake;
    [SerializeField] private bool spawnCharacterOnRegister;
    protected IGameCharacter ownedCharacter;
    
    private GameUserCameraManager cameraManager;

    public GameUserCameraManager CameraManager
    {
        get
        {
            if (cameraManager == null)
            {
                cameraManager = GetComponent<GameUserCameraManager>();
            }

            return cameraManager;
        }
    }
    public virtual void SetOwnedCharacter(IGameCharacter owned)
    {
        ownedCharacter = owned;
    }

    private void Awake()
    {
        if (registerOnAwake)
        {
            GameState.Instance.RegisterUser(this, spawnCharacterOnRegister);
        }
    }
}
