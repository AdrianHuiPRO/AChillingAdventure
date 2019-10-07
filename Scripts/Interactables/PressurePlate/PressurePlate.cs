using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour
{
    private IOnPlate[] _OnPlate;
    private IOffPlate[] _OffPlate;
    private bool _Activated = false;
    public Animator _Anim1;
    public AudioClip _Triggered;
    public AudioClip _NotTriggered;
    public List<Rigidbody> _Rigidbodies = new List<Rigidbody>();
    private bool _SoundPlayed1;
    private bool _SoundPlayed2 = true;
    public AudioSource _AS;

    private void Awake()
    {
        _OnPlate = GetComponents<IOnPlate>();
        _OffPlate = GetComponents<IOffPlate>();
        _AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_Rigidbodies.Count == 0)
        {
            _Anim1.SetBool("Triggered", false);
            if(_SoundPlayed2 == false)
            {
                _AS.PlayOneShot(_NotTriggered, 0.6f);
                _SoundPlayed2 = true;
            }
            _SoundPlayed1 = false;
            OffPlate();
        }
        else if(_Rigidbodies.Count >= 1)
        {
            _Anim1.SetBool("Triggered", true);
            if(_SoundPlayed1 == false)
            {
                _AS.PlayOneShot(_Triggered, 0.6f);
                _SoundPlayed1 = true;
            }
            _SoundPlayed2 = false;
            OnPlate();
        }

        foreach (Rigidbody rb in _Rigidbodies)
        {
            if(rb.gameObject.activeInHierarchy == false)
            {
                _Rigidbodies.Remove(rb);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2") || other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("PushableBlock"))
        {
            if(other.isTrigger == false)
            {
                _Rigidbodies.Add(other.GetComponent<Rigidbody>());
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2") || other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("PushableBlock"))
        {
            if(other.isTrigger == false)
            {
                _Rigidbodies.Remove(other.GetComponent<Rigidbody>());
            }
        }
    }

    public void OnPlate()
    {
        foreach(var OnPlt in _OnPlate)
        {
            OnPlt.OnPlate();
        }
    }

    public void OffPlate()
    {
        foreach(var OffPlt in _OffPlate)
        {
            OffPlt.OffPlate();
        }
    }
}
