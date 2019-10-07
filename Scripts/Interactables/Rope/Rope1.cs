using InControl;
using UnityEngine;
using UnityEngine.UI;
using ObjectPooling;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Rope1 : MonoBehaviour
{
    [Header("Rope")]
    [SerializeField]
    private GameObject _Flame;
    public AudioClip[] _Burning;
    public Image image;
    public Particle _BurntParticle;
    public float _DistanceFromPoint = 1f;
    public Transform EndPoint;

    private AudioSource _AS;
    private InputDevice _CurrentDevice;
    private Transform _CurrentOther;
    private bool _Picked;
    private bool _IsSpiceGirl = false;
    private GameObject _SpiceGirl;
    private Vector3 _RopeOrigin;
    private Vector3 _Origin;
    private RespawnObjects _RespawnObjects;
    public Animator _Anim;

    private void OnEnable()
    {
        _RespawnObjects = GameObject.FindObjectOfType<RespawnObjects>();
        _CurrentOther = null;
        _CurrentDevice = null;
    }

    private void Awake()
    {
        _RopeOrigin = transform.position;
        _Origin = _Flame.transform.position;
        _RespawnObjects = GetComponent<RespawnObjects>();
        _AS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();


        if(other.CompareTag("Player2"))
        {
            if (playerController != null)
            {
                _CurrentDevice = playerController.Device;
                _CurrentOther = other.transform;
                playerController._CurrentInteractable = this.gameObject;
            }
            _IsSpiceGirl = true;
            _SpiceGirl = other.gameObject;

            if(image == null) return;
            if(image.gameObject.activeInHierarchy == false)
            {
                image.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if(other.CompareTag("Player2"))
        {
            if (playerController != null)
            {
                _CurrentDevice = null;
                _CurrentOther = null;
            }
            _IsSpiceGirl = false;
            _SpiceGirl = other.gameObject;
            playerController._CurrentInteractable = null;

            if(image == null) return;
            if(image.gameObject.activeInHierarchy == true)
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    private void Update() 
    {
        if (_CurrentDevice == null) return;

        // if(_Flame.activeInHierarchy == true)
        // {
        //     MovePossessedPlayer();
        // }
        
        if(_CurrentOther.gameObject.GetComponent<PlayerController>().Grab.transform.childCount == 0 
        && _CurrentOther.gameObject.GetComponent<PlayerController>()._CurrentInteractable == this.gameObject)
        {
            if (_Picked == false && _CurrentDevice.RightTrigger.WasPressed)
            {
                Debug.Log("Right trigger pressed");
                if (_IsSpiceGirl == true)
                {
                    if(_SpiceGirl.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire)
                    {
                        StartCoroutine("PlayAnimation");
                        _Picked = true;
                        _Flame.SetActive(true);
                        _SpiceGirl.gameObject.SetActive(false);
                        // Debug.Log($"{gameObject.name} was Picked");
                        // if(RopeBurning == null) return;
                        // RopeBurning.SetActive(true);
                    }
                }
            }
        }
    }

    // private void MovePossessedPlayer()
    // {
    //     if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Flame.transform.position) <= _PointRadius)
    //     {
    //         _Current++;
    //         if(_Current >= _MoveLocation.Length)
    //         {
    //             if(_Flame.activeInHierarchy == true)
    //             {   
    //                 this.gameObject.SetActive(false);
    //             }
    //         }
    //     }
    //     _Flame.transform.position = Vector3.MoveTowards(_Flame.transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
    // }

    IEnumerator PlayAnimation()
    {
        _Anim.SetBool("BurningToptoBottom", true);
        _AS.PlayOneShot(_Burning[Random.Range(0, _Burning.Length)]);
        yield return new WaitForSeconds(_Anim.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        _SpiceGirl.gameObject.SetActive(true);
        _Flame.gameObject.SetActive(false);
        if(_CurrentDevice.Direction.X >= 0)
        {
            _SpiceGirl.gameObject.transform.position = EndPoint.transform.position + Vector3.right;
        }
        else if(_CurrentDevice.Direction.X <= 0)
        {
            _SpiceGirl.gameObject.transform.position = EndPoint.transform.position + Vector3.left;
        }
        var clone = PoolManager.GetObjectFromPool(_BurntParticle.gameObject);
        clone.transform.position = EndPoint.transform.position;
        clone.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
        yield return null;
    }

    private void OnDisable()
    {
        _Picked = false;
        _Anim.SetBool("BurningToptoBottom", false);
        _Flame.transform.position = _Origin;
        _RespawnObjects.ReEnableGameObject(this.transform.parent.gameObject);
        transform.position = _RopeOrigin;
    }
}
