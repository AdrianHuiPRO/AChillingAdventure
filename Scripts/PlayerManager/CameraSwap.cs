using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public PlayerCamera _Cam;
    public Camera _NewCameraPosition;

    [Header("This should be between the values of 0.15-1 for the proper effect")]
    public float _SmoothTime = 0.15f;
    public float _ChangeSize = 10f;
    public bool _MakeTheCameraStatic = false;
    private bool _Player1Entered = false;
    private bool _Player2Entered = false;
    private bool _Player1Exit = false;
    private bool _Player2Exit = false;

    private void Start()
    {
        _ChangeSize = _NewCameraPosition.GetComponent<Camera>().orthographicSize;
        _NewCameraPosition.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player1"))
        {
            _Player1Entered = true;
            _Player1Exit = false;
        }
        else if(other.gameObject.CompareTag("Player2"))
        {
            _Player2Entered = true;
            _Player2Exit = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player1"))
        {
            _Player1Exit = true;
            _Player1Entered = false;
        }
        else if(other.gameObject.CompareTag("Player2"))
        {
            _Player2Exit = true;
            _Player2Entered = false;
        }
    }

    private void FixedUpdate()
    {
        if(_Player1Exit == true && _Player2Exit == true)
        {
            _Cam.enabled = true;
            _Player1Entered = false;
            _Player2Entered = false;
            _Player1Exit = false;
            _Player2Exit = false;
        }

        if(_Player1Entered == true && _Player2Entered == true)
        {
            _Cam.enabled = false;
            LerpCamera();
        }
    }

    private void LerpCamera()
    {
        Debug.Log("Lerping");
        _Cam.transform.position = Vector3.Lerp(_Cam.transform.position, _NewCameraPosition.transform.position, Time.fixedDeltaTime * _SmoothTime);
        _Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(_Cam.GetComponent<Camera>().orthographicSize, _ChangeSize, Time.deltaTime * _SmoothTime);
    }
}
