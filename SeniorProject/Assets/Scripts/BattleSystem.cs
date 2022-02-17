using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public Transform playerLocation1;
    public Transform playerLocation2;
    public Transform playerLocation3;
    public Transform enemyLocation1;
    public Transform enemyLocation2;
    public Transform enemyLocation3;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
    }

    void SetUpBattle()
    {
        Instantiate(playerPrefab1, playerLocation1);
        Instantiate(playerPrefab2, playerLocation2);
        Instantiate(playerPrefab3, playerLocation3);
        Instantiate(enemyPrefab1, enemyLocation1);
        Instantiate(enemyPrefab2, enemyLocation2);
        Instantiate(enemyPrefab3, enemyLocation3);
    }


}
