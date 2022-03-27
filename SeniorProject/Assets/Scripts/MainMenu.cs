using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public TMPro.TextMeshProUGUI pressAnyText;
    public GameObject MainMenuPanel;
    
    //used for scripts check/load saves
    public SaveManager saveManager;

    //used for to check which file to load
    public GameObject EmptyTxt1, EmptyTxt2, EmptyTxt3;

    void Update()
    {
        if (pressAnyText == null || MainMenuPanel == null || volumeSlider == null)
        {
            //this debug will fire on the PlayGame()/QuitGame() scripts are clicked as well
            //Debug.Log("Parameter missing in MainMenu Script");
        }
        else
        {
            if (Input.anyKeyDown && pressAnyText.enabled == true)
            {
                pressAnyText.enabled = false;
                MainMenuPanel.SetActive(true);
            }
            else if (volumeSlider.value != AudioListener.volume)
            {
                ChangeMusicVolume();
            }
        }
    }

    //changes BG music volume based on the appropriate slider
    public void ChangeMusicVolume()
    {
        //NEED TO CHANGE TO PLAYER PREFS
        AudioListener.volume = volumeSlider.value;
    }

    //button in place of file 1 for now, just loads player house scene
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerHouse");
    }
    //quit button, quits the built version of the game
    //won't work in the unity editor
    public void QuitGame()
    {
        Application.Quit();
    }

    //load save based off of button clicked
    //also loads the PlayerHouse scene if no save is in the selected file
    public void LoadSaveFile()
    {
        if (gameObject.name == "File1 Button")
        {
            if(EmptyTxt1.activeSelf == false)
            {
                saveManager.Load(1);
            }
            else
            {
                SceneManager.LoadScene("PlayerHouse");
            }
        }
        else if (gameObject.name == "File2 Button")
        {
            if (EmptyTxt2.activeSelf == false)
            {
                saveManager.Load(1);
            }
            else
            {
                SceneManager.LoadScene("PlayerHouse");
            }
        }
        else if (gameObject.name == "File3 Button")
        {
            if (EmptyTxt3.activeSelf == false)
            {
                saveManager.Load(1);
            }
            else
            {
                SceneManager.LoadScene("PlayerHouse");
            }
        }
    }
    //add function to update file names with corresponding save files when 'Play' button is clicked
    public void updateSaveNames(){
        GameObject canvas = GameObject.Find("Canvas");
        
        //check if each file name in the persistent path is not null
        for (int i = 1; i <= 3; i++)
        {
            //if file is found set the empty text for it to false
            if (saveManager.FileCheck(i))
            {
                if (i == 1)
                {
                    EmptyTxt1.SetActive(false);
                }
                else if (i == 2)
                {
                    EmptyTxt2.SetActive(false);
                }
                else if (i == 3)
                {
                    EmptyTxt3.SetActive(false);
                }
            }
            //otherwise set it to show the empty text
            else
            {
                if(i == 1)
                {
                    EmptyTxt1.SetActive(true);
                }
                else if (i == 2)
                {
                    EmptyTxt2.SetActive(true);
                }
                else if (i == 3)
                {
                    EmptyTxt3.SetActive(true);
                }
            }
        }
    }
}
