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

    [SerializeField] private Transform pfCharacterBattle;
    [SerializeField] private Transform[] pfCharacterBattles;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    private CharacterBattle playerCharacterBattle;
    private CharacterBattle enemyCharacterBattle;

    public CharacterBattle[] playerCharacterBattles;
    private CharacterBattle activeCharacterBattle;
    private State state;
    public SceneChanger sceneChanger;
    private GameObject[] DontDestroyOnLoadObjects;
    private Unit characterStats;

    private enum State
    {
        WaitngForPlayer,
        Busy
    }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        /*for (int i = 0; i < playerCharacterBattles.Length; i++)
        {
            playerCharacterBattles[i] = SpawnCharacter(true, i); 
        }*/
        playerCharacterBattle = SpawnCharacter(true, 1);
        enemyCharacterBattle = SpawnCharacter(false, 1);
        SetActiveCharacterBattle(playerCharacterBattle);
        state = State.WaitngForPlayer;
        DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
        sceneChanger = DontDestroyOnLoadObjects[0].GetComponent<SceneChanger>();
    }

    private void Update()
    {
        if (state == State.WaitngForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Busy;
                playerCharacterBattle.Attack(enemyCharacterBattle, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
        }
    }

    private CharacterBattle SpawnCharacter(bool isPlayerTeam, int unitPlacement)
    {
        Vector3 position;
        Transform characterTransform;
        if (isPlayerTeam)
        {
            position = new Vector3(-5 * -(unitPlacement / 2), 0);
            characterTransform = Instantiate(pfCharacterBattles[unitPlacement], position, Quaternion.identity);
        }
        else
        {
            position = new Vector3(+5, 0);
            characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
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
            return;
        }
        if (activeCharacterBattle == playerCharacterBattle)
        {
            SetActiveCharacterBattle(enemyCharacterBattle);
            state = State.Busy; 

            enemyCharacterBattle.Attack(playerCharacterBattle, () =>
            {
                ChooseNextActiveCharacter();
            });
        }
        else
        {
            SetActiveCharacterBattle(playerCharacterBattle);
            state = State.WaitngForPlayer;
        }
    }

    private bool TestBattleOver()
    {
        if (playerCharacterBattle.IsDead())
        {
            //Player dead, enemy wins
            Debug.Log("Enemy wins");
            return true;
        }
        if (enemyCharacterBattle.IsDead())
        {
            //Enemy dead, player wins
            Debug.Log("Player wins");
            sceneChanger.PreviousScene();
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
