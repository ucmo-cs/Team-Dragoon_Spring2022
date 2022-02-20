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

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneChanger = sceneManager.GetComponent<SceneChanger>();
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
            sceneChanger.LoadScene(sceneName);
            //SceneManager.LoadScene(sceneName);
        }
    }
}
