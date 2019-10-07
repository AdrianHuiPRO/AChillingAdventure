using UnityEngine;
using ObjectPooling;

[RequireComponent(typeof(Rigidbody))]
public class Salty_Projectile : MonoBehaviour
{
    public float _RecoilStrength = 5f;
    public float _ProjectileSpeed = 1f;
    public bool _FacingRight = false;
    public float _Duration = 0.2f;
    public float _Magnitude = 0.5f;
    public AudioClip _SaltExplosion;
    [SerializeField]
    private Particle _Particle;
    private Rigidbody _RB;
    private PlayerCamera _Cam;

    private void Awake()
    {
        _Cam = GameObject.FindObjectOfType<PlayerCamera>();
        _RB = GetComponent<Rigidbody>();
        if(_Cam == null) return;
        _Cam._Explosion = _SaltExplosion;
    }

    private void Update()
    {
        if(_Cam == null)
        {
            _Cam = GameObject.FindObjectOfType<PlayerCamera>();
        }
        else
        {
            _Cam = GetComponent<PlayerCamera>();
        }

        if(_FacingRight == true)
        {
            transform.position += Vector3.right * _ProjectileSpeed * Time.deltaTime;
        }
        else if(_FacingRight == false)
        {
            transform.position += Vector3.left * _ProjectileSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        this.gameObject.SetActive(false);
        if(other.gameObject.GetComponent<Rigidbody>() == null) return;

        if(_FacingRight == true)
        {
            if(other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
            {
                other.gameObject.GetComponent<PlayerController>().enabled = false;
                _Cam.StartCoroutine(_Cam.Shake(_Duration, _Magnitude));
                if(other.gameObject.CompareTag("Player1"))
                {
                    if(other.gameObject.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Solid)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedVoice.Length)]);
                    }
                    else if(other.gameObject.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedVoice.Length)]);
                    }
                }
                else if(other.gameObject.CompareTag("Player2"))
                {
                    if(other.gameObject.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice.Length)]);
                    }
                    else if(other.gameObject.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice.Length)]);
                    }
                }
                other.gameObject.GetComponent<Rigidbody>().AddForce(10f * _RecoilStrength, 10f, 0f);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(10f * _RecoilStrength, 10f, 0f);
            }
        }
        else
        {
            if(other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
            {
                other.gameObject.GetComponent<PlayerController>().enabled = false;
                _Cam.StartCoroutine(_Cam.Shake(_Duration, _Magnitude));
                if(other.gameObject.CompareTag("Player1"))
                {
                    if(other.gameObject.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Solid)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedVoice.Length)]);
                    }
                    else if(other.gameObject.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedVoice.Length)]);
                    }
                }
                else if(other.gameObject.CompareTag("Player2"))
                {
                    if(other.gameObject.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Fire)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice.Length)]);
                    }
                    else if(other.gameObject.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke)
                    {
                        other.gameObject.GetComponent<PlayerController>()._AudioSource2.PlayOneShot(other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice[Random.Range(0, other.gameObject.GetComponent<PlayerController>()._DamagedSecondStateVoice.Length)]);
                    }
                }
                other.gameObject.GetComponent<Rigidbody>().AddForce(-10f * _RecoilStrength, 10f, 0f);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(-10f * _RecoilStrength, 10f, 0f);
            }
        }
    }

    private void OnDisable()
    {
        var clone = PoolManager.GetObjectFromPool(_Particle.gameObject);
        clone.transform.position = transform.position;
        clone.gameObject.SetActive(true);
        DontDestroyOnLoad(clone);
        _RB.velocity = Vector3.zero;
    }
}
