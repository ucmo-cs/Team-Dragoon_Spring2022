using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{
    public SaveData activeSave;
    public static SaveManager instance;

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

    // Update is called once per frame
    void Update()
    {
        
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
}

[System.Serializable]
public class SaveData
{
    public int saveNumber;

    public Vector3 playerPosition;

    //need scene visited list as well, to not break the system
    public string sceneName;

    //pass volume slider from player prefs after main menu
    public float volumeSlider;

    //arrays for inventory and enemies not fought that are needed to be reloaded
}