using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    Vector3 Position { get; }
    
    void RegisterAvailability(IPicker picker)
    {
        picker.RegisterPickable(this);
    }
}
