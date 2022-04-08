using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator anim;
    public int currentSentenceNum;
    private Dialogue myDialogue;
    public Button nextButton;
    public GameObject attachedObject;
    public float dialogueSpeed, storePCSpeed;
    public bool isDialogueShown, isObjectDeactivateAfter;

    public string[] sentences;
    // Start is called before the first frame update
    void Start()
    {
        currentSentenceNum = 0;
        dialogueSpeed = 0.03f;
        isDialogueShown = false;
        isObjectDeactivateAfter = false;
    }

    void Update()
    {
        if (isDialogueShown)
        {
            SpaceToContinue();
        }
    } 

    public void StartDialogue(Dialogue dialogue, bool isDeactivate, bool onlyShowOneSentence, int numToShow)
    {
        CharacterOverworldController.instance.canMove = false;
        CharacterOverworldController.instance.anim.SetBool("isIdle", true);
        CharacterOverworldController.instance.anim.SetBool("isMoveSide", false);
        CharacterOverworldController.instance.anim.SetFloat("PlayerSpeedHor", 0);
        CharacterOverworldController.instance.anim.SetFloat("PlayerSpeedVert", 0);
        myDialogue = dialogue;
        isObjectDeactivateAfter = isDeactivate;
        anim.SetBool("isOpen", true);
        isDialogueShown = true;

        if (nameText != null)
        {
            nameText.text = dialogue.name;
        }

        if (onlyShowOneSentence)
        {
            ShowOnlySentence(numToShow);
        }
        else
        {
            ContinueDialogue();
        }
    }

    public void ContinueDialogue()
    {
        Debug.Log("Continue Dialogue func");
        if (currentSentenceNum == sentences.Length + 1)
        {
            EndDialogue();
        }
        else
        {
            StopAllCoroutines();
            nextButton.enabled = false;
            StartCoroutine(TypeSentence(myDialogue.sentences[currentSentenceNum]));
            currentSentenceNum++;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        char[] arr = sentence.ToCharArray();
        Invoke("EnableNext", 2f);
        for (int i = 0; i<arr.Length; i++)
        {
            dialogueText.text += arr[i];
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }

    void SpaceToContinue()
    {
        if (isDialogueShown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (nextButton.enabled)
                {
                    Debug.Log("Invoking next button");
                    nextButton.onClick.Invoke();
                }
            }
        }
    }

    public void EnableNext()
    {
        nextButton.enabled = true;
        Debug.Log("Enabled Next button");
    }

    public void EndDialogue()
    {
        nextButton.enabled = false;
        Debug.Log("Ending Dialogue");
        anim.SetBool("isOpen", false);
        CharacterOverworldController.instance.canMove = true;
        if (isObjectDeactivateAfter)
        {
            attachedObject.GetComponent<InteractionScript>().DeactivateObject();
        }
        Start();
    }

    public void ShowOnlySentence(int num)
    {
        StopAllCoroutines();
        nextButton.enabled = false;
        StartCoroutine(TypeSentence(myDialogue.sentences[num]));
    }
}
