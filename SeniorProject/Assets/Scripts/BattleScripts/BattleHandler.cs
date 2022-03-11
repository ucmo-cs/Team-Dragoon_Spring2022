using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{

    private static BattleHandler instance;

    public static BattleHandler GetInstance()
    {
        return instance;
    }

    [SerializeField] private Transform pfCharacterBattle1;
    [SerializeField] private Transform pfCharacterBattle2;
    [SerializeField] private Transform pfCharacterBattle3;
    [SerializeField] private Transform[] pfCharacterBattles;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    private CharacterBattle playerCharacterBattle1;
    private CharacterBattle playerCharacterBattle2;
    private CharacterBattle playerCharacterBattle3;
    private CharacterBattle enemyCharacterBattle;

    //public CharacterBattle[] playerCharacterBattles;
    private CharacterBattle activeCharacterBattle;
    private State state;
    public SceneChanger sceneChanger;
    private GameObject[] DontDestroyOnLoadObjects;
    private Unit characterStats;
    private int partyMemberTurn;


    //Overworld Items
    private GameObject playerFromOverworld;
    private GameObject enemyFromOverworld;

    private enum State
    {
        WaitngForPlayer,
        Busy
    }


    private void Awake()
    {
        partyMemberTurn = 1;
        instance = this;
    }

    private void Start()
    {
        /*for (int i = 0; i < playerCharacterBattles.Length; i++)
        {
            playerCharacterBattles[i] = SpawnCharacter(true, i); 
        }*/
        playerCharacterBattle1 = SpawnCharacter(true);
        playerCharacterBattle2 = SpawnCharacter(true);
        playerCharacterBattle3 = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);
        SetActiveCharacterBattle(playerCharacterBattle1);
        state = State.WaitngForPlayer;
        DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
        sceneChanger = DontDestroyOnLoadObjects[1].GetComponent<SceneChanger>();
        playerFromOverworld = DontDestroyOnLoadObjects[0];
        playerFromOverworld.SetActive(false);
        enemyFromOverworld = DontDestroyOnLoadObjects[2];
        enemyFromOverworld.SetActive(false);
    }

    private void Update()
    {
        if (state == State.WaitngForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space) && partyMemberTurn == 1)
            {
                state = State.Busy;
                playerCharacterBattle1.Attack(enemyCharacterBattle, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (Input.GetKeyDown(KeyCode.Space) && partyMemberTurn == 2)
            {
                state = State.Busy;
                playerCharacterBattle2.Attack(enemyCharacterBattle, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (Input.GetKeyDown(KeyCode.Space) && partyMemberTurn == 3)
            {
                state = State.Busy;
                playerCharacterBattle3.Attack(enemyCharacterBattle, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
        }
    }

    private CharacterBattle SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        Transform characterTransform;
        if (isPlayerTeam && partyMemberTurn == 1)
        {
            position = new Vector3(-5, 0);
            characterTransform = Instantiate(pfCharacterBattle1, position, Quaternion.identity);
            partyMemberTurn += 1;
        }
        else if(isPlayerTeam && partyMemberTurn == 2)
        {
            position = new Vector3(-6, 1);
            characterTransform = Instantiate(pfCharacterBattle2, position, Quaternion.identity);
            partyMemberTurn += 1;
        }
        else if(isPlayerTeam && partyMemberTurn == 3)
        {
            position = new Vector3(-7, -1);
            characterTransform = Instantiate(pfCharacterBattle3, position, Quaternion.identity);

            partyMemberTurn = 1;
        }
        else
        {
            position = new Vector3(+5, 0);
            characterTransform = Instantiate(pfCharacterBattle1, position, Quaternion.identity);
        }

        //Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam);

        return characterBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle)
    {
        if (activeCharacterBattle != null)
        {
            activeCharacterBattle.HideSelectionCircle();
        }
        
        activeCharacterBattle = characterBattle;
        activeCharacterBattle.showSelectionCircle();
    }

    private void ChooseNextActiveCharacter()
    {
        if (TestBattleOver())
        {
            playerFromOverworld.SetActive(true);
            sceneChanger.PreviousScene();
            return;
        }
        if (activeCharacterBattle == playerCharacterBattle1 && partyMemberTurn == 1)
        {
            SetActiveCharacterBattle(playerCharacterBattle2);

            partyMemberTurn += 1;

            state = State.WaitngForPlayer;
            //state = State.Busy; 

            /*enemyCharacterBattle.Attack(playerCharacterBattle1, () =>
            {
                ChooseNextActiveCharacter();
            });*/
        }
        else if (activeCharacterBattle == playerCharacterBattle2 && partyMemberTurn == 2)
        {
            SetActiveCharacterBattle(playerCharacterBattle3);

            partyMemberTurn += 1;

            state = State.WaitngForPlayer;
            //state = State.Busy;

            /*enemyCharacterBattle.Attack(playerCharacterBattle1, () =>
            {
                ChooseNextActiveCharacter();
            });*/
        }
        else if (activeCharacterBattle == playerCharacterBattle3 && partyMemberTurn == 3)
        {
            SetActiveCharacterBattle(enemyCharacterBattle);
            partyMemberTurn = 1;
            state = State.Busy;

            enemyCharacterBattle.Attack(playerCharacterBattle1, () =>
            {
                ChooseNextActiveCharacter();
            });
        }
        else
        {
            SetActiveCharacterBattle(playerCharacterBattle1);
            state = State.WaitngForPlayer;
        }
    }

    private bool TestBattleOver()
    {
        if (playerCharacterBattle1.IsDead())
        {
            //Player dead, enemy wins
            Debug.Log("Enemy wins");
            return true;
        }
        if (enemyCharacterBattle.IsDead())
        {
            //Enemy dead, player wins
            Debug.Log("Player wins");
            
            return true;
        }
        return false;
    }

    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }
}
