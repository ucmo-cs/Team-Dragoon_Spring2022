using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Slider slider;

    public bool isPaused;

    // Update is called once per frame

    private void Awake()
    {
        slider.value = PlayerPrefs.GetFloat("BG Music");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !(pauseMenuPanel.activeInHierarchy) && !isPaused)
        {
            pauseMenuPanel.SetActive(true);
            isPaused = true;
        }
        else if (slider.value != PlayerPrefs.GetFloat("BG Music"))
        {
            PlayerPrefs.SetFloat("BG Music", slider.value);
        }
    }

    public void setPauseFalse()
    {
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
