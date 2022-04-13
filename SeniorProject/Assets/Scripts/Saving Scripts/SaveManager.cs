using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public static SaveManager instance;

    public GameObject playerCharacter, playerPartyPool, sceneManager, audioManager, overworldObjectPool, spawner, camera;
    [SerializeField] GameObject partyMember1, partyMember2, partyMember3, partyMember4;

    public bool gamePartialLoad = false;
 
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == SaveManager.instance.activeSave.sceneName && gamePartialLoad)
        {
            LoadSecondHalf();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Save(int saveNum)
    {
        //writes to same specific local path on machine
        string dataPath = Application.persistentDataPath;

        //create a serializer with the data type of the SaveData
        var serializer = new XmlSerializer(typeof(SaveData));
        //creates a stream of data from the desired path
        var stream = new FileStream(dataPath + "/Save" + saveNum + ".save", FileMode.Create);
        //serialize the data into the stream
        serializer.Serialize(stream, activeSave);
        //must close stream once done
        stream.Close();

        Debug.Log("Game Saved!");
    }

    public void Load(int saveNum)
    {
        string dataPath = Application.persistentDataPath;

        if(System.IO.File.Exists(dataPath + "/Save" + saveNum + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/Save" + saveNum + ".save", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Game Loaded!");
        }
    }

    public void DeleteSave(int saveNum)
    {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/Save" + saveNum + ".save"))
        {
            File.Delete(dataPath + "/Save" + saveNum + ".save");
        }
    }
    //returns true/flase if save file is in location
    public bool FileCheck(int saveNum)
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/Save" + saveNum + ".save"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //load everything for the scene after the scene change
    public void LoadSecondHalf()
    {
        gamePartialLoad = false;
        if (SceneManager.GetActiveScene().name == "PlayerHouse")
        {
            GameObject player = GameObject.Find("PlayerCharacter");
            player.transform.position = SaveManager.instance.activeSave.playerPosition;
        }
        else
        {
            LoadImportantObjects();
            SetupCamera();
        }

        PlayerPrefs.SetFloat("BG Music", instance.activeSave.BGMSoundLevel);
    }

    public void LoadImportantObjects()
    {
        LoadPlayerCharacter();
        LoadPlayerPartyPool();
        LoadSceneManager();
        LoadAudioManager();
        LoadOverworldObjectPool();
        LoadSpawner();
        LoadCamera();
    }

    public void LoadPlayerCharacter()
    {
        Instantiate(playerCharacter);
        playerCharacter.transform.position = SaveManager.instance.activeSave.playerPosition;
    }

    public void LoadPlayerPartyPool()
    {
        Instantiate(playerPartyPool);
    }

    public void LoadSceneManager()
    {
        Instantiate(sceneManager);
    }

    public void LoadAudioManager()
    {
        Instantiate(audioManager);
    }

    public void LoadOverworldObjectPool()
    {
        Instantiate(overworldObjectPool);
    }

    public void LoadSpawner()
    {
        Instantiate(spawner);
        spawner.GetComponent<Spawner>().playerEncounterPrefab1 = partyMember1;
    }
    public void LoadCamera()
    {
    }

    public void SetupCamera()
    {
        GameObject playerCam = GameObject.Find("Player Cam");
        GameObject mewPlayer = GameObject.FindGameObjectWithTag("Player");
        playerCam.GetComponent<CameraMovement>().tPlayer = mewPlayer;
        playerCam.GetComponent<CameraMovement>().tFollowTarget = mewPlayer.transform;
        playerCam.GetComponent<CinemachineVirtualCamera>().Follow = mewPlayer.transform;
        playerCam.GetComponent<CinemachineVirtualCamera>().LookAt = mewPlayer.transform;
    }
}

[System.Serializable]
public class SaveData
{
    public int saveNumber;

    public Vector3 playerPosition;

    //need scene visited list as well, to not break the system
    public string sceneName;

    //pass volume slider from player prefs after main menu
    public float BGMSoundLevel;

    //arrays for inventory and enemies not fought that are needed to be reloaded
}