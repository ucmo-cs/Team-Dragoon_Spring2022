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

    private GameObject[] DontDestroyOnLoadObjects;

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoadObjects = GetDontDestroyOnLoadObjects();
        sceneChanger = DontDestroyOnLoadObjects[1].GetComponent<SceneChanger>();
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
            gameObject.SetActive(false);
            sceneChanger.LoadScene(sceneName);
            //SceneManager.LoadScene(sceneName);
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
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }
}
