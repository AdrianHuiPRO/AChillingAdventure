using InControl;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour
{
    [SerializeField]
    private string[] _PickAbleTags;

    public Image _RightTriggerImage;
    public Canvas _Canvas;
    public LayerMask GroundBlockingLayers;

    private InputDevice _CurrentDevice;
    private Transform _CurrentOther;
    public bool _Picked = false;
    private Rigidbody _RB;
    private float _Distance;
    private float _timer;
    private bool _ThrowButtonIsPressed;
    private int _TrackButtonPresses = 0;
    private AudioSource _AS;
    private Vector3 _OriginalSize;
    private bool _SoundPlayed;
    public AudioClip _Land;
    public AudioClip _Moving;
    public AudioClip _PickedUp;
    private bool _SoundCheckIfMoving;
    public AudioSource _AS2;
    private GameObject _CurrentParent;
    private bool _IsVisible = false;
    [SerializeField]
    private Transform _CanvasOrigin;
    [HideInInspector]public bool _IsFacingRight; 

    private void OnDisable()
    {
        _RightTriggerImage.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _AS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _OriginalSize = transform.localScale;
        _Canvas.gameObject.transform.SetParent(null);
    }

    bool IsGrounded()
    {
        Vector3 size = Vector3.one * 0.5f;
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

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            _CurrentDevice = playerController.Device;
            _CurrentOther = other.transform;
            playerController._CurrentInteractable = this.gameObject;
        }

        if (_RightTriggerImage == null) return;
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if(other.gameObject.CompareTag("Player1") && playerController._Player1CurrentState ==  PlayerController.Player1State.Solid)
            {
                if (_RightTriggerImage.gameObject.activeInHierarchy == false)
                {
                    _RightTriggerImage.gameObject.SetActive(true);
                }
            }
            else if(other.gameObject.CompareTag("Player2") && playerController._Player2CurrentState == PlayerController.Player2State.Fire)
            {
                if (_RightTriggerImage.gameObject.activeInHierarchy == false)
                {
                    _RightTriggerImage.gameObject.SetActive(true);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        _Canvas.transform.position = _CanvasOrigin.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if (playerController != null && _Picked == false)
        {
            _CurrentDevice = null;
            _CurrentOther = null;
        }

        if (_RightTriggerImage == null) return;
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (_RightTriggerImage.gameObject.activeInHierarchy == true)
            {
                _RightTriggerImage.gameObject.SetActive(false);
            }
        }
    }

    private void PlayLandSound()
    {
        if (_SoundPlayed == false && IsGrounded() == true)
        {
            if(_SoundPlayed == false)
            {
                _AS2.PlayOneShot(_Land);
            }
            _SoundPlayed = true;
        }
        else if (_SoundPlayed == true && IsGrounded() == false)
        {
            _SoundPlayed = false;
        }
    }

    private void PlayMovingSound()
    {
        if (_RB.velocity.x >= 1f && IsGrounded() == true)
        {
            if (_SoundCheckIfMoving == false)
            {
                _AS.loop = true;
                _AS.clip = _Moving;
                _AS.Play();
                _SoundCheckIfMoving = true;
            }
        }
        else if (_RB.velocity.x <= -1f && IsGrounded() == true)
        {
            if (_SoundCheckIfMoving == false)
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

    private void Update()
    {
        if(_Picked == false)
        {
            PlayLandSound();
            PlayMovingSound();
        }
        else if(_Picked == true)
        {
            _AS.loop = false;
            _AS.Stop();
        }

        if (_CurrentDevice == null) return;

        // if(_CurrentOther != null)
        // {
        //     _CurrentDevice = _CurrentOther.GetComponent<PlayerController>().Device;
        // }

        if (transform.parent == null)
        {
            transform.localScale = _OriginalSize;
            Physics.IgnoreCollision(_CurrentOther.GetComponent<CapsuleCollider>(), this.gameObject.GetComponent<Collider>(), false);
        }

        if (_Picked == true && transform.parent != null)
        {
            _RB.position = transform.parent.GetComponentInParent<PlayerController>().Grab.position;
            _RB.velocity = Vector3.zero;
            _RB.angularVelocity = Vector3.zero;
        }
        else if (_Picked == false && transform.parent == null)
        {
            _RB.position = transform.position;
            _RB.velocity = _RB.velocity;
            _RB.angularVelocity = _RB.velocity;
            transform.SetParent(null);
        }
        else if (_Picked == false && transform.parent != null)
        {
            transform.SetParent(null);
        }

        if (_CurrentOther.GetComponent<PlayerController>()._CurrentInteractable == this.gameObject && _CurrentOther.GetComponent<PlayerController>().Grab.transform.childCount == 0)
        {
            if (transform.parent == null)
            {
                if (_CurrentOther.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Solid
                && _CurrentOther.CompareTag("Player1")
                || _CurrentOther.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire
                && _CurrentOther.CompareTag("Player2"))
                {
                    if (_Picked == false && _CurrentDevice.RightTrigger.WasPressed)
                    {
                        foreach (var tag in _PickAbleTags)
                        {
                            if (_CurrentOther.CompareTag(tag))
                            {
                                _CurrentOther.GetComponent<PlayerController>()._Anim.SetTrigger("Grabbed");
                                _CurrentOther.GetComponent<PlayerController>().Grabbing = true;
                                _RB.detectCollisions = true;
                                _RB.useGravity = false;
                                Debug.Log($"{gameObject.name} was Picked");
                                _Picked = true;
                                transform.SetParent(_CurrentOther.GetComponent<PlayerController>().Grab);
                                _AS2.PlayOneShot(_CurrentOther.GetComponent<PlayerController>()._Grab[Random.Range(0, _CurrentOther.GetComponent<PlayerController>()._Grab.Length)], 0.4f);
                                _AS2.PlayOneShot(_CurrentOther.GetComponent<PlayerController>()._VoiceGrab[Random.Range(0, _CurrentOther.GetComponent<PlayerController>()._VoiceGrab.Length)], 1f);
                                if(_PickedUp != null)
                                {
                                    _AS2.PlayOneShot(_PickedUp);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (transform.parent != null)
        {
            if(transform.parent.GetComponentInParent<PlayerController>().transform.rotation == Quaternion.Euler(0f, 90f, 0f))
            {
                _IsFacingRight = true;
            }
            else if(transform.parent.GetComponentInParent<PlayerController>().transform.rotation == Quaternion.Euler(0f, 270f, 0f))
            {
                _IsFacingRight = false;
            }

            transform.localScale = _OriginalSize;
            if (transform.parent.GetComponentInParent<PlayerController>().Grab.transform.childCount >= 1)
            {
                transform.position = transform.parent.GetComponentInParent<PlayerController>().Grab.position;
                _RB.velocity = transform.parent.GetComponentInParent<Rigidbody>().velocity;
                _RB.angularVelocity = transform.parent.GetComponentInParent<Rigidbody>().angularVelocity;
                Physics.IgnoreCollision(transform.parent.GetComponentInParent<CapsuleCollider>(), this.GetComponent<Collider>(), true);
                Physics.IgnoreCollision(transform.parent.GetComponentInParent<BoxCollider>(), this.GetComponent<Collider>(), true);
                if (transform.parent.GetComponentInParent<PlayerController>()._Anim.GetCurrentAnimatorStateInfo(0).IsName("Grab_Movement")
                || transform.parent.GetComponentInParent<PlayerController>()._Anim.GetCurrentAnimatorStateInfo(0).IsName("Grabbing_Jumping"))
                {
                    if (_Picked == true && transform.parent.GetComponentInParent<PlayerController>().Device.RightTrigger.WasPressed && _TrackButtonPresses == 1)
                    {
                        if (_ThrowButtonIsPressed == false)
                        {
                            if (transform.GetComponentInParent<PlayerController>().CompareTag("Player1"))
                            {
                                StartCoroutine("ThrowPlayer1");
                            }
                            else if (transform.GetComponentInParent<PlayerController>().CompareTag("Player2"))
                            {
                                StartCoroutine("ThrowPlayer2");
                            }
                            _ThrowButtonIsPressed = true;
                        }
                    }
                }

                if (transform.parent.GetComponentInParent<PlayerController>().Device.RightTrigger.WasReleased)
                {
                    _TrackButtonPresses = 1;
                }
                // if (_Picked == true && transform.parent.GetComponentInParent<PlayerController>().Device.RightTrigger.WasPressed && _TrackButtonPresses == 1)
                // {
                //     if (transform.parent.GetComponentInParent<PlayerController>()._Anim.GetCurrentAnimatorStateInfo(0).IsName("Grab_Movement")
                //     || transform.parent.GetComponentInParent<PlayerController>()._Anim.GetCurrentAnimatorStateInfo(0).IsName("Grabbing_Jumping"))
                //     {
                //         Physics.IgnoreCollision(transform.parent.GetComponentInParent<CapsuleCollider>(), this.GetComponent<Collider>(), false);
                //         transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
                //         Debug.Log($"{gameObject.name} was Dropped because of the right trigger");
                //         _Picked = false;
                //         _RB.useGravity = true;
                //         _SoundCheckIfMoving = false;
                //         _RB.velocity = Vector3.zero;
                //         _RB.angularVelocity = Vector3.zero;
                //         transform.SetParent(null);
                //         _TrackButtonPresses = 0;
                //     }
                // }

                if (_RightTriggerImage.gameObject.activeInHierarchy == true)
                {
                    _RightTriggerImage.gameObject.SetActive(false);
                }

                if (_Picked == true)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                _Distance = Vector3.Distance(transform.position, transform.parent.GetComponentInParent<PlayerController>().Grab.position);
                if (_Distance >= 0.5f)
                {
                    Physics.IgnoreCollision(transform.parent.GetComponentInParent<CapsuleCollider>(), this.GetComponent<Collider>(), false);
                    transform.root.parent.GetComponent<PlayerController>().Grabbing = false;
                    Debug.Log($"{gameObject.name} was Dropped because of distance");
                    _Picked = false;
                    _RB.useGravity = true;
                    _RB.velocity = Vector3.zero;
                    _RB.angularVelocity = Vector3.zero;
                    transform.SetParent(null);
                    _TrackButtonPresses = 0;
                }

                if (_CurrentOther.CompareTag("Player2") && transform.parent.GetComponentInParent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke)
                {
                    Physics.IgnoreCollision(transform.parent.GetComponentInParent<CapsuleCollider>(), this.GetComponent<Collider>(), false);
                    transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
                    Debug.Log($"{gameObject.name} was Dropped because player is smoke");
                    _Picked = false;
                    _RB.useGravity = true;
                    _RB.velocity = Vector3.zero;
                    _RB.angularVelocity = Vector3.zero;
                    transform.SetParent(null);
                    _TrackButtonPresses = 0;
                }
                else if (_CurrentOther.CompareTag("Player1") && transform.parent.GetComponentInParent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
                {
                    Physics.IgnoreCollision(transform.parent.GetComponentInParent<CapsuleCollider>(), this.GetComponent<Collider>(), false);
                    transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
                    Debug.Log($"{gameObject.name} was Dropped because player is smoke");
                    _Picked = false;
                    _RB.useGravity = true;
                    _RB.velocity = Vector3.zero;
                    _RB.angularVelocity = Vector3.zero;
                    transform.SetParent(null);
                    _TrackButtonPresses = 0;
                }
            }
        }
    }

    IEnumerator ThrowPlayer1()
    {
        transform.parent.GetComponentInParent<PlayerController>()._Anim.SetTrigger("Throw");
        var _Collider = transform.parent.GetComponentInParent<CapsuleCollider>();
        var _Collider2 = transform.parent.GetComponentInParent<BoxCollider>();
        yield return new WaitForSeconds(transform.parent.GetComponentInParent<PlayerController>()._Anim.GetCurrentAnimatorStateInfo(0).length / 2f - 0.325f);
        if (transform.parent.GetComponentInParent<PlayerController>().transform.rotation == Quaternion.Euler(0f, 90f, 0f))
        {
            transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
            _Picked = false;
            _RB.useGravity = true;
            _RB.velocity = Vector3.zero;
            _RB.angularVelocity = Vector3.zero;
            _ThrowButtonIsPressed = false;
            _TrackButtonPresses = 0;
            _SoundCheckIfMoving = false;
            _CurrentOther = null;
            transform.position = transform.parent.GetComponentInParent<PlayerController>().ThrowTransform.position;
            //_RB.AddForce(new Vector3(30f, 30f, 0f) * _ThrowStrength);
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._Throw[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._Throw.Length)], 0.4f);
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow.Length)], 1f);
            _RB.velocity = new Vector3(11f, 11f, 0f);
            transform.SetParent(null);
            _CurrentDevice = null;
            yield return new WaitForSecondsRealtime(0.25f);
            Physics.IgnoreCollision(_Collider, this.GetComponent<Collider>(), false);
            Physics.IgnoreCollision(_Collider2, this.GetComponent<Collider>(), false);

        }
        else if (transform.parent.GetComponentInParent<PlayerController>().transform.rotation == Quaternion.Euler(0f, 270f, 0f))
        {
            transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
            _Picked = false;
            _RB.useGravity = true;
            _RB.velocity = Vector3.zero;
            _RB.angularVelocity = Vector3.zero;
            _ThrowButtonIsPressed = false;
            _TrackButtonPresses = 0;
            _CurrentOther = null;
            _SoundCheckIfMoving = false;
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._Throw[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._Throw.Length)], 0.4f);
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow.Length)], 1f);
            transform.position = transform.parent.GetComponentInParent<PlayerController>().ThrowTransform.position;
            _RB.velocity = new Vector3(-11f, 11f, 0f);
            transform.SetParent(null);
            _CurrentDevice = null;
            //_RB.AddForce(new Vector3(-30f, 30f, 0f) * _ThrowStrength);
            yield return new WaitForSecondsRealtime(0.25f);
            Physics.IgnoreCollision(_Collider, this.GetComponent<Collider>(), false);
            Physics.IgnoreCollision(_Collider2, this.GetComponent<Collider>(), false);
        }
        yield return null;
    }

    IEnumerator ThrowPlayer2()
    {
        transform.parent.GetComponentInParent<PlayerController>()._Anim.SetTrigger("Throw");
        var _Collider = transform.parent.GetComponentInParent<CapsuleCollider>();
        var _Collider2 = transform.parent.GetComponentInParent<BoxCollider>();
        yield return new WaitForSeconds(transform.parent.GetComponentInParent<PlayerController>()._Anim.GetCurrentAnimatorStateInfo(0).length / 2f - 0.3f);
        if (transform.parent.GetComponentInParent<PlayerController>().transform.rotation == Quaternion.Euler(0f, 90f, 0f))
        {
            transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
            _Picked = false;
            _RB.useGravity = true;
            _RB.velocity = Vector3.zero;
            _RB.angularVelocity = Vector3.zero;
            _ThrowButtonIsPressed = false;
            _TrackButtonPresses = 0;
            _SoundCheckIfMoving = false;
            _CurrentOther = null;
            transform.position = transform.parent.GetComponentInParent<PlayerController>().ThrowTransform.position;
            // _RB.AddForce(new Vector3(30f, 30f, 0f) * _ThrowStrength);
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._Throw[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._Throw.Length)], 0.4f);
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow.Length)], 1f);
            _RB.velocity = new Vector3(11f, 11f, 0f);
            transform.SetParent(null);
            _CurrentDevice = null;
            yield return new WaitForSecondsRealtime(0.25f);
            Physics.IgnoreCollision(_Collider, this.GetComponent<Collider>(), false);
            Physics.IgnoreCollision(_Collider2, this.GetComponent<Collider>(), false);
        }
        else if (transform.parent.GetComponentInParent<PlayerController>().transform.rotation == Quaternion.Euler(0f, 270f, 0f))
        {
            transform.parent.GetComponentInParent<PlayerController>().Grabbing = false;
            _Picked = false;
            _RB.useGravity = true;
            _ThrowButtonIsPressed = false;
            _RB.velocity = Vector3.zero;
            _RB.angularVelocity = Vector3.zero;
            _TrackButtonPresses = 0;
            _SoundCheckIfMoving = false;
            _CurrentOther = null;
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._Throw[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._Throw.Length)], 0.4f);
            _AS2.PlayOneShot(transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow[Random.Range(0, transform.parent.GetComponentInParent<PlayerController>()._VoiceThrow.Length)], 1f);
            // _RB.AddForce(new Vector3(-30f, 30f, 0f) * _ThrowStrength);
            transform.position = transform.parent.GetComponentInParent<PlayerController>().ThrowTransform.position;
            _RB.velocity = new Vector3(-11f, 11f, 0f);
            transform.SetParent(null);
            _CurrentDevice = null;
            yield return new WaitForSecondsRealtime(0.25f);
            Physics.IgnoreCollision(_Collider, this.GetComponent<Collider>(), false);
            Physics.IgnoreCollision(_Collider2, this.GetComponent<Collider>(), false);
        }
        yield return null;
    }
}
