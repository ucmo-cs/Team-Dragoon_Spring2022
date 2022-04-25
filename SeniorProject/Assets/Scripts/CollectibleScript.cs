using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public bool isInTree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isInTree)
            {
                if (CharacterOverworldController.instance.isClimbing)
                {
                    CollectItem();
                }
            }
            else
            {
                CollectItem();
            }
        }
    }

    private void CollectItem()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.collectItem);
        CharacterOverworldController.instance.storyProgress++;
        Debug.Log(CharacterOverworldController.instance.storyProgress);
        this.gameObject.SetActive(false);
    }
}
