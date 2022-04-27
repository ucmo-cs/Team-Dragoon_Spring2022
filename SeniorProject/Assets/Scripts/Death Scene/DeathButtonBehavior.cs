using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathButtonBehavior : MonoBehaviour
{
    public void Retry()
    {

        AudioManager.instance.StartCoroutine(AudioManager.FadeIn(AudioManager.instance.battleMusic, 3f));
        SceneChanger.instance.PreviousScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
