using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    //can change scene switching system later as it is a little
    //more complex to implement refrencing the actual scene
    //
    //this link has a solution that seems to work, will look at it later
    //https://answers.unity.com/questions/605184/scene-as-a-variable.html?page=1&pageSize=5&sort=votes

    public string sceneName;
    private Vector2 doorExitPos;
    public float exitPosX;
    public float exitPosY;
    public bool isStoryLocked;
    public int storyRequirement;

    private SceneChanger sceneChanger;
    public GameObject sceneManager;
    public GameObject playerChar;

    //add private variable to save the character that enters door


    // Start is called before the first frame update
    void Start()
    {
        sceneChanger = sceneManager.GetComponent<SceneChanger>();
        doorExitPos.x = exitPosX;
        doorExitPos.y = exitPosY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isStoryLocked && CharacterOverworldController.instance.storyProgress < storyRequirement)
            {
                //Display dialogue stating what needs to happen
                //Handled within InteractionScript
            }
            else
            {
                playerChar = collision.gameObject;
                DontDestroyOnLoad(playerChar);
                Debug.Log("Door/Player collision");
                sceneChanger.LoadScene(sceneName);
                playerChar.transform.position = doorExitPos;
            }
        }
    }
}