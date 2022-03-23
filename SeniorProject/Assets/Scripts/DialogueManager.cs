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
    public float dialogueSpeed, storePCSpeed;
    public bool isDialogueShown;

    public string[] sentences;
    // Start is called before the first frame update
    void Start()
    {
        currentSentenceNum = 0;
        dialogueSpeed = 0.03f;
        isDialogueShown = false;
    }

    void Update()
    {
        SpaceToContinue();
    } 

    public void StartDialogue(Dialogue dialogue)
    {
        storePCSpeed = CharacterOverworldController.instance.speed;
        CharacterOverworldController.instance.FreezeChar2s(storePCSpeed);
        myDialogue = dialogue;
        anim.SetBool("isOpen", true);
        isDialogueShown = true;

        if (nameText != null)
        {
            nameText.text = dialogue.name;
        }
        ContinueDialogue();
        
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
        anim.SetBool("isOpen", false);
        Start();
    }
}
