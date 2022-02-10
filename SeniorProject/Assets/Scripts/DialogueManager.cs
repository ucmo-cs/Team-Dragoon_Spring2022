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
    public float dialogueSpeed;

    public string[] sentences;
    // Start is called before the first frame update
    void Start()
    {
        currentSentenceNum = 0;
        dialogueSpeed = 0.03f;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        CharacterOverworldController.instance.canMove = false;
        myDialogue = dialogue;
        anim.SetBool("isOpen", true);

        if (nameText != null)
        {
            nameText.text = dialogue.name;
        }
        ContinueDialogue();
        
    }

    public void ContinueDialogue()
    {
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
        for (int i = 0; i<arr.Length; i++)
        {
            dialogueText.text += arr[i];
            yield return new WaitForSeconds(dialogueSpeed);
            if (i >= 0.5 * arr.Length)
            {
                nextButton.enabled = true;
            }
        }
    }
    public void EndDialogue()
    {
        anim.SetBool("isOpen", false);
        CharacterOverworldController.instance.canMove = true;
    }
}
