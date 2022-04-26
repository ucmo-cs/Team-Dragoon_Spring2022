using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DojoAnimationScript : MonoBehaviour
{
    int loops = 0;
    bool isTriggered = false;
    public Animator anim;
    private DialogueTrigger dia;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dia = GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoopsDown()
    {
        loops++;
        if (loops > 2)
        {
            anim.SetBool("IsLifting", true);
        }
    }

    public void FullyLifted()
    {
        anim.SetBool("IsUp", true);
    }

    public void StartDialogue()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            //CharacterOverworldController.instance.storyProgress++;
            //CharacterOverworldController.instance.canBattle = true;
            //Both now handled in DialogueManager when switching back to overworld scene
            dia.TriggerDialogue(false, false, 0);
        }
    }
}
