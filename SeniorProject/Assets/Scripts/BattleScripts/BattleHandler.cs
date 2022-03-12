using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{

    private static BattleHandler instance;

    public static BattleHandler GetInstance()
    {
        return instance;
    }

    // Player's party and information
    [SerializeField] private Transform pfCharacterBattle1;
    [SerializeField] private Transform pfCharacterBattle2;
    [SerializeField] private Transform pfCharacterBattle3;
    [SerializeField] private Transform pfCharacterBattle4;
    //[SerializeField] private Transform[] pfCharacterBattles;
    public Texture2D playerSpritesheet;

    private CharacterBattle playerCharacterBattle1;
    private CharacterBattle playerCharacterBattle2;
    private CharacterBattle playerCharacterBattle3;
    private CharacterBattle playerCharacterBattle4;

    // Enemy's party and information
    public Texture2D enemySpritesheet;

    [SerializeField] private Transform pfEnemyBattle1;
    [SerializeField] private Transform pfEnemyBattle2;
    [SerializeField] private Transform pfEnemyBattle3;
    [SerializeField] private Transform pfEnemyBattle4;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle enemyCharacterBattle2;
    private CharacterBattle enemyCharacterBattle3;
    private CharacterBattle enemyCharacterBattle4;

    // Determine which character in the battle should be active
    private CharacterBattle activeCharacterBattle;
    private State state;

    // Scene management
    public SceneChanger sceneChanger;
    private GameObject[] DontDestroyOnLoadObjects;
    private Unit characterStats;
    private int partyMemberTurn;
    private int enemyPartyMemberTurn;

    // UI Elements
    public Button physicalAttackButton;
    private bool physicalAttackButtonIsClicked;


    //Overworld Items
    private GameObject playerFromOverworld;

    private enum State
    {
        WaitngForPlayer,
        Busy
    }

    private void Awake()
    {
        partyMemberTurn = 1;
        enemyPartyMemberTurn = 1;
        instance = this;
    }

    private void Start()
    {
        playerCharacterBattle1 = SpawnCharacter(true);
        playerCharacterBattle2 = SpawnCharacter(true);
        playerCharacterBattle3 = SpawnCharacter(true);
        playerCharacterBattle4 = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);
        enemyCharacterBattle2 = SpawnCharacter(false);
        enemyCharacterBattle3 = SpawnCharacter(false);
        enemyCharacterBattle4 = SpawnCharacter(false);
        SetActiveCharacterBattle(playerCharacterBattle1);
        state = State.WaitngForPlayer;
        DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
        sceneChanger = DontDestroyOnLoadObjects[1].GetComponent<SceneChanger>();
        playerFromOverworld = DontDestroyOnLoadObjects[0];
        playerFromOverworld.SetActive(false);
        physicalAttackButton = GameObject.FindGameObjectWithTag("PhysicalAttackButton").GetComponent<Button>();
        physicalAttackButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        physicalAttackButtonIsClicked = true;
    }

    private void Update()
    {
        if (state == State.WaitngForPlayer)
        {
            if (physicalAttackButtonIsClicked)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (partyMemberTurn == 1 && hit.collider != null)
                    {
                        state = State.Busy;
                        physicalAttackButtonIsClicked = false;
                        playerCharacterBattle1.Attack(hit.collider.gameObject.GetComponent<CharacterBattle>(), () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (partyMemberTurn == 2 && hit.collider != null)
                    {
                        state = State.Busy;
                        physicalAttackButtonIsClicked = false;
                        playerCharacterBattle2.Attack(hit.collider.gameObject.GetComponent<CharacterBattle>(), () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (partyMemberTurn == 3 && hit.collider != null)
                    {
                        state = State.Busy;
                        physicalAttackButtonIsClicked = false;
                        playerCharacterBattle3.Attack(hit.collider.gameObject.GetComponent<CharacterBattle>(), () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (partyMemberTurn == 4 && hit.collider != null)
                    {
                        state = State.Busy;
                        physicalAttackButtonIsClicked = false;
                        playerCharacterBattle4.Attack(hit.collider.gameObject.GetComponent<CharacterBattle>(), () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    if (hit.collider != null)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                    }
                }
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

            partyMemberTurn += 1;
        }
        else if (isPlayerTeam && partyMemberTurn == 4)
        {
            position = new Vector3(-8, 0);
            characterTransform = Instantiate(pfCharacterBattle4, position, Quaternion.identity);

            partyMemberTurn = 1;
        }
        else if (enemyPartyMemberTurn == 1)
        {
            position = new Vector3(+5, 0);
            characterTransform = Instantiate(pfEnemyBattle1, position, Quaternion.identity);
            enemyPartyMemberTurn++;
        }
        else if (enemyPartyMemberTurn == 2)
        {
            position = new Vector3(+6, 1);
            characterTransform = Instantiate(pfEnemyBattle2, position, Quaternion.identity);
            enemyPartyMemberTurn++;
        }
        else if (enemyPartyMemberTurn == 3)
        {
            position = new Vector3(+7, -1);
            characterTransform = Instantiate(pfEnemyBattle3, position, Quaternion.identity);
            enemyPartyMemberTurn++;
        }
        else
        {
            position = new Vector3(+8, 0);
            characterTransform = Instantiate(pfEnemyBattle4, position, Quaternion.identity);
            enemyPartyMemberTurn = 1;
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
            ObjectPooling.instance.canSpawn[StartBattle.instance.enemyIndexInPool] = false;
            playerFromOverworld.SetActive(true);
            sceneChanger.PreviousScene();
            return;
        }
        if (activeCharacterBattle == playerCharacterBattle1 && partyMemberTurn == 1)
        {
            SetActiveCharacterBattle(playerCharacterBattle2);

            partyMemberTurn += 1;

            state = State.WaitngForPlayer;
        }
        else if (activeCharacterBattle == playerCharacterBattle2 && partyMemberTurn == 2)
        {
            SetActiveCharacterBattle(playerCharacterBattle3);

            partyMemberTurn += 1;

            state = State.WaitngForPlayer;
        }
        else if (activeCharacterBattle == playerCharacterBattle3 && partyMemberTurn == 3)
        {
            SetActiveCharacterBattle(playerCharacterBattle4);
            partyMemberTurn += 1;

            state = State.WaitngForPlayer;
        }
        else if (activeCharacterBattle == playerCharacterBattle4 && partyMemberTurn == 4)
        {
            SetActiveCharacterBattle(enemyCharacterBattle);
            partyMemberTurn = 1;
            state = State.Busy;

            if (AIDetermineAttack(enemyCharacterBattle.partyMembersDamage) == 0 && !enemyCharacterBattle.IsDead())
            {
                enemyCharacterBattle.Attack(playerCharacterBattle1, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle.partyMembersDamage) == 1 && !enemyCharacterBattle.IsDead())
            {
                enemyCharacterBattle.Attack(playerCharacterBattle2, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle.partyMembersDamage) == 2 && !enemyCharacterBattle.IsDead())
            {
                enemyCharacterBattle.Attack(playerCharacterBattle3, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle.partyMembersDamage) == 3 && !enemyCharacterBattle.IsDead())
            {
                enemyCharacterBattle.Attack(playerCharacterBattle4, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else
            {
                ChooseNextActiveCharacter();
            }
        }
        else if (activeCharacterBattle == enemyCharacterBattle && enemyPartyMemberTurn == 1)
        {
            SetActiveCharacterBattle(enemyCharacterBattle2);
            enemyPartyMemberTurn++;
            state = State.Busy;

            if (AIDetermineAttack(enemyCharacterBattle2.partyMembersDamage) == 0 && !enemyCharacterBattle2.IsDead())
            {
                enemyCharacterBattle2.Attack(playerCharacterBattle1, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle2.partyMembersDamage) == 1 && !enemyCharacterBattle2.IsDead())
            {
                enemyCharacterBattle2.Attack(playerCharacterBattle2, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle2.partyMembersDamage) == 2 && !enemyCharacterBattle2.IsDead())
            {
                enemyCharacterBattle2.Attack(playerCharacterBattle3, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle2.partyMembersDamage) == 3 && !enemyCharacterBattle2.IsDead())
            {
                enemyCharacterBattle2.Attack(playerCharacterBattle4, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else
            {
                ChooseNextActiveCharacter();
            }
        }
        else if (activeCharacterBattle == enemyCharacterBattle2 && enemyPartyMemberTurn == 2)
        {
            SetActiveCharacterBattle(enemyCharacterBattle3);
            enemyPartyMemberTurn++;
            state = State.Busy;

            if (AIDetermineAttack(enemyCharacterBattle3.partyMembersDamage) == 0 && !enemyCharacterBattle3.IsDead())
            {
                enemyCharacterBattle3.Attack(playerCharacterBattle1, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle3.partyMembersDamage) == 1 && !enemyCharacterBattle3.IsDead())
            {
                enemyCharacterBattle3.Attack(playerCharacterBattle2, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle3.partyMembersDamage) == 2 && !enemyCharacterBattle3.IsDead())
            {
                enemyCharacterBattle3.Attack(playerCharacterBattle3, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle3.partyMembersDamage) == 3 && !enemyCharacterBattle3.IsDead())
            {
                enemyCharacterBattle3.Attack(playerCharacterBattle4, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else
            {
                ChooseNextActiveCharacter();
            }
        }
        else if (activeCharacterBattle == enemyCharacterBattle3 && enemyPartyMemberTurn == 3)
        {
            SetActiveCharacterBattle(enemyCharacterBattle4);
            enemyPartyMemberTurn = 1;
            state = State.Busy;

            if (AIDetermineAttack(enemyCharacterBattle4.partyMembersDamage) == 0 && !enemyCharacterBattle4.IsDead())
            {
                enemyCharacterBattle4.Attack(playerCharacterBattle1, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle4.partyMembersDamage) == 1 && !enemyCharacterBattle4.IsDead())
            {
                enemyCharacterBattle4.Attack(playerCharacterBattle2, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle4.partyMembersDamage) == 2 && !enemyCharacterBattle4.IsDead())
            {
                enemyCharacterBattle4.Attack(playerCharacterBattle3, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else if (AIDetermineAttack(enemyCharacterBattle4.partyMembersDamage) == 3 && !enemyCharacterBattle4.IsDead())
            {
                enemyCharacterBattle4.Attack(playerCharacterBattle4, () =>
                {
                    ChooseNextActiveCharacter();
                });
            }
            else
            {
                ChooseNextActiveCharacter();
            }
        }
        else
        {
            SetActiveCharacterBattle(playerCharacterBattle1);
            state = State.WaitngForPlayer;
        }
    }

    private bool TestBattleOver()
    {
        if (playerCharacterBattle1.IsDead() && playerCharacterBattle2.IsDead() && playerCharacterBattle3.IsDead() && playerCharacterBattle4.IsDead())
        {
            //Player dead, enemy wins
            Debug.Log("Enemy wins");
            return true;
        }
        if (enemyCharacterBattle.IsDead() && enemyCharacterBattle2.IsDead() && enemyCharacterBattle3.IsDead() && enemyCharacterBattle4.IsDead())
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

    private int AIDetermineAttack(int[] partyMemberDamage)
    {
        int index = 0;
        var max = partyMemberDamage[0];
        for (int i = 1; i < partyMemberDamage.Length; i++)
        {
            if (partyMemberDamage[i] > max)
            {
                max = partyMemberDamage[i];
                index = i;
            }
        }
        Debug.Log("The party member that dealt the most damage was" + index);
        return index;

    }
}
