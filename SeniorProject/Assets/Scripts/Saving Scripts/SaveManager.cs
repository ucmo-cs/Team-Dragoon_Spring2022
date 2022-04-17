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

    public GameObject playerCharacter, playerPartyPool, sceneManager, audioManager, overworldObjectPool, spawner;
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
        Debug.Log("Loading second half. scene = " + SceneManager.GetActiveScene().name);
        gamePartialLoad = false;
        if (SceneManager.GetActiveScene().name == "PlayerHouse")
        {
            GameObject player = GameObject.FindWithTag("Player");
            LoadPlayerCharacter(player);
        }
        else
        {
            LoadImportantObjects();
            
        }

        PlayerPrefs.SetFloat("BG Music", instance.activeSave.BGMSoundLevel);
    }

    public void LoadImportantObjects()
    {
        Debug.Log("Loading important objs");
        Instantiate(playerCharacter);
        LoadPlayerCharacter(playerCharacter);

        LoadPlayerPartyPool();

        LoadSceneManager();

        LoadAudioManager();

        //called in Start() of StartBattle script
        //LoadOverworldObjectPool();

        LoadSpawner();
    }

    public void LoadPlayerCharacter(GameObject player)
    {
        //Debug.Log();
        player.transform.position = SaveManager.instance.activeSave.playerPosition;
        player.GetComponent<CharacterOverworldController>().storyProgress = SaveManager.instance.activeSave.storyProgress;
        
        if (SceneManager.GetActiveScene().name == "PlayerHouse")
        {
            GameObject roomObjects = GameObject.FindWithTag("PHRoomObjects");

            GameObject note = roomObjects.transform.GetChild(0).gameObject;
            GameObject armorStand = roomObjects.transform.GetChild(1).gameObject;
            GameObject tableEmpty = roomObjects.transform.GetChild(3).gameObject;
            GameObject armorEmpty = roomObjects.transform.GetChild(4).gameObject;

            //note collected
            if (player.GetComponent<CharacterOverworldController>().storyProgress == 1)
            {
                //load note/tables
                note.SetActive(false);
                tableEmpty.SetActive(true);
            }
            //armor collected
            else if (player.GetComponent<CharacterOverworldController>().storyProgress == 2)
            {
                //load armor stand
                armorStand.SetActive(false);
                armorEmpty.SetActive(true);

                //set player armor values
                player.GetComponent<CharacterOverworldController>().anim.SetBool("isDressed",true);
                player.GetComponent<CharacterOverworldController>().currPlayer = 1;
                player.GetComponent<CharacterOverworldController>().canSwitch = true;
                player.GetComponent<CharacterOverworldController>().isDressed = true;
            }
            //note/armor both collected
            else if (player.GetComponent<CharacterOverworldController>().storyProgress == 3)
            {
                //load note/tables and armor stands
                note.SetActive(false);
                tableEmpty.SetActive(true);
                armorStand.SetActive(false);
                armorEmpty.SetActive(true);

                //set player armor values
                player.GetComponent<CharacterOverworldController>().anim.SetBool("isDressed", true);
                player.GetComponent<CharacterOverworldController>().currPlayer = 1;
                player.GetComponent<CharacterOverworldController>().canSwitch = true;
                player.GetComponent<CharacterOverworldController>().isDressed = true;
            }
        }
        //loading any other scene but PlayerHouse
        else
        {
            Debug.Log("loading player out of house");
            
            //Animator null at this point, but it is still playing proper animations
            Debug.Log(player.GetComponent<CharacterOverworldController>().anim);
            
            //set player armor values
            player.GetComponent<CharacterOverworldController>().anim.SetBool("isDressed", true);
            player.GetComponent<CharacterOverworldController>().currPlayer = 1;
            player.GetComponent<CharacterOverworldController>().canSwitch = true;
            player.GetComponent<CharacterOverworldController>().isDressed = true;
        }
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

    public GameObject LoadOverworldObjectPool()
    {
        Instantiate(overworldObjectPool);
        return overworldObjectPool;
    }

    public void LoadSpawner()
    {
        Instantiate(spawner);
        spawner.GetComponent<Spawner>().playerEncounterPrefab1 = partyMember1;
        spawner.GetComponent<Spawner>().playerEncounterPrefab2 = partyMember2;
        spawner.GetComponent<Spawner>().playerEncounterPrefab3 = partyMember3;
        spawner.GetComponent<Spawner>().playerEncounterPrefab4 = partyMember4;
    }

    public void SetupCamera()
    {
        GameObject playerCam = GameObject.FindGameObjectWithTag("VirtualCamera");
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

    public int storyProgress;

    //need scene visited list as well, to not break the system
    public string sceneName;

    //pass volume slider from player prefs after main menu
    public float BGMSoundLevel;

    //overworld object pooling
    public bool[] overworldObjectPool;


}