using UnityEngine;
using ObjectPooling;
using InControl;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Pipe : MonoBehaviour
{
    [Header("Joints")]
    [SerializeField]
    private GameObject[] _MoveLocation;

    [Header("Pipe")]
    [SerializeField]
    private GameObject _Water;
    public float _Speed = 5f;

    public int _Current = 0;
    public float _PointRadius = 0.005f;
    public GameObject _Icebert;
    public AudioClip[] _PipeSounds;
    public Particle _SuckedInParticle;
    public Particle _SpatOutParticle;
    public Image _RightTriggerImage;
    private bool _Return = false;
    private bool _SoundPlaying;
    private AudioSource _AS;
    private InputDevice _CurrentDevice;
    private Transform _CurrentOther;
    private bool _IsIceBert;
    private bool _Picked;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if(other.CompareTag("Player1"))
        {
            if (playerController != null)
            {
                _CurrentDevice = playerController.Device;
                _CurrentOther = other.transform;
                playerController._CurrentInteractable = this.gameObject;
            }
            _IsIceBert = true;
            _Icebert = other.gameObject;

            if(_RightTriggerImage == null) return;
            if(_RightTriggerImage.gameObject.activeInHierarchy == false)
            {
                _RightTriggerImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if(other.CompareTag("Player1"))
        {
            if (playerController != null && _Picked == false)
            {
                _CurrentDevice = null;
                _CurrentOther = null;
            }
            _IsIceBert = false;
            _Icebert = other.gameObject;

            if(_RightTriggerImage == null) return;
            if(_RightTriggerImage.gameObject.activeInHierarchy == true)
            {
                _RightTriggerImage.gameObject.SetActive(false);
            }
        }
    }

    private void Update() 
    {
        if(_CurrentDevice == null) return;

        if(_Water.activeInHierarchy == true)
        {
            MovePossessedPlayer();
        }

        if(_CurrentOther.GetComponent<PlayerController>()._CurrentInteractable == null) return;

        if(_CurrentOther.GetComponent<PlayerController>()._CurrentInteractable == this.gameObject 
        && _CurrentOther.GetComponent<PlayerController>().Grab.transform.childCount == 0)
        {
            if(_Picked == false && _Icebert.GetComponent<PlayerController>().Device.RightTrigger.WasPressed)
            {
                if(_IsIceBert == true)
                {
                    Debug.Log("Icebert player has pressed right trigger");
                    if(_Icebert.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
                    {
                        _Picked = true;
                        _Water.transform.position = _Icebert.transform.position;
                        _Icebert.SetActive(false);
                        _Water.SetActive(true);
                        _RightTriggerImage.gameObject.SetActive(false);
                        if(_SoundPlaying == false)
                        {
                            _AS.clip = _PipeSounds[Random.Range(0,_PipeSounds.Length)];
                            _AS.loop = true;
                            _AS.Play();
                            _SoundPlaying = true;
                        }
                        // var clone = PoolManager.GetObjectFromPool(_SuckedInParticle.gameObject);
                        // clone.transform.position = transform.position;
                        // clone.gameObject.SetActive(true);
                    }
                }

                if(_RightTriggerImage == null) return;
                if(_RightTriggerImage.gameObject.activeInHierarchy == true)
                {
                    _RightTriggerImage.gameObject.SetActive(false);
                }
            }
        }
    }

    private void MovePossessedPlayer()
    {
        if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Water.transform.position) <= _PointRadius)
        {
            _Current++;
            if(_Current >= _MoveLocation.Length)
            {
                if(_Water.activeInHierarchy == true)
                {                
                    _Icebert.SetActive(true);
                    _Water.SetActive(false);
                    _Picked = false;
                    _Icebert.gameObject.transform.position = _Water.transform.position;
                    _Water.transform.position = _MoveLocation[0].transform.position;
                    _Current = 0;
                    _CurrentDevice = null;
                    // var clone1 = PoolManager.GetObjectFromPool(_SpatOutParticle.gameObject);
                    // clone1.transform.position = _Water.transform.position;
                    // clone1.gameObject.SetActive(true);
                }
                _AS.loop = false;
                _AS.Stop();
                _SoundPlaying = false;
            }
        }
        _Water.transform.position = Vector3.MoveTowards(_Water.transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
    }
}