using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private BattleHandler battleHandler;

    public GameObject playerEncounterPrefab1;
    public GameObject playerEncounterPrefab2;
    public GameObject playerEncounterPrefab3;
    public GameObject playerEncounterPrefab4;

    // Enemies to spawn in the battle
    public GameObject enemyEncounterPrefab1;
    public GameObject enemyEncounterPrefab2;
    public GameObject enemyEncounterPrefab3;
    public GameObject enemyEncounterPrefab4;
    public int ObjectPoolIndex;


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
    public void SetIndex(int index)
    {
        ObjectPoolIndex = index;
    }
}
