using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    private CinemachineVirtualCamera vcam;
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void FixedUpdate()
    {

        var vcam = GetComponent<CinemachineVirtualCamera>();

        tPlayer = GameObject.FindWithTag("Player");

        //used to reassign the tPlayer after loading
        if (tPlayer == null)
        {
            if (SceneManager.GetActiveScene().name == SaveManager.instance.activeSave.sceneName)
            {
                tPlayer = GameObject.FindWithTag("Player");
            }
        }
        else
        {
        tFollowTarget = tPlayer.transform;
        vcam.LookAt = tFollowTarget;
        vcam.Follow = tFollowTarget;
        }
    }
}
