using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameCharacter
{
    GameCharacterVirtualCameraRig CameraRig { get; }
    
    public GameUser Owner { get; }
    
    public bool SetOwnership(GameUser user);

    public bool RemoveOwnership();
}
