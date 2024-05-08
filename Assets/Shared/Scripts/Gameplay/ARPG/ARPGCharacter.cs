using Gameplay.ARPG;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ARPGCharacter : MonoBehaviour, IGameCharacter
{
    private PlayerInput playerInput;

    [SerializeField]private GameUser owner;

    #region Lazy Initializers

    private PlayerInput PlayerInput
    {
        get
        {
            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();
            }

            return playerInput;
        }
    }

    #endregion

    public GameCharacterVirtualCameraRig CameraRig => GetComponent<GameCharacterVirtualCameraRig>();
    public GameUser Owner => owner;

    public bool SetOwnership(GameUser user)
    {
        if (user == null) return false;
        owner = user;
        PlayerInput.enabled = true;
        CameraRig.SpawnCameraRig();
        return true;
    }

    public bool RemoveOwnership()
    {
        if (owner == null) return false;

        owner = null;
        PlayerInput.enabled = false;
        return true;
    }
}
