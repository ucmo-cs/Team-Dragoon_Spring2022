using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
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
            AudioManager.instance.PlayClip(AudioManager.instance.collectItem);
            CharacterOverworldController.instance.storyProgress++;
            Debug.Log(CharacterOverworldController.instance.storyProgress);
            this.gameObject.SetActive(false);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }*/
}
