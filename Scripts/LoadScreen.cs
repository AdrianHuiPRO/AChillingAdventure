using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreen : MonoBehaviour
{
    private CharacterSelect _CS;
    private Animator _Anim;
    public AudioSource[] _DisableSounds;

    private void Awake()
    {
        _CS = GameObject.FindObjectOfType<CharacterSelect>();
        _Anim = GetComponent<Animator>();
    }

    public void Player1ButtonIcebert()
    {
        _Anim.SetTrigger("Player1IsIcebert");
        foreach (AudioSource item in _DisableSounds)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void Player2ButtonIcebert()
    {
        _Anim.SetTrigger("Player2IsIcebert");
        foreach (AudioSource item in _DisableSounds)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void Player1IsIcebert()
    {
        _CS.Player1isIcebert();
    }

    public void Player2IsIcebert()
    {
        _CS.Player2isIcebert();
    }
}
