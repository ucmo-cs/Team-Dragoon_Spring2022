using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel, optionsMenuPanel;
    public Slider BGMSlider, FXSlider;
    public Image note, textbox;

    public bool isPaused;


    private void Awake()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BG Music");
        FXSlider.value = PlayerPrefs.GetFloat("FX Levels");
    }

    void Update()
    {
        
        //Escape pressed and not paused
        if (Input.GetKeyDown(KeyCode.Escape) && !(pauseMenuPanel.activeInHierarchy) && !isPaused)
        {
            pauseMenuPanel.SetActive(true);
            isPaused = true;

            Time.timeScale = 0;
        }
        //if BGM slider value is different than the actual levels
        else if (BGMSlider.value != PlayerPrefs.GetFloat("BG Music") || FXSlider.value != PlayerPrefs.GetFloat("FX Levels"))
        {
            PlayerPrefs.SetFloat("BG Music", BGMSlider.value);
            PlayerPrefs.SetFloat("FX Levels", FXSlider.value);

            if (FXSlider.value != AudioManager.instance.fireball.volume || FXSlider.value != AudioManager.instance.arrow.volume || FXSlider.value != AudioManager.instance.star.volume || FXSlider.value != AudioManager.instance.slap.volume)
            {
                AudioManager.instance.fireball.volume = PlayerPrefs.GetFloat("FX Levels");
                AudioManager.instance.star.volume = PlayerPrefs.GetFloat("FX Levels");
                AudioManager.instance.arrow.volume = PlayerPrefs.GetFloat("FX Levels");
                AudioManager.instance.slap.volume = PlayerPrefs.GetFloat("FX Levels");
            }

            //checks music sources and adjusts according to the slider value
            if ((BGMSlider.value != AudioManager.instance.mainBGM.volume || BGMSlider.value != AudioManager.instance.battleMusic.volume) && AudioManager.instance != null)
            {
                AudioManager.instance.mainBGM.volume = PlayerPrefs.GetFloat("BG Music");
                AudioManager.instance.battleMusic.volume = PlayerPrefs.GetFloat("BG Music");
            }
            
        }
        //if game is paused already and Escape is pressed
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            //if you're in the options submenu when Escape is pressed
            if (optionsMenuPanel.activeInHierarchy)
            {
                optionsMenuPanel.SetActive(false);
            }
            setPauseFalse();
        }
    }

    public void setPauseFalse()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Destroy(SaveManager.instance);
        Destroy(GameObject.Find("SaveManager"));

        Destroy(GameObject.Find("PlayerPartyPool"));

        Destroy(SceneChanger.instance);
        Destroy(GameObject.Find("SceneManager"));
        
        Destroy(AudioManager.instance);
        Destroy(GameObject.Find("AudioManager"));

        Destroy(ObjectPooling.instance);
        Destroy(GameObject.Find("OverworldObjectPool"));

        Destroy(CharacterOverworldController.instance);
        Destroy(GameObject.Find("PlayerCharacter"));

        Destroy(GameObject.Find("Spawner"));

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        //player position in scene
        SaveManager.instance.activeSave.playerPosition.x = CharacterOverworldController.instance.gameObject.transform.position.x;
        SaveManager.instance.activeSave.playerPosition.y = CharacterOverworldController.instance.gameObject.transform.position.y;
        SaveManager.instance.activeSave.playerPosition.z = CharacterOverworldController.instance.gameObject.transform.position.z;

        //get player story value
        SaveManager.instance.activeSave.storyProgress = CharacterOverworldController.instance.storyProgress;

        //get overworld object pool
        SaveManager.instance.activeSave.overworldObjectPool = ObjectPooling.instance.canSpawn;

        //get active scene
        SaveManager.instance.activeSave.sceneName = SceneManager.GetActiveScene().name;

        //get volume levels
        SaveManager.instance.activeSave.BGMSoundLevel = PlayerPrefs.GetFloat("BG Music");

        //get scene history
        SaveManager.instance.activeSave.sceneList = SceneChanger.instance.sceneHistory;

        Debug.Log("game values saved");
        SaveManager.instance.Save(SaveManager.instance.activeSave.saveNumber);
    }
}
