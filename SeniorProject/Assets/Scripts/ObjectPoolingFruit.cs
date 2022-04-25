using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingFruit : MonoBehaviour
{
    public static ObjectPoolingFruit instance;
    // Array to determine if enemy in overworld spawns or not
    public bool[] canSpawn = new bool[20];
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
