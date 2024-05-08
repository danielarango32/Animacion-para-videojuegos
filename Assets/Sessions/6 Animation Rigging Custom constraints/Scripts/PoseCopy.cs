using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoseCopy : MonoBehaviour
{
    [Flags]
    public enum CopySettings
    {
        Position = 2,
        Rotation = 4,
        Scale = 8
    }
    
    [SerializeField] private Transform sourceRoot;

    [SerializeField] private Transform destRoot;

    [SerializeField] private CopySettings copySettings = (CopySettings)14;

    public void CopyPose()
    {
        foreach (Transform sourceChild in sourceRoot)
        {
            try
            {
                Transform destChild = destRoot.GetComponentsInChildren<Transform>()
                    .First(x => x.name == sourceChild.name);
                if (copySettings.HasFlag(CopySettings.Position))
                {
                    if (sourceChild == sourceRoot)
                        destChild.position = sourceChild.position;
                    else
                        destChild.localPosition = sourceChild.localPosition;
                }

                if (copySettings.HasFlag(CopySettings.Rotation))
                {
                    destChild.rotation = sourceChild.rotation;
                }

                if (copySettings.HasFlag(CopySettings.Scale))
                {
                    destChild.localScale = sourceChild.localScale;
                }
            }
            catch
            {
                continue;
            }
        }
    }
}
