using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverworldController : MonoBehaviour
{
    public static CharacterOverworldController instance;
    public float speed = 4f;
    public float previousSpeed;
    private Rigidbody2D myBody;
    private Animator anim;
    int idleFrameCt;
    public int currPlayer;
    public bool canSwitch, canInteract = true, canMove = true;
    bool isDressed;
    public GameObject interactableObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        DontDestroyOnLoad(this);
        idleFrameCt = 0;
        currPlayer = 0;
        canSwitch = false;
        canMove = true;
        isDressed = false;
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
    }

    void PlayerWalk()
    {
        if (canMove)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            float playerSpeedHor = 0;
            float playerSpeedVert = 0;

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
                anim.SetInteger("CharSelect", 1);
                currPlayer = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetInteger("CharSelect", 2);
                currPlayer = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetInteger("CharSelect", 3);
                currPlayer = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                anim.SetInteger("CharSelect", 4);
                currPlayer = 4;
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
                    switch (interactableObject.tag)
                    {
                        case ("ArmorStand"):
                            anim.SetBool("isDressed", true);
                            currPlayer = 1;
                            canSwitch = true;
                            isDressed = true;
                            interactableObject.GetComponent<InteractionScript>().DeactivateObject();
                            break;
                        case ("Table"):
                            interactableObject.GetComponent<DialogueTrigger>().TriggerDialogue();
                            interactableObject.GetComponent<InteractionScript>().DeactivateObject();
                            break;
                    }
                }
            }
        }
    }

    public void SceneChangeCharUpdate(int charNum)
    {
        anim.SetInteger("CharSelect", charNum);
        currPlayer = charNum;
    }

    public void FreezeChar2s(float startSpeed)
    {
        Debug.Log("Freezing Character");
        previousSpeed = startSpeed;
        speed = 0f;
        canMove = false;
        Invoke("UnfreezeChar", 2f);
    }

    public void UnfreezeChar()
    {
        Debug.Log("Unfreezing Character");
        speed = previousSpeed;
        canMove = true;
    }

}