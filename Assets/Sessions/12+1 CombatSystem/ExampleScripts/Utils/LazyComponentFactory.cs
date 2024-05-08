using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class LazyComponentFactory
{
    public static TComp LazyComponent<TComp>(MonoBehaviour parent) where TComp : Component
    {
        return parent.AddComponent<TComp>();
    }
}
