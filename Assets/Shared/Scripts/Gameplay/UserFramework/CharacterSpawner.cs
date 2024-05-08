using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private bool spawnOnAwake;

    private bool ValidateCharacterPrefab()
    {
        if (characterPrefab == null)
        {
            return false;
        }
        if (characterPrefab.TryGetComponent(out IGameCharacter character))
        {
            return true;
        }

        return false;
    }

    public void SpawnCharacter(GameUser owner)
    {
        if (ValidateCharacterPrefab())
        {
            Debug.Log(characterPrefab);
            GameObject newCharacterGo = Instantiate(characterPrefab, transform.position, transform.rotation);
            IGameCharacter newCharacter = newCharacterGo.GetComponent<IGameCharacter>(); 
            if (newCharacter.SetOwnership(owner))
            {
                owner.SetOwnedCharacter(newCharacter);
            }
            Debug.Log(newCharacter);
            newCharacterGo.SendMessage("OnOwned");
        }
    }
    
    private void Awake()
    {
        if (spawnOnAwake)
        {
            if (GameState.Instance.User != null)
            {
                SpawnCharacter(GameState.Instance.User);
            }
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Mesh m = Resources.GetBuiltinResource<Mesh>("Capsule.fbx");
        Handles.DrawWireDisc(transform.position, transform.up, 0.5f);
        Handles.DrawWireDisc(transform.position + transform.up * 2f, transform.up, 0.5f);
        Handles.DrawWireDisc(transform.position + transform.up * 1f, transform.up, 0.5f);
        Handles.DrawLine(transform.position + transform.right * 0.5f, transform.position + transform.up * 2f + transform.right * 0.5f);
        Handles.DrawLine(transform.position - transform.right * 0.5f, transform.position + transform.up * 2f - transform.right * 0.5f);
        Handles.DrawLine(transform.position - transform.forward * 0.5f, transform.position + transform.up * 2f - transform.forward * 0.5f);
        Handles.DrawLine(transform.position + transform.forward * 0.5f, transform.position + transform.up * 2f + transform.forward * 0.5f);
    }
#endif
}
