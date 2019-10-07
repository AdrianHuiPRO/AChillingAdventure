using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltyCheeks : MonoBehaviour
{
    public AudioClip[] _IdleSounds;
    private AudioSource _AS;
    private Animator _Anim;
    private Salty_Shoot _Shoot;
    private bool _IdleSoundIsPlaying;
    private bool _IsVisible;

    private void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Shoot = GetComponent<Salty_Shoot>();
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

    public void Update()
    {
        if(_Anim.GetCurrentAnimatorStateInfo(0).IsName("Salty_Idle"))
        {
            if(_IdleSoundIsPlaying == false)
            {
                if(_IsVisible == true)
                {
                    StartCoroutine("IdleSound");
                    _IdleSoundIsPlaying = true;
                }
                else
                {
                    StopCoroutine("IdleSound");
                    _IdleSoundIsPlaying = false;
                }
            }
        }

        if(this.gameObject.transform.rotation == Quaternion.Euler(0f, 90f, 0f) || this.gameObject.transform.rotation == Quaternion.Euler(0f, -270f, 0f))
        {
            _Shoot._IsFacingRight = true;
        }
        else if(this.gameObject.transform.rotation == Quaternion.Euler(0f, 270f, 0f) || this.gameObject.transform.rotation == Quaternion.Euler(0f, -90f, 0f))
        {
            _Shoot._IsFacingRight = false;
        }    
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            _Anim.SetBool("IsShooting", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            _Anim.SetBool("IsShooting", false);
        }  
    }

    IEnumerator IdleSound()
    {
        yield return new WaitForSeconds(Random.Range(8f, 10f));
        _AS.clip = _IdleSounds[Random.Range(0, _IdleSounds.Length)];
        _AS.Play();
        yield return new WaitWhile(()=> _AS.isPlaying);
        _IdleSoundIsPlaying = false;
    }
}
