using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel, optionsMenuPanel;
    public Slider slider;
    public Image note, textbox;

    public bool isPaused;


    private void Awake()
    {
        slider.value = PlayerPrefs.GetFloat("BG Music");
    }

    void Update()
    {
        
        //Escape pressed and not paused
        if (Input.GetKeyDown(KeyCode.Escape) && !(pauseMenuPanel.activeInHierarchy) && !isPaused)
        {
            pauseMenuPanel.SetActive(true);
            isPaused = true;

            //freeze player and inputs available to them
            CharacterOverworldController.instance.canMove = false;
            CharacterOverworldController.instance.canInteract = false;
            CharacterOverworldController.instance.canSwitch = false;
        }
        //if BGM slider value is different than the actual levels
        else if (slider.value != PlayerPrefs.GetFloat("BG Music"))
        {
            PlayerPrefs.SetFloat("BG Music", slider.value);
            //checks music sources and adjusts according to the slider value
            if ((slider.value != AudioManager.instance.mainBGM.volume || slider.value != AudioManager.instance.battleMusic.volume) && AudioManager.instance != null)
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
        CharacterOverworldController.instance.canMove = true;
        CharacterOverworldController.instance.canInteract = true;
        CharacterOverworldController.instance.canSwitch = true;

        pauseMenuPanel.SetActive(false);
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

        //get active scene
        SaveManager.instance.activeSave.sceneName = SceneManager.GetActiveScene().name;

        //get volume levels
        SaveManager.instance.activeSave.BGMSoundLevel = PlayerPrefs.GetFloat("BG Music");

        Debug.Log("game values saved");
        SaveManager.instance.Save(SaveManager.instance.activeSave.saveNumber);
    }
}
