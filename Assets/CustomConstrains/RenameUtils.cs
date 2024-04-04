using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameUtils : MonoBehaviour
{
    [SerializeField] private GameObject[] renameTargets;
    [SerializeField] private string suffix;
    [SerializeField] private string prefix;

    public void RenamePrefixes()
    {
        foreach (GameObject renameTarget in renameTargets)
        {
            renameTarget.name = prefix + renameTarget.name;
        }
    }

    public void RenameSuffixes()
    {
        
        foreach (GameObject renameTarget in renameTargets)
        {
            renameTarget.name += suffix;
        }
    }
}
