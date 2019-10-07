using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCheck : MonoBehaviour
{
    public PlayerController _Script;
    private bool _Damaged;
    private AudioSource _AS;
    private float _Timer;
    private bool _PlaySoundOnce;

    private void Start()
    {
        _Script = GetComponent<PlayerController>();
        _AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_Script == null) return;

        if(_Script.enabled == false)
        {
            if(_Damaged == true)
            {
                _Timer += Time.deltaTime;
            }
        }

        if(_Timer >= 3f)
        {
            _Script.enabled = true;
            _Timer = 0f;
            _PlaySoundOnce = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(_Script == null) return;
        if(_Script.enabled == false)
        {
            Debug.Log("Timer Started");
            if (other.collider.tag != "Salt")
            {
                Debug.Log("Script Re-enabled");
                _Script.enabled = true;
                _PlaySoundOnce = false;
                _Damaged = false;
                _Timer = 0f;
            }
        }
    }
}
