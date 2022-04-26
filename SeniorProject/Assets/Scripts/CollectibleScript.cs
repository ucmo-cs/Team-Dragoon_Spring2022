using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public bool isInTree;

    [SerializeField] private int poolIndex;
    private GameObject[] DontDestroyOnLoadObjects;
    private Spawner spawner;
    [SerializeField] private GameObject objectpooling;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
        objectpooling = ReturnObjectFromArray(DontDestroyOnLoadObjects, "ObjectPoolFruit");
        gameObject.SetActive(objectpooling.GetComponent<ObjectPoolingFruit>().canSpawn[poolIndex]);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Collection" && CharacterOverworldController.instance.storyProgress > 3)
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
        objectpooling.GetComponent<ObjectPoolingFruit>().canSpawn[poolIndex] = false;
    }
    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.Destroy(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                Object.Destroy(temp);
        }
    }

    public static GameObject ReturnObjectFromArray(GameObject[] array, string tag)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].gameObject.tag == tag)
            {
                return array[i];
            }
        }
        return null;
    }
}
