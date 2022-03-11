using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    public static ObjectPooling instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    // Array to determine if enemy in overworld spawns or not
    public bool[] canSpawn = new bool[20];

    private void Start()
    {
        for (int i = 0; i < canSpawn.Length; i++)
        {
            canSpawn[i] = true;
        }
    }
}
