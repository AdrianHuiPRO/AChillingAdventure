using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private RespawnObjects _RespawnObjects;
    private Vector3 _Origin;
    private Rigidbody _RB;
    private AudioSource _AS;
    public AudioSource _AS2;
    public AudioClip _Moving;
    public AudioClip[] _Land;
    
    public LayerMask GroundBlockingLayers;
    public bool _IsPBlock = false;

    private bool _SoundPlayed = false;
    private bool _SoundCheckIfMoving = false;
    private bool _IsVisible = false;

    private void OnEnable()
    {
        _RespawnObjects = GameObject.FindObjectOfType<RespawnObjects>();
        transform.position = _Origin;
    }

    private void Awake()
    {
        _Origin = transform.position;
        _RespawnObjects = GetComponent<RespawnObjects>();
        _AS = GetComponent<AudioSource>();
        _RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _RespawnObjects = GameObject.FindObjectOfType<RespawnObjects>();
    }

    private void OnDisable()
    {
        if(_RespawnObjects == null) return;
        _RespawnObjects.ReEnableGameObject(this.gameObject);
    }

    private void Update()
    {
        if(_IsPBlock == true)
        {
            if(_IsVisible == true)
            {
                PlayLandSound();
                PlayMovingSound();
            }
            else if(_IsVisible == false)
            {
                _AS.loop = false;
                _AS.Stop();
                _SoundCheckIfMoving = false;
            }
        }
    }

    bool IsGrounded()
    {
		Vector3 size = Vector3.one*0.5f;
		Vector3 center = transform.position;
		RaycastHit hit = new RaycastHit();

        if (Physics.BoxCast(center + Vector3.up, size, Vector3.down, out hit, Quaternion.identity, 1f, GroundBlockingLayers, QueryTriggerInteraction.Ignore))
        {
            //hit ground
            return true;
        }
		//missed ground
		return false;
    }

    private void OnBecameVisible()
    {
        _IsVisible = true;
        Debug.Log(name + " Is Visible");
    }

    private void OnBecameInvisible()
    {
        _IsVisible = false;
        Debug.Log(name + " Is not visible");
    }

    private void PlayLandSound()
    {
        if(_SoundPlayed == false && IsGrounded() == true)
        {
            if(_SoundPlayed == false)
            {
                _AS2.PlayOneShot(_Land[Random.Range(0, _Land.Length)], 1f);
                Debug.Log(name + " Land sound played");
            }
            _SoundPlayed = true;
        }
        else if(_SoundPlayed == true && IsGrounded() == false)
        {
            _SoundPlayed = false;
        }
    }

    private void PlayMovingSound()
    {
        if(_RB.velocity.x >= 1.5f && IsGrounded() == true)
        {
            if(_SoundCheckIfMoving == false)
            {
                _AS.loop = true;
                _AS.clip = _Moving;
                _AS.Play();
                _SoundCheckIfMoving = true;
            }
        }
        else if(_RB.velocity.x <= -1.5f && IsGrounded() == true)
        {
            if(_SoundCheckIfMoving == false)
            {
                _AS.loop = true;
                _AS.clip = _Moving;
                _AS.Play();
                _SoundCheckIfMoving = true;
            }
        }
        else
        {
            _AS.loop = false;
            _AS.Stop();
            _SoundCheckIfMoving = false;
        }
    }
}
