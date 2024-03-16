using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance = null;
    public static bool allAI;

    private void Awake()
    {
        Debug.Log("Game Data Awake " + GetInstanceID());
        if (instance != null)
        {
            Destroy(gameObject);
            Debug.Log("Duplicate game data removed this instance!");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
