using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BrokenWireConnect : MonoBehaviour
{
    private IOffWire[] _IOffwire;
    private IOnWire[] _IOnWire;
    private AudioSource _AS;
    public AudioSource _AS2;
    public AudioClip _StopElecSound;
    public AudioClip _StartElecSound;
    public AudioClip[] _SmokeToFire;
    public AudioClip[] _SmokeToFireVoice;
    public AudioClip[] _Sparking;
    public AudioClip _WireBuzz;
    public ParticleSystem[] _Sparks;
    private bool _IsVisible;
    private bool _PlaySoundOnce;
    public AudioSource _AS3;
    private bool _CoroutineIsAcive;
    private bool _DoOnce2;
    private bool _DoOnce3;

    public bool _WaterIsConnected = false;

    private void OnBecameInvisible()
    {
        _IsVisible = false;
        Debug.Log("Is Invisible");
    }

    private void OnBecameVisible()
    {
        _IsVisible = true;
        Debug.Log("Is visible");
    }

    private void Start()
    {
        _IOnWire = GetComponents<IOnWire>();
        _IOffwire = GetComponents<IOffWire>();
        _AS = GetComponent<AudioSource>();
    }

    bool _DoOnce = false;

    private void Update()
    {
        Sounds();
    }

    private void Sounds()
    {
        if(_IsVisible == true)
        {
            if(_WaterIsConnected == false)
            {
                if(_CoroutineIsAcive == false)
                {
                    StartCoroutine("Spark");
                    _CoroutineIsAcive = true;
                }

                if(_PlaySoundOnce == false)
                {
                    _AS.loop = true;
                    _AS.clip = _WireBuzz;
                    _AS.Play();
                    _AS3.clip = null;
                    _AS3.loop = false;
                    _AS3.Stop();
                    _DoOnce2 = false;
                    _PlaySoundOnce = true;
                }

                foreach (var item in _Sparks)
                {
                    if(_DoOnce == false)
                    {
                        item.Play();
                        _DoOnce = true;
                    }
                }
            }
            else if(_WaterIsConnected == true)
            {
                _AS.loop = false;
                _AS.clip = _WireBuzz;
                _AS.Stop();
                if(_DoOnce2 == false)
                {
                    _AS3.loop = true;
                    _AS3.clip = _StartElecSound;
                    _AS3.Play();
                    _DoOnce2 = true;
                }
                _PlaySoundOnce = false;
                _DoOnce = false;
                _CoroutineIsAcive = false;
                foreach (var item in _Sparks)
                {
                    item.Stop();
                }
                StopCoroutine("Spark");
            }
        }
        else if(_IsVisible == false)
        {
            _AS.loop = false;
            _AS.clip = _WireBuzz;
            _AS.Stop();
            _AS3.loop = false;
            _AS3.clip = null;
            _AS3.Stop();
            _DoOnce2 = false;
            _PlaySoundOnce = false;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.GetComponent<PlayerController>() == null) return;
        if(other.gameObject.tag == "Player1")
        {   
            if(other.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
            {
                OnWire();
                _WaterIsConnected = true;
            }
        }

        if(_WaterIsConnected == false)
        {
            if(other.gameObject.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke)
            {
                _AS.PlayOneShot(_SmokeToFire[Random.Range(0, _SmokeToFire.Length)]);
                _AS.PlayOneShot(_SmokeToFireVoice[Random.Range(0, _SmokeToFireVoice.Length)]);
                other.gameObject.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Fire;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
       if(other.gameObject.tag == "Player1" && other.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
        {
            OffWire();
            _WaterIsConnected = false;
        }
    }

    public void OnWire()
    {
        if(_DoOnce3 == false)
        {
            _AS.PlayOneShot(_StopElecSound);
            Debug.Log("Played Stop Elec");
            _DoOnce3 = true;
        }
        foreach(var OnWire in _IOnWire)
        {
            OnWire.OnWire();
        }
    }

    public void OffWire()
    {
        foreach(var OffWire in _IOffwire)
        {
            OffWire.OffWire();
            _DoOnce3 = false;
        }
    }

    IEnumerator Spark()
    {
        _AS2.PlayOneShot(_Sparking[Random.Range(0, _Sparking.Length)], 0.6f);
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        _CoroutineIsAcive = false;
    }
}
