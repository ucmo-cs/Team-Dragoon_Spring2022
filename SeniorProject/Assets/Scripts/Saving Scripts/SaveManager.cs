using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public static SaveManager instance;

    public bool gamePartialLoad = false;


    private void Update()
    {
        if (!(SceneManager.GetActiveScene().ToString() == "MainMenu") && gamePartialLoad)
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

        CharacterOverworldController.instance.gameObject.transform.position = instance.activeSave.playerPosition;

        PlayerPrefs.SetFloat("BG Music", instance.activeSave.BGMSoundLevel);
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