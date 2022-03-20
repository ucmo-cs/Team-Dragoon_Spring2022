using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private BattleHandler battleHandler;


    // Enemies to spawn in the battle
    [SerializeField] public GameObject enemyEncounterPrefab1;
    [SerializeField] public GameObject enemyEncounterPrefab2;
    [SerializeField] public GameObject enemyEncounterPrefab3;
    [SerializeField] public GameObject enemyEncounterPrefab4;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSpawns(GameObject enemy1, GameObject enemy2, GameObject enemy3, GameObject enemy4)
    {
        enemyEncounterPrefab1 = enemy1;
        enemyEncounterPrefab2 = enemy2;
        enemyEncounterPrefab3 = enemy3;
        enemyEncounterPrefab4 = enemy4;
    }
}
