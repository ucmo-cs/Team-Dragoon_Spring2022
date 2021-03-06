using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    // Enemies to spawn in the battle
    [SerializeField] public GameObject enemyEncounterPrefab1;
    [SerializeField] public GameObject enemyEncounterPrefab2;
    [SerializeField] public GameObject enemyEncounterPrefab3;
    [SerializeField] public GameObject enemyEncounterPrefab4;
    private GameObject[] DontDestroyOnLoadObjects;
    private Spawner spawner;

    // Determine if the character should spawn or not
    private bool spawning = false;
    // Variables to load the battle scene
    public string sceneName;
    private SceneChanger sceneChanger;
    public GameObject sceneManager;

    public int enemyIndexInPool;

    public static StartBattle instance;

    //Checking to see what is being linked
    [SerializeField] private GameObject objectpooling;

    private void Awake()
    {
        //instance = this;
    }

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        //SceneManager.sceneLoaded += OnSceneLoaded;

        /*if (ObjectPooling.instance != null)
        {
            gameObject.SetActive(ObjectPooling.instance.canSpawn[enemyIndexInPool]);
        }
        else
        {
        }*/

    }
    private void Update()
    {
        DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
        objectpooling = ReturnObjectFromArray(DontDestroyOnLoadObjects, "ObjectPool");
        gameObject.SetActive(objectpooling.GetComponent<ObjectPooling>().canSpawn[enemyIndexInPool]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            if (this.spawning)
            {
                //Instantiate(enemyEncounterPrefab1);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && CharacterOverworldController.instance.canBattle)
        {
            DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
            Debug.Log("Add to Don't Destroy on Load");
            spawner = ReturnObjectFromArray(DontDestroyOnLoadObjects, "Spawner").GetComponent<Spawner>();
            spawner.SetSpawns(enemyEncounterPrefab1, enemyEncounterPrefab2, enemyEncounterPrefab3, enemyEncounterPrefab4);
            spawner.SetIndex(enemyIndexInPool);
            Debug.Log("Set Spawns");
            this.spawning = true;
            Debug.Log("Fade Out Overworld BGM");
            AudioManager.instance.StartCoroutine(AudioManager.FadeOut(AudioManager.instance.mainBGM, 2f));
            Debug.Log("Fade in Battle Music");
            AudioManager.instance.StartCoroutine(AudioManager.FadeIn(AudioManager.instance.battleMusic, 3f));
            CharacterOverworldController.instance.canBattle = false;
            CharacterOverworldController.instance.storyProgress++;
            Debug.Log(CharacterOverworldController.instance.storyProgress);
            Debug.Log("Load new scene");
            SceneChanger.instance.LoadScene(sceneName);
        }
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
