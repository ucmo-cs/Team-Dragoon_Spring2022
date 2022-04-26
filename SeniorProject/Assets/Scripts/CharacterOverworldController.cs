using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverworldController : MonoBehaviour
{
    public static CharacterOverworldController instance;
    public float speed = 4f;
    public float previousSpeed;
    private Rigidbody2D myBody;
    public Animator anim;
    int idleFrameCt;
    public int currPlayer;
    public bool canSwitch, canInteract = true, canMove = true;
    public bool isDressed, canClimb, isClimbing, canBattle;
    public GameObject interactableObject;
    public GameObject tree;
    public int storyProgress;
    public Vector2 startPos;
    private SpriteRenderer rend;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        DontDestroyOnLoad(this);
        idleFrameCt = 0;
        currPlayer = 0;
        canSwitch = false;
        canMove = true;
        isDressed = false;
        canClimb = false;
        canBattle = true;
        storyProgress = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWalk();
        SwitchAvatar();
        Interaction();
        Climbing();
    }

    void PlayerWalk()
    {
        if (canMove)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            float playerSpeedHor = 0;
            float playerSpeedVert = 0;

            if (currPlayer == 4)
            {
                ChangeDirection(1);
                if (h > 0)
                {
                    playerSpeedHor = speed;
                    anim.SetBool("isMoveSide", true);
                }
                else if (h < 0)
                {
                    playerSpeedHor = -speed;
                    anim.SetBool("isMoveSide", true);
                }
                else
                {
                    anim.SetBool("isMoveSide", false);
                }
            }
            else
            {
                if (h > 0)
                {
                    playerSpeedHor = speed;
                    ChangeDirection(-1);
                    anim.SetBool("isMoveSide", true);
                }
                else if (h < 0)
                {
                    playerSpeedHor = -speed;
                    ChangeDirection(1);
                    anim.SetBool("isMoveSide", true);
                }
                else
                {
                    anim.SetBool("isMoveSide", false);
                }
            }

            if (v > 0)
            {
                playerSpeedVert = speed;
            }
            else if (v < 0)
            {
                playerSpeedVert = -speed;
            }

            anim.SetFloat("PlayerSpeedHor", playerSpeedHor);
            anim.SetFloat("PlayerSpeedVert", playerSpeedVert);

            if(isClimbing)
            {
                anim.SetFloat("PlayerSpeedHor", 0);
                anim.SetFloat("PlayerSpeedVert", 1);
                anim.SetBool("isMoveSide", false);
            }

            if (playerSpeedHor == 0 && playerSpeedVert == 0)
            {
                idleFrameCt++;
                if (idleFrameCt > 10)
                {
                    anim.SetBool("isIdle", true);
                    if (isDressed)
                    {
                        canSwitch = true;
                    }
                    idleFrameCt = 11;
                }
            }
            else
            {
                anim.SetBool("isIdle", false);
                canSwitch = false;
                idleFrameCt = 0;
            }
            myBody.velocity = new Vector2(playerSpeedHor, playerSpeedVert);
        }
        else
        {
            myBody.velocity = new Vector2(0, 0);
        }
    }

    void ChangeDirection(int direction)
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = direction;
        transform.localScale = tempScale;
    }

    void SwitchAvatar()
    {
        if (canSwitch)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //anim.SetInteger("CharSelect", 1);
                //currPlayer = 1;
                SceneChangeCharUpdate(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //anim.SetInteger("CharSelect", 2);
                //currPlayer = 2;
                SceneChangeCharUpdate(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //anim.SetInteger("CharSelect", 3);
                //currPlayer = 3;
                SceneChangeCharUpdate(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //anim.SetInteger("CharSelect", 4);
                //currPlayer = 4;
                SceneChangeCharUpdate(4);
            }
        }
    }

    void Interaction()
    {
        if (canInteract)
        {
            if (canMove)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Attempting Interaction");
                    switch (interactableObject.tag)
                    {
                        case ("ArmorStand"):
                            anim.SetBool("isDressed", true);
                            currPlayer = 1;
                            canSwitch = true;
                            isDressed = true;
                            storyProgress += 2;
                            interactableObject.GetComponent<InteractionScript>().DeactivateObject();
                            break;
                        case ("Table"):
                            storyProgress += 1;
                            interactableObject.GetComponent<DialogueTrigger>().TriggerDialogue(true, false, 0);
                            //interactableObject.GetComponent<InteractionScript>().DeactivateObject();
                            break;
                    }
                }
            }
        }
    }

    public void SceneChangeCharUpdate(int charNum)
    {
        Debug.Log("Set model to " + charNum);
        anim.SetInteger("CharSelect", charNum);
        currPlayer = charNum;
        anim.SetBool("isDressed", true);
        canSwitch = true;
        isDressed = true;
    }

    public void FreezeChar(float startSpeed, float duration)
    {
        Debug.Log("Freezing Character");
        previousSpeed = startSpeed;
        speed = 0f;
        canMove = false;
        Invoke("UnfreezeChar", duration);
    }

    public void UnfreezeChar()
    {
        Debug.Log("Unfreezing Character");
        speed = previousSpeed;
        canMove = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKey(KeyCode.E) && collision.gameObject.tag == "Breakable Rock" && currPlayer == 4)
        {
            Debug.Log("Destroy the rock");
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Tree")
        {
            canClimb = true;
            tree = collision.gameObject;
        }
    }

    private void Climbing()
    {
        if (canClimb && currPlayer == 3)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                startPos = gameObject.transform.position;
                isClimbing = true;
                Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), tree.GetComponent<Collider2D>());
                rend.sortingOrder = 2;
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                gameObject.transform.position = new Vector2(startPos.x, startPos.y - 0.3f);
                canClimb = false;
                isClimbing = false;
                Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), tree.GetComponent<Collider2D>(), false);
                rend.sortingOrder = 1;
            }
        }
    }

    public void ReactivateBattleAbility()
    {
        canBattle = true;
    }

    public void ReactivateBattle2s()
    {
        Invoke("ReactivateBattleAbility", 2f);
    }
}