using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState instance;
    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GAME STATE");
                instance = go.AddComponent<GameState>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private List<GameUser> users = new List<GameUser>();
    public GameUser User
    {
        get
        {
            if (users.Count < 1)
            {
                return null;
            }

            return users[0];
        }
    }

    public void RegisterUser(GameUser user, bool spawn = false)
    {
        if (users.Contains(user) || user == null) return;
        users.Add(user);
        if (spawn)
        {
            CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
            if (spawner != null)
            {
                FindObjectOfType<CharacterSpawner>().SpawnCharacter(user);
            }
            
        }
    }
}
