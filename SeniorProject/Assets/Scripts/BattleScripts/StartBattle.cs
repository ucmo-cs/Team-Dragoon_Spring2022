using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    [SerializeField] private GameObject enemyEncounterPrefab;

    private bool spawning = false;
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
                Instantiate(enemyEncounterPrefab);
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
