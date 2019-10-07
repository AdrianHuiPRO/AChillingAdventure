using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FiretoSmoke : MonoBehaviour
{
    private AudioSource _AS;
    public AudioClip[] _FireToSmokeSounds;
    public AudioClip[] _FireToSmokeVoice;
    public AudioClip[] _Sprinkler;
    private bool _SoundIsPlaying;
    private bool _IsVisible = false;
    public AudioSource _AS2;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void OnBecameVisible()
    {
        _IsVisible = true;
    }

    private void OnBecameInvisible()
    {
        _IsVisible = false;
    }

    private void Update()
    {
        if(_SoundIsPlaying == false)
        {
            if(_IsVisible == true)
            {
                _AS.clip = _Sprinkler[Random.Range(0, _Sprinkler.Length)];
                _AS.loop = true;
                _AS.Play();
                _SoundIsPlaying = true;
            }
        }

        if(_IsVisible == false)
        {
            _AS.clip = _Sprinkler[Random.Range(0, _Sprinkler.Length)];
            _AS.loop = false;
            _AS.Stop();

        }
    }

    private void OnParticleCollision(GameObject other) 
    {
        if(other.CompareTag("Player2"))
        {
            if(other.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire)
            {
                _AS2.PlayOneShot(_FireToSmokeSounds[Random.Range(0, _FireToSmokeSounds.Length)], 0.4f);
                _AS2.PlayOneShot(_FireToSmokeVoice[Random.Range(0, _FireToSmokeVoice.Length)], 1f);
                other.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Smoke;
            }
        }
        else if(other.CompareTag("Lighter"))
        {
            if(other.GetComponent<Lighter>()._LighterCurrentState == Lighter.LighterState.On)
            {
                other.GetComponent<Lighter>()._LighterCurrentState = Lighter.LighterState.Off;
            }
        }
    }
}
