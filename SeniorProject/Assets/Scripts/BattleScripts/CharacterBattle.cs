using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CharacterBattle : MonoBehaviour
{
    //Animation Script goes here
    private Character_Base characterBase;
    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    private bool isPlayerTeam;
    private GameObject selectionCircleGameObject;
    private HealthSystem healthSystem;
    private Slider healthSlider;
    private Unit characterStats;

    // Damage values from player party. For enemies determining who to attack
    [SerializeField] private int partyMemberIndex;
    public int[] partyMembersDamage = new int[4];
    private enum State
    {
        Idle,
        Sliding,
        Busy,
    }

    private void Awake()
    {
        //Animation script goes here
        characterBase = GetComponent<Character_Base>();
        selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
        healthSlider = transform.Find("HealthBar").gameObject.GetComponent<Slider>();
        HideSelectionCircle();
        state = State.Idle;
        characterStats = GetComponent<Unit>();
    }

    public void Setup(bool isPlayerTeam)
    {
        this.isPlayerTeam = isPlayerTeam;
        if (isPlayerTeam)
        {
            /* Setting Animations and Sprites
            characterBase.SetAnimsSwordTwoHandedBack();
            characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance.playerSpritesheet;
            */
        }
        else
        {
            /* Setting Animations and Sprites
            characterBase.setAnimsSwordShield();
            characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance.enemySpritesheet;
            */
        }

        healthSystem = new HealthSystem(characterStats.maxHP);

        PlayAnimIdle();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Sliding:
                float slideSpeed = 10f;
                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

                float reachedDistance = 1f;
                if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
                {
                    // Arriced to slide target position
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;
        }

    }

    private void PlayAnimIdle()
    {
        /*if (isPlayerTeam)
        {
            characterBase.PlayAnimIdle(new Vector3(+1, 0));
        }
        else
            characterBase.PlayAnimIdle(new Vecotr3(-1, 0));*/
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
        Debug.Log("Hit " + healthSystem.GetHealthAmount());
        healthSlider.value = healthSystem.GetHealthPercent();

        if (healthSystem.IsDead())
        {
            //Destroy(gameObject);
            // Character has died
            //characterBase.PlayAnimLyingUp();
        }
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
    public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
    {
        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized;
        Vector3 startingPosition = GetPosition();
        
        SlideToPosition(slideTargetPosition, () =>
        {
            //Instert animation here;
            /*
            Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;

            characterBase.PlayAnimAttack(attackDir, null, () => {
             *      characterBase.PlayAnimIdle(attackDir);
             *      onAttackComplete();
             * }); */
            state = State.Busy;

            if (isPlayerTeam)
            {
                if (partyMemberIndex == 1)
                {
                    targetCharacterBattle.partyMembersDamage[0] += characterStats.damage;
                }
                else if (partyMemberIndex == 2)
                {
                    targetCharacterBattle.partyMembersDamage[1] += characterStats.damage;
                }
                else if (partyMemberIndex == 3)
                {
                    targetCharacterBattle.partyMembersDamage[2] += characterStats.damage;
                }
                else if (partyMemberIndex == 4)
                {
                    targetCharacterBattle.partyMembersDamage[3] += characterStats.damage;
                }
            }

            targetCharacterBattle.Damage(characterStats.damage);
            SlideToPosition(startingPosition, () =>
            {
                state = State.Idle;
                onAttackComplete();
            });
        });


        
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;

        // More animation code to be added
        /*if (slideTargetPosition.x > 0)
        {
            characterBase.PlayAnimSlideRight();
        } else
        {
            characterBase.PlayAnimSlideLeft();
        }*/

    }

    public void HideSelectionCircle()
    {
        selectionCircleGameObject.SetActive(false);
    }

    public void showSelectionCircle()
    {
        selectionCircleGameObject.SetActive(true);
    }
}
