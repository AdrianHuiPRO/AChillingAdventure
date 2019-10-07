using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LiquidtoSolid : MonoBehaviour
{
    private AudioSource _AS;
    public AudioClip[] _LiquidToSolid;
    public AudioClip[] _LiquidToSolidVoice;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player1")
        {
            if(other.gameObject.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
            {
                other.gameObject.GetComponent<PlayerController>()._Player1CurrentState = PlayerController.Player1State.Solid;
                _AS.PlayOneShot(_LiquidToSolid[Random.Range(0, _LiquidToSolid.Length)]);
                _AS.PlayOneShot(_LiquidToSolidVoice[Random.Range(0, _LiquidToSolidVoice.Length)]);
            }
        }
    }
}
