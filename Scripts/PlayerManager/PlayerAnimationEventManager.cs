using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventManager : MonoBehaviour
{
    public AudioClip[] _Footstep;
    private AudioSource _AudioSource;
    private Pickable _Pickable;
    public bool _DidHitKillZ;

    private void Awake()
    {
        _AudioSource = GetComponentInParent<AudioSource>();
    }

    public void FootSounds()
    {
        if(_DidHitKillZ == false)
        {
            _AudioSource.PlayOneShot(_Footstep[Random.Range(0,_Footstep.Length)]);
        }
    }
}
