using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillZ : MonoBehaviour
{
    private CheckpointManager _CM;

    private GameObject _Player1;
    private GameObject _Player2;
    private AudioSource _AS;
    public AudioSource _AS2;
    public AudioClip _RangehoodSound;
    private bool _TimerStartP1 = false;
    private bool _TimerStartP2 = false;
    private float _TimerP1;
    private float _TimerP2;
    private bool _DoOnce;
    private bool _DoOnce3;
    private bool _DoOnce2 = false;
    public bool _IsRangeHood;
    private bool _IsVisible;

    private void OnBecameInvisible()
    {
        _IsVisible = false;
        Debug.Log(name + " Is not visible");
    }

    private void OnBecameVisible()
    {
        _IsVisible = true;
        Debug.Log(name + " Is visible");
    }

    private void Start()
    {
        _CM = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
        _AS = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(_IsRangeHood == true)
        {
            if(_IsVisible == true)
            {
                if(_DoOnce2 == false)
                {
                    _AS2.clip = _RangehoodSound;
                    _AS2.loop = true;
                    Debug.Log("Is Playing");
                    _AS2.Play();
                    _DoOnce2 = true;
                }
            }

            if(_IsVisible == false)
            {
                if(_DoOnce2 == true)
                {
                    _AS2.clip = _RangehoodSound;
                    _AS2.loop = false;
                    Debug.Log("Is not Playing");
                    _AS2.Stop();
                    _DoOnce2 = false;
                }
            }
        }

        if(_TimerStartP1 == true)
        {
            _TimerP1 += Time.deltaTime;
        }

        if(_TimerStartP2 == true)
        {
            _TimerP2 += Time.deltaTime;
        }

        if(_TimerP1 >= 2f)
        {
            _Player1.transform.position = _CM._LastCheckpointPosP1 + Vector3.right;
            _Player1.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(_Player1.GetComponentInChildren<PlayerAnimationEventManager>() != null)
            {
                _Player1.GetComponentInChildren<PlayerAnimationEventManager>()._DidHitKillZ = false;
            }
            _AS.PlayOneShot(_Player1.GetComponent<PlayerController>()._Respawn[Random.Range(0, _Player1.GetComponent<PlayerController>()._Respawn.Length)]);
            _TimerP1 = 0f;
            _DoOnce = false;
            _TimerStartP1 = false;
        }

        if(_TimerP2 >= 2f)
        {
            _Player2.transform.position = _CM._LastCheckpointPosP2 + Vector3.left;
            _Player2.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(_Player2.GetComponentInChildren<PlayerAnimationEventManager>() != null)
            {
                _Player2.GetComponentInChildren<PlayerAnimationEventManager>()._DidHitKillZ = false;
            }
            _AS.PlayOneShot(_Player2.GetComponent<PlayerController>()._Respawn[Random.Range(0, _Player2.GetComponent<PlayerController>()._Respawn.Length)]);
            _TimerP2 = 0f;
            _DoOnce3 = false;
            _TimerStartP2 = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player1"))
        {
            _Player1 = other.gameObject;
            if(_Player1.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Solid && _DoOnce == false)
            {
                _AS.PlayOneShot(_Player1.GetComponent<PlayerController>()._Death[Random.Range(0, _Player1.GetComponent<PlayerController>()._Death.Length)], 1f);
                _TimerStartP1 = true;
                _Player1.GetComponentInChildren<PlayerAnimationEventManager>()._DidHitKillZ = true;
                _DoOnce = true;
            }
            else if(_Player1.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid && _DoOnce == false)
            {
                _AS.PlayOneShot(_Player1.GetComponent<PlayerController>()._DeathSecondState[Random.Range(0, _Player1.GetComponent<PlayerController>()._DeathSecondState.Length)], 1f);
                _TimerStartP1 = true;
                _DoOnce = true;
            }
        }

        if(other.CompareTag("Player2"))
        {
            _Player2 = other.gameObject;
            if(_Player2.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire && _DoOnce3 == false)
            {
                _AS.PlayOneShot(_Player2.GetComponent<PlayerController>()._Death[Random.Range(0, _Player2.GetComponent<PlayerController>()._Death.Length)], 1f);
                _TimerStartP2 = true;
                _Player2.GetComponentInChildren<PlayerAnimationEventManager>()._DidHitKillZ = true;
                _DoOnce3 = true;
            }
            else if(_Player2.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke && _DoOnce3 == false)
            {
                _AS.PlayOneShot(_Player2.GetComponent<PlayerController>()._Death[Random.Range(0, _Player2.GetComponent<PlayerController>()._Death.Length)], 1f);
                _TimerStartP2 = true;
                _DoOnce3 = true;
            }
        }

        if(other.CompareTag("PushableBlock") || other.CompareTag("Bomb"))
        {
            other.gameObject.SetActive(false);
        }
    }
}

