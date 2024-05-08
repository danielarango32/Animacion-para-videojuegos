using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPicker
{
    List<IPickable> AvailablePickables { get; }

    void RegisterPickable(IPickable pickable)
    {
        AvailablePickables.Add(pickable);
    }
}
