using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _MoveLocation;
    [SerializeField]
    private bool _IsThePlatformGoingToTheGround = true;
    public float _Speed = 5f;
    public float _StopTime = 1f;
    [Header("Put in the point closest to the ground, defualt is 0")]
    public int _PointClosestToTheGround = 0;

    private int _Current = 0;
    private float _PointRadius = 0.001f;
    private float _DistanceForActivatingBoxcast = 0.25f;
    private float _Timer;
    private bool _IsMoving = true;
    private bool _TimerStart;
    private AudioSource _AS;
    public AudioClip _Moving;
    private bool _SoundCheckIfMoving;
    public bool _IsVisible = false;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void PlayMovingSound()
    {
        if(_IsMoving == true)
        {
            if(_SoundCheckIfMoving == false)
            {
                if(_IsVisible == true)
                {
                    _AS.loop = true;
                    _AS.clip = _Moving;
                    _AS.Play();
                    _SoundCheckIfMoving = true;
                }
            }
        }
        else
        {
            _AS.loop = false;
            _AS.Stop();
            _SoundCheckIfMoving = false;
        }        
    }
    
    private void Update()
    {
        if(_TimerStart == true)
        {
            _Timer += Time.deltaTime;
        }

        if(_Timer >= _StopTime)
        {
            _Timer = 0f;
            _TimerStart = false;
            _IsMoving = true;
        }
        PlayMovingSound();
        MovePlatform();
    }


    private void OnBecameVisible()
    {        
        _IsVisible = true;
        Debug.Log(name + " Is visible");
    }

    private void OnBecameInvisible()
    {
        _IsVisible = false;
        Debug.Log(name + " Is not visible");
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Rigidbody>() != null)
        {
            if(_IsThePlatformGoingToTheGround == true)
            {
                if(Vector3.Distance(_MoveLocation[_PointClosestToTheGround].transform.position, transform.position) <= _DistanceForActivatingBoxcast)
                {
                    if(other.gameObject.transform.position.y < transform.position.y)
                    {
                        other.gameObject.transform.position += new Vector3(0f, 1f, 0f);
                    }
                }
            }
        }
    }

    private void MovePlatform()
    {
        if(Vector3.Distance(_MoveLocation[_Current].transform.position, transform.position) <= _PointRadius)
        {
            _IsMoving = false;

            if(_TimerStart == false)
            {
                _TimerStart = true;
            }

            _Current++;
            if(_Current >= _MoveLocation.Length)
            {
                _Current = 0;
            }
        }
        if(_IsMoving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
        }
    }
}
