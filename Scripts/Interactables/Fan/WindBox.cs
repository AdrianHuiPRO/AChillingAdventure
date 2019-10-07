using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBox : MonoBehaviour
{
    public float _Strength = 3f;
    public Vector3 _WindDirection;
    public float _Timer;
    private bool _BlowingRight = false;

    private void Awake()
    {
        if(transform.root.rotation == Quaternion.Euler(0f, 180f, 0f) ||
        transform.root.rotation == Quaternion.Euler(0f, -180f, 0f) ||
        transform.root.rotation == Quaternion.Euler(0f, 0f, 180f) ||
        transform.root.rotation == Quaternion.Euler (0f, 0f, -180f))
        {
            _BlowingRight = true;
        }
        else
        {
            _BlowingRight = false;
        }

        if(_BlowingRight == false)
        {
            _Strength = _Strength * -1f;
        }
        else if(_BlowingRight == true)
        {
            _Strength = _Strength * 1f;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        _Timer += Time.deltaTime * 25f;

        if(other.CompareTag("Player2") && other.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke)
        {
            Rigidbody smokeRb = other.GetComponent<Rigidbody>();
            if(smokeRb != null)
            {
                smokeRb.AddForce(_WindDirection * _Timer * _Strength);
            }

            if(_Timer >= 200f)
            {
                _Timer = 200f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _Timer = 0f;
    }
}
