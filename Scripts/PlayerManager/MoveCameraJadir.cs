using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraJadir : MonoBehaviour
{
    public Transform mainCamTransform;
    public float scalar = 1f;
    private Vector3 lastCamPos = Vector3.zero;

    void Update()
    {
        Vector3 position = transform.position;
        position.x = mainCamTransform.transform.position.x;
        //position.y = mainCamTransform.transform.position.y;
        transform.position = position;


    }
}
