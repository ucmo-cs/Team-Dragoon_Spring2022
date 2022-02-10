using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public float smoothing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPostion = new Vector3(target.position.x, target.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, targetPostion, smoothing);
        }
    }
}
