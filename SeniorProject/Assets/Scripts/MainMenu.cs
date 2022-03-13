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

    // Update is called once per frame
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
}
