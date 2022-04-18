using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider volumeSlider, FX;
    public TMPro.TextMeshProUGUI pressAnyText, deleteTxt;
    public GameObject MainMenuPanel;
    public Button file1Btn, file2Btn, file3Btn;

    //used for to check which file to load on button press
    public GameObject EmptyTxt1, EmptyTxt2, EmptyTxt3;
    private static bool deleteSelected;
    private ColorBlock colorBlock;

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
                //deleteSelected = false;
            }
            else if (volumeSlider.value != AudioListener.volume)
            {
                ChangeVolume();
            }
            else if (PlayerPrefs.GetFloat("FX Level") == 0)
            {
                PlayerPrefs.SetFloat("FX Level", FX.value);
            }
        }
    }

    //changes BG music volume based on the appropriate slider
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("BG Music", volumeSlider.value);
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
            if (deleteSelected)
            {
                SaveManager.instance.DeleteSave(1);
                updateSaveNames();
                DeleteButtonUnselected();
                //Debug.Log("File1 deleted");
            }
            else if (EmptyTxt1.activeSelf == false)
            {
                SaveManager.instance.Load(1);
                //Debug.Log("Save file loaded");
                LoadFirstHalf();
            }
            else
            {
                SceneManager.LoadScene("PlayerHouse");
                SaveManager.instance.activeSave.saveNumber = 1;
            }
        }
        else if (gameObject.name == "File2 Button")
        {
            if (deleteSelected)
            {
                SaveManager.instance.DeleteSave(2);
                updateSaveNames();
                DeleteButtonUnselected();
                //Debug.Log("File2 deleted");
            }
            else if (EmptyTxt2.activeSelf == false)
            {
                SaveManager.instance.Load(2);
                LoadFirstHalf();
            }
            else
            {
                SceneManager.LoadScene("PlayerHouse");
                SaveManager.instance.activeSave.saveNumber = 2;
            }
        }
        else if (gameObject.name == "File3 Button")
        {
            if (deleteSelected)
            {
                SaveManager.instance.DeleteSave(3);
                updateSaveNames();
                DeleteButtonUnselected();
                //Debug.Log("File3 deleted");
            }
            else if (EmptyTxt3.activeSelf == false)
            {
                SaveManager.instance.Load(3);
                LoadFirstHalf();
            }
            else
            {
                SceneManager.LoadScene("PlayerHouse");
                SaveManager.instance.activeSave.saveNumber = 3;
            }
        }
    }

    //add function to update file names with corresponding save files when 'Play' button is clicked
    public void updateSaveNames() {
        GameObject canvas = GameObject.Find("Canvas");

        //check if each file name in the persistent path is not null
        for (int i = 1; i <= 3; i++)
        {
            //if file is found set the empty text for it to false
            if (SaveManager.instance.FileCheck(i))
            {
                if (i == 1)
                {
                    EmptyTxt1.SetActive(false);
                    //Debug.Log("file in slot 1");
                }
                else if (i == 2)
                {
                    EmptyTxt2.SetActive(false);
                    //Debug.Log("file in slot 2");
                }
                else if (i == 3)
                {
                    EmptyTxt3.SetActive(false);
                    //Debug.Log("file in slot 3");
                }
            }
            //otherwise set it to show the empty text
            else
            {
                if (i == 1)
                {
                    EmptyTxt1.SetActive(true);
                    //Debug.Log("no file in slot 1");
                }
                else if (i == 2)
                {
                    EmptyTxt2.SetActive(true);
                    //Debug.Log("no file in slot 2");
                }
                else if (i == 3)
                {
                    EmptyTxt3.SetActive(true);
                    //Debug.Log("no file in slot 3");
                }
            }
        }
    }

    public void DeletePressed(){
        if (!deleteSelected)
        {
            DeleteButtonSelected();
        }
        else if (deleteSelected)
        {
            DeleteButtonUnselected();
        }
    }

    private void DeleteButtonSelected()
    {
        colorBlock = file1Btn.colors;

        ColorBlock tempCB = colorBlock;

        tempCB.highlightedColor = Color.red;

        file1Btn.colors = tempCB;
        file2Btn.colors = tempCB;
        file3Btn.colors = tempCB;

        deleteTxt.text = "Cancel";

        deleteSelected = true;
        //Debug.Log(deleteSelected);
    }

    private void DeleteButtonUnselected()
    {
        deleteTxt.text = "Delete";
        file1Btn.colors = colorBlock;
        file2Btn.colors = colorBlock;
        file3Btn.colors = colorBlock;

        //deleteTxt.text = "Delete";

        deleteSelected = false;
        //Debug.Log("Delete Unselected() - called");
    }

    public void LoadFirstHalf()
    {
        //load scene and once in scene we can check which game objects need to be loaded
        SceneManager.LoadScene(SaveManager.instance.activeSave.sceneName.ToString());
        
        /*if (SaveManager.instance.activeSave.sceneName.ToString() != "PlayerHouse")
        {
            Debug.Log("load import obj - not player house");
            SaveManager.instance.LoadImportantObjects();
        }*/
        
        
        //Debug.Log(SceneManager.GetActiveScene().name + " 1");
        SaveManager.instance.gamePartialLoad = true;
    }
}
