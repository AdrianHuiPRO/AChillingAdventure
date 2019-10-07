using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WinPoint : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _Win;
    [SerializeField]
    private UnityEvent _Player1InWinPoint;
    [SerializeField]
    private UnityEvent _Player2InWinPoint;
    [SerializeField]
    private UnityEvent _Player1NotInWinPoint;
    [SerializeField]
    private UnityEvent _Player2NotInWinPoint;

    private PlayerController _Icebert;
    private PlayerController _SpiceGirl;
    private PlayerCamera _Cam;
    public Camera _NewCameraPosition;

    private bool _P1InTrigger = false;
    private bool _P2InTrigger = false;
    private float _ChangeSize;
    public float _SmoothTime = 0.15f;
    private bool _BothPlayersIn = false;
    private AudioSource _AS;
    public AudioSource _BackgroundMusicAS;
    public AudioClip[] _WinSound;
    public AudioClip _VictoryMusic;
    public GameObject _Hit;
    private bool DoOnce;

    public Animator _Anim;
    public Animator _LoadingScreen;

    private void Awake()
    {
        _Cam = Camera.main.GetComponent<PlayerCamera>();
        _ChangeSize = _NewCameraPosition.GetComponent<Camera>().orthographicSize;
        _NewCameraPosition.gameObject.SetActive(false);
        _AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_P1InTrigger == true && _P2InTrigger == true)
        {
            StartCoroutine("ArrivedAtWinPoint");          
            _Cam.transform.position = Vector3.Lerp(_Cam.transform.position, _NewCameraPosition.transform.position, Time.fixedDeltaTime * _SmoothTime);
            _BothPlayersIn = true;
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player1"))
        {
            _P1InTrigger = true;
            _Icebert = other.GetComponent<PlayerController>();
            _Anim.SetBool("IcebertInTrigger", true);
            _Player1InWinPoint.Invoke();
            // if(_WinSound != null)
            // {
            //     _AS.clip = _WinSound[Random.Range(0, _WinSound.Length)];
            //     _AS.Play();
            // }
        }
        
        if(other.CompareTag("Player2"))
        {
            _P2InTrigger = true;
            _SpiceGirl = other.GetComponent<PlayerController>();
            _Player2InWinPoint.Invoke();
            _Anim.SetBool("SpicegirlInTrigger", true);
            // if(_WinSound != null)
            // {
            //     _AS.clip = _WinSound[Random.Range(0, _WinSound.Length)];
            //     _AS.Play();
            // }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_BothPlayersIn == false)
        {
            if(other.CompareTag("Player1"))
            {
                _P1InTrigger = false;
                _Player1NotInWinPoint.Invoke();
                _Anim.SetBool("IcebertInTrigger", false);
            }
            
            if(other.CompareTag("Player2"))
            {
                _P2InTrigger = false;
                _Player2NotInWinPoint.Invoke();
                _Anim.SetBool("SpicegirlInTrigger", false);
            }
        }
    }

    IEnumerator ArrivedAtWinPoint()
    {
        _Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(_Cam.GetComponent<Camera>().orthographicSize, _ChangeSize, Time.fixedDeltaTime * _SmoothTime);
        _Cam._IsStatic = true;
        _Icebert.gameObject.GetComponent<PlayerController>().enabled = false;
        _SpiceGirl.gameObject.GetComponent<PlayerController>().enabled = false;
        _Icebert.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        _SpiceGirl.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        _Icebert.gameObject.GetComponent<AudioSource>().enabled = false;
        _SpiceGirl.gameObject.GetComponent<AudioSource>().enabled = false;
        _Hit.SetActive(true);
        if(_VictoryMusic != null)
        {
            if(DoOnce == false)
            {
                DoOnce = true;
                _BackgroundMusicAS.clip = _VictoryMusic;
                _BackgroundMusicAS.loop = false;
                _BackgroundMusicAS.Play();
            }
        }
        yield return new WaitForSeconds(3f);
        _LoadingScreen.SetTrigger("NextLevel");
        yield return null;
    }
}
