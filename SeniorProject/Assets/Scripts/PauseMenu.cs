using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !(pauseMenuPanel.activeInHierarchy) && !isPaused)
        {
            pauseMenuPanel.SetActive(true);
            isPaused = true;
        }
    }

    public void setPauseFalse()
    {
        isPaused = false;
    }
}
