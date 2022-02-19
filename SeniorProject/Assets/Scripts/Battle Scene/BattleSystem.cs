using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject[] playerPrefabs = new GameObject[3];
    public GameObject[] enemyPrefabs = new GameObject[3];

    public Transform[] playerLocations = new Transform[3];
    public Transform[] enemyLocations = new Transform[3];

    public BattleHUD[] playerHUDs = new BattleHUD[3];
    public BattleHUD[] enemyHUDs = new BattleHUD[3];

    public Unit[] playerUnits = new Unit[3];
    public Unit[] enemyUnits = new Unit[3];

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
    }

    void SetUpBattle()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject playerGO = Instantiate(playerPrefabs[i], playerLocations[i]);
            playerUnits[i] = playerGO.GetComponent<Unit>();
            Debug.Log("Spawn in " + playerUnits[i].name);
            GameObject enemyGO = Instantiate(enemyPrefabs[i], enemyLocations[i]);
            enemyUnits[i] = enemyGO.GetComponent<Unit>();
            Debug.Log("Spawn in " + enemyUnits[i].name);

            playerHUDs[i].SetHUD(playerUnits[i]);
            enemyHUDs[i].SetHUD(enemyUnits[i]);
        }
    }


}
