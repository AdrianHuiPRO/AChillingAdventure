using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform_AtoB : MonoBehaviour, IOnPlate, IOffPlate
{
    [SerializeField]
    private GameObject[] _MoveLocation;

    public GameObject _Platform;

    private int _Current = 0;
    private float _PointRadius = 0.001f;
    private float _DistanceForActivatingBoxcast = 0.4f;
    private AudioSource _AS;
    public AudioClip _Moving;

    public float _Speed = 5f;
    private bool _IsPressed = false;
    [Header("Put in the point closest to the ground, defualt is 0")]
    [SerializeField]
    private bool _IsThePlatformGoingToTheGround = true;
    [SerializeField]
    private int _PointClosestToTheGround = 0;
    private bool _SoundCheckIfMoving;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void Update() 
    {
        MoveThePlatform();
        // if(Vector3.Distance(_MoveLocation[0].transform.position, _Platform.transform.position) <= _DistanceForActivatingBoxcast)
        // {
        //     Vector3 size = this.transform.localScale;
        //     RaycastHit hit = new RaycastHit();
        //     float SizeY = transform.localScale.y;
        //     int distance = (int) SizeY;

        //     if (Physics.BoxCast(transform.position, size, Vector3.down, out hit, Quaternion.identity, distance))
        //     {
        //         if(hit.transform.GetComponent<Rigidbody>() == null) return;
        //         hit.transform.GetComponent<Rigidbody>().position = hit.transform.GetComponent<Rigidbody>().position + new Vector3(0f, 1f, 0f);
        //     }
        // }
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

    private void MoveThePlatform()
    {
        if(_IsPressed == false)
        {
            _Current = 0;
            if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Platform.transform.position) <= _PointRadius)
            {
                if(_SoundCheckIfMoving == true)
                {
                    _AS.Stop();
                    _SoundCheckIfMoving = false;
                }
                if(_Current >= _MoveLocation.Length)
                {
                    //Intentionally do nothing
                }
            }
            else if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Platform.transform.position) > _PointRadius)
            {
                if(_SoundCheckIfMoving == false)
                {
                    _AS.loop = true;
                    _AS.clip = _Moving;
                    _AS.Play();
                    _SoundCheckIfMoving = true;
                }
            }
            _Platform.transform.position = Vector3.MoveTowards(_Platform.transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
        }
        else if(_IsPressed == true)
        {
            _Current = 1;
            if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Platform.transform.position) <= _PointRadius)
            {
                if(_SoundCheckIfMoving == true)
                {
                    _AS.Stop();
                    _SoundCheckIfMoving = false;
                }
                if(_Current >= _MoveLocation.Length)
                {
                    _Current = 1;
                }
            }
            else if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Platform.transform.position) > _PointRadius)
            {
                if(_SoundCheckIfMoving == false)
                {
                    _AS.loop = true;
                    _AS.clip = _Moving;
                    _AS.Play();
                    _SoundCheckIfMoving = true;
                }
            }
            _Platform.transform.position = Vector3.MoveTowards(_Platform.transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
        }
    }

    public void OnPlate()
    {
        _IsPressed = true;
    }

    public void OffPlate()
    {
        _IsPressed = false;
    }
}
