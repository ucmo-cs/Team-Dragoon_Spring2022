using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathButtonBehavior : MonoBehaviour
{
    public void Retry()
    {
        SceneChanger.instance.PreviousScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
