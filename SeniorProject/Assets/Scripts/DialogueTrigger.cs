using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;

    public void TriggerDialogue(bool isDeactivateAfter)
    {
        dialogueManager.StartDialogue(dialogue, isDeactivateAfter);
    }
}
