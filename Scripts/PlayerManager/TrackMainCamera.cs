using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMainCamera : MonoBehaviour
{
    public float scalar = 1f;

    public Transform mainCamTransform;

    private Vector3 lastCamPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(mainCamTransform == null) return;
        mainCamTransform = Camera.main.transform;
        lastCamPos = mainCamTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(mainCamTransform == null) return;
        Vector3 dif = mainCamTransform.position - lastCamPos;
        transform.position += dif * scalar;
        lastCamPos = mainCamTransform.position;
    }
}
