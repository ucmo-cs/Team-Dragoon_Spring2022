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

    // Determine if the character should spawn or not
    private bool spawning = false;
    // Variables to load the battle scene
    public string sceneName;
    private SceneChanger sceneChanger;
    public GameObject sceneManager;

    public int enemyIndexInPool;

    public static StartBattle instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        gameObject.SetActive(ObjectPooling.instance.canSpawn[enemyIndexInPool]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            if (this.spawning)
            {
                Instantiate(enemyEncounterPrefab1);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.spawning = true;
            SceneChanger.instance.LoadScene(sceneName);
            gameObject.SetActive(false);
        }
    }
}
