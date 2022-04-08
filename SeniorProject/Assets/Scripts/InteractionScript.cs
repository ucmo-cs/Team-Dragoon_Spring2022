using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    public Text toolText;
    public bool canInteract, isDeactivated, isTouchDialogueTrigger, isStoryLocked;
    public GameObject exclamation_mark, inactiveVersion;
    public DialogueTrigger dialogueTrigger;
    public int storyRequirement;

    private void Awake()
    {
        canInteract = false;
        isDeactivated = false;
        if (toolText != null)
        {
            toolText.enabled = false;
        }
        if (exclamation_mark != null)
        {
            exclamation_mark.SetActive(false);
        }
        if (inactiveVersion != null)
        {
            inactiveVersion.SetActive(false);
        }
        dialogueTrigger = this.GetComponent<DialogueTrigger>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isDeactivated)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (isTouchDialogueTrigger)
                {
                    if (isStoryLocked && CharacterOverworldController.instance.storyProgress < storyRequirement)
                    {
                        CancelInvoke();
                        int sentenceToShow = storyRequirement - CharacterOverworldController.instance.storyProgress - 1;
                        dialogueTrigger.TriggerDialogue(false, true, sentenceToShow);
                        isDeactivated = true;
                    }
                    else if (isStoryLocked && CharacterOverworldController.instance.storyProgress >= storyRequirement)
                    {
                        //Do nothing, allow DoorManager to send to new scene
                    }
                    else
                    {
                        CancelInvoke();
                        dialogueTrigger.TriggerDialogue(false, false, 0);
                        isDeactivated = true;
                    }
                }
                else
                {
                    if (toolText != null)
                    {
                        toolText.enabled = true;
                    }
                    canInteract = true;
                    CharacterOverworldController.instance.canInteract = true;
                    CharacterOverworldController.instance.interactableObject = this.gameObject;
                    exclamation_mark.SetActive(true);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isTouchDialogueTrigger)
        {
            Invoke("ReactivateObject", 3f);
        }
        else if (!isDeactivated)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (toolText != null)
                {
                    toolText.enabled = false;
                }
                canInteract = false;
                CharacterOverworldController.instance.canInteract = false;
                CharacterOverworldController.instance.interactableObject = null;
                exclamation_mark.SetActive(false);
            }
        }
    }

    public void DeactivateObject()
    {
        isDeactivated = true;
        canInteract = false;
        CharacterOverworldController.instance.canInteract = false;
        CharacterOverworldController.instance.interactableObject = null;
        exclamation_mark.SetActive(false);
        inactiveVersion.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ReactivateObject()
    {
        isDeactivated = false;
    }
}
