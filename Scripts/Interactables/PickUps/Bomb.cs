using InControl;
using UnityEngine;
using ObjectPooling;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Bomb : MonoBehaviour
{
    [Header("Bomb")]
    public float _ThrowStrength = 25f;
    public float _Timer;
    public float _Speed = 5f;
    public float _RespawnTimer;
    public GameObject _Wick;
    public GameObject _SpiceGirl;
    public bool _Disabled;
    public AudioClip _BombIsActive;
    public Particle _Particle;
    [Header("Shake Settings")]
    public float _Duration = 0.15f;
    public float _Magnitude = 0.75f;

    private Rigidbody _RB;
    private bool _Picked;
    private InputDevice _CurrentDevice;
    private Transform _CurrentOther;
    private bool _IsSpiceGirl;
    private RespawnObjects _RespawnObjects;
    private Vector3 _Origin;
    private PlayerCamera _Cam;
    public AudioSource _AS;
    private bool _SoundPlaying = false;
    private bool _HasEntered;
    public List<BreakableWall> _Brownies = new List<BreakableWall>();
    private Animator _Anim;
    private Pickable _Pickable;
    public TextMeshProUGUI uiText;
    private bool DoOnce = false;
    private bool DoOnce2 = false;
    private bool DoOnce3 = false;

    private void OnEnable()
    {
        transform.position = _Origin;
        _CurrentDevice = null;
        _CurrentOther = null;
        if(_SpiceGirl == null) return;
        _SpiceGirl.GetComponent<PlayerController>().enabled = true;
    }

    private void Awake()
    {
        _Origin = transform.position;
        _RB = GetComponent<Rigidbody>();
        _Cam = GameObject.FindObjectOfType<PlayerCamera>();
        _Anim = GetComponent<Animator>();
        uiText.gameObject.SetActive(false);
        _Pickable = GetComponent<Pickable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if(playerController != null && other.CompareTag("Player2"))
        {
            _CurrentDevice = playerController.Device;
            _CurrentOther = other.transform;
            playerController._CurrentInteractable = this.gameObject;
            _IsSpiceGirl = true;
            _SpiceGirl = other.gameObject;
        }

        if(other.CompareTag("Breakable"))
        {
            _Brownies.Add(other.gameObject.GetComponent<BreakableWall>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if (playerController != null && _Picked == false && other.CompareTag("Player2"))
        {
            _CurrentDevice = null;
            _CurrentOther = null;
            _IsSpiceGirl = false;
            _SpiceGirl = other.gameObject;
        }

        if(other.CompareTag("Breakable"))
        {
            _Brownies.Remove(other.gameObject.GetComponent<BreakableWall>());
        }
    }

    private void Update() 
    {
        if(_RespawnObjects == null)
        {
            _RespawnObjects = GameObject.FindObjectOfType<RespawnObjects>();
        }

        if(_Timer >= 3f)
        {
            foreach (BreakableWall wall in _Brownies)
            {
                wall.gameObject.SetActive(false); 
            }
            DisableCharacter();
            _Timer = 0f;
        }

        CountdownText();

        if (_CurrentDevice == null) return;

        if(_CurrentOther.GetComponent<PlayerController>()._CurrentInteractable == this.gameObject 
        && _CurrentOther.GetComponent<PlayerController>().Grab.transform.childCount == 0)
        {
            if (_Picked == false && _CurrentDevice.RightTrigger.WasPressed)
            {
                if(_IsSpiceGirl == true)
                {
                    if(_CurrentOther.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire && _CurrentOther.CompareTag("Player2"))
                    {
                        _Picked = true;
                        _Wick.SetActive(true);
                        _Anim.SetTrigger("Activated");
                        _Pickable._RightTriggerImage.gameObject.SetActive(false);
                        _SpiceGirl.gameObject.SetActive(false);
                    }
                }
            }
        }

        if(_Wick.activeInHierarchy == true)
        {
            if(_SoundPlaying == false)
            {
                _AS.PlayOneShot(_BombIsActive, 1f);
                _SoundPlaying = true;
            }
            uiText.gameObject.SetActive(true);
            _Timer += Time.deltaTime;
        }

        if(_Timer >= 3.05f)
        {
            DisableCharacter();
            _Timer = 0f;
        }
    }

    private void CountdownText()
    {

        if(_Timer >= 0f && DoOnce == false)
        {
            uiText.text = "3";
            Debug.Log("Set to 3");
            DoOnce = true;
        }
        if(_Timer >= 1f && DoOnce2 == false)
        {
            uiText.text = "2";
            Debug.Log("Set to 2");
            DoOnce2 = true;
        }
        if(_Timer >= 2f && DoOnce3 == false)
        {
            uiText.text = "1";
            Debug.Log("Set to 1");
            DoOnce3 = true;
        }
        if(_Timer >= 2.95f)
        {
            DoOnce = false;
            DoOnce2 = false;
            DoOnce3 = false;
        }
    }

    private void DisableCharacter()
    {
        var clone = PoolManager.GetObjectFromPool(_Particle.gameObject);
        clone.transform.position = transform.position;
        clone.gameObject.SetActive(true);
        DontDestroyOnLoad(clone);
        _SpiceGirl.gameObject.SetActive(true);
        _Wick.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        uiText.gameObject.SetActive(false);
        _SpiceGirl.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Fire;
        _SpiceGirl.gameObject.transform.position = this.gameObject.transform.position;
    }

    private void OnDisable()
    {
        var clone = PoolManager.GetObjectFromPool(_Particle.gameObject);
        clone.transform.position = transform.position;
        clone.gameObject.SetActive(true);
        DontDestroyOnLoad(clone);
        uiText.gameObject.SetActive(false);
        _SoundPlaying = false;
        _Picked = false;
        _RespawnObjects.ReEnableGameObject(this.gameObject);
        _Wick.gameObject.SetActive(false);
        _Timer = 0f;
        if(_SpiceGirl != null)
        {
            _SpiceGirl.transform.position = this.gameObject.transform.position;
            _SpiceGirl.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Fire;
            _SpiceGirl.GetComponent<PlayerController>().enabled = false;
        }
        if(_Cam != null)
        {
            _Cam.DoShake(_Duration, _Magnitude);
        }
    }
}
