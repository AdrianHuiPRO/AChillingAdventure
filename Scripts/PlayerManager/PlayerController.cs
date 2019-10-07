using UnityEngine;
using InControl;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Solid/Fire")]
    [SerializeField]
    public float _Speed = 10f;
    [SerializeField]
    private float _JumpHeight = 20f;
    [SerializeField]
    private float _AirSpeed = 5f;

    [Header("Liquid")]
    [SerializeField]
    private float _LiquidSpeed = 10f;
    [SerializeField]
    private float _LiquidAirSpeed = 5f;

    [Header("Smoke")]
    [SerializeField]
    private float _SmokeSpeed = 10f;
    [SerializeField]
    private float _Float = 5f;

    [Header("Grab")]
    public Transform Grab;
    public Transform ThrowTransform;
    private bool _Player1Interacted;
    private bool _Player2Interacted;

    [Header("Audio")]
    public AudioClip[] _Jump;
    public AudioClip[] _VoiceJump;
    public AudioClip[] _Land;
    public AudioClip[] _Grab;
    public AudioClip[] _VoiceGrab;
    public AudioClip[] _Throw;
    public AudioClip[] _VoiceThrow;
    public AudioClip[] _DamagedVoice;
    public AudioClip[] _DamagedSecondStateVoice;
    public AudioClip[] _Death;
    public AudioClip[] _DeathSecondState;
    public AudioClip[] _Respawn;
    public AudioClip[] _RespawnSecondState;
    public AudioClip _DefaultState;
    public AudioClip _SecondState;

    [Header("Waterbert Audio")]
    public AudioClip[] _WaterLand;
    public AudioClip _WaterMove;

    [Header("Meshes, Materials, Animator")]
    public Animator _Anim;
    public Animator _Anim2;
    public GameObject _State1;
    public GameObject _State2;

    private Rigidbody _Rb;
    public GameObject _CurrentInteractable;

    public LayerMask GroundBlockingLayers;
    public LayerMask ObjectsLayer;
    
    [SerializeField]
    private Collider _PhysicsCollider;

    [SerializeField]
    private float _DistanceGround = 3f;
    private bool _IsGrounded = false;
    private bool _SoundPlayed = false;
    private bool _Jumped = false;
    private float TimerForJump;

    [HideInInspector]public bool Grabbing = false;

    public InputDevice Device { get; set; }

    [HideInInspector]public enum Player1State { Solid, Liquid }
    [HideInInspector]public enum Player2State { Fire, Smoke }
    [HideInInspector]public Player1State _Player1CurrentState;
    [HideInInspector]public Player2State _Player2CurrentState;
    [HideInInspector]public AudioSource _AudioSource;

    private CapsuleCollider _CC;

    private Renderer _Rend;
    private Vector3 _LeftGroundCheck;
    private Vector3 _RightGroundCheck;

    private float _OriginalCapsuleHeight;
    private float verticalMovement;
    private bool _OnSlope;

    Vector3 lastDirection;
    private bool _SoundCheckWaterMoving = false;
    private bool _StartJumpTimer = false;
    private bool _SoundCheckIdle;
    
    public AudioSource _AudioSource2;

    [SerializeField]
    private GameObject _SecondStateObject;

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody>();
        _Rend = GetComponentInChildren<Renderer>();
        _AudioSource = GetComponent<AudioSource>();
        _CC = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        if (this.gameObject.CompareTag("Player1"))
        {
            _Player1CurrentState = Player1State.Solid;
        }
        else if (this.gameObject.CompareTag("Player2"))
        {
            _Player2CurrentState = Player2State.Fire;
        }
        _OriginalCapsuleHeight = _CC.height;
    }

    // private void CheckRespawnSound()
    // {
    //     if(this.gameObject.CompareTag("Player1"))
    //     {
    //         if(_Player1CurrentState == Player1State.Solid)
    //         {
    //             _AudioSource.PlayOneShot(_Respawn[Random.Range(0, _Respawn.Length)]);
    //         }
    //         else if(_Player1CurrentState == Player1State.Liquid)
    //         {
    //             _AudioSource.PlayOneShot(_RespawnSecondState[Random.Range(0, _RespawnSecondState.Length)]);
    //         }
    //     }
    //     else if(this.gameObject.CompareTag("Player2"))
    //     {
    //         if(_Player2CurrentState == Player2State.Fire)
    //         {
    //             _AudioSource.PlayOneShot(_Respawn[Random.Range(0, _Respawn.Length)]);
    //         }
    //         else if(_Player2CurrentState == Player2State.Smoke)
    //         {
    //             _AudioSource.PlayOneShot(_RespawnSecondState[Random.Range(0, _RespawnSecondState.Length)]);
    //         }
    //     }
    // }

    // private void OnEnable()
    // {
    //     CheckRespawnSound();
    // }

    // private void CheckState()
    // {
    //     if(IsGrounded() && _Rb.velocity.x <= 1f && _Rb.velocity.x >= -1f)
    //     {  
    //         if(this.gameObject.CompareTag("Player1"))
    //         {
    //             if(_Player1CurrentState == Player1State.Solid)
    //             {
    //                 if(_SoundCheckIdle == false)
    //                 {
    //                     _AudioSource2.loop = true;
    //                     _AudioSource2.clip = _DefaultState;
    //                     _AudioSource2.Play();
    //                     _SoundCheckIdle = true;
    //                 }
    //             }
    //             else if(_Player1CurrentState == Player1State.Liquid)
    //             {
    //                 if(_SoundCheckIdle == false)
    //                 {
    //                     _AudioSource2.loop = true;
    //                     _AudioSource2.clip = _SecondState;
    //                     _AudioSource2.Play();
    //                     _SoundCheckIdle = true;
    //                 }
    //             }
    //         }
    //         if(this.gameObject.CompareTag("Player2"))
    //         {
    //             if(_Player2CurrentState == Player2State.Fire)
    //             {
    //                 if(_SoundCheckIdle == false)
    //                 {
    //                     _AudioSource2.loop = true;
    //                     _AudioSource2.clip = _DefaultState;
    //                     _AudioSource2.Play();
    //                     _SoundCheckIdle = true;
    //                 }
    //             }
    //             else if(_Player2CurrentState == Player2State.Smoke)
    //             {
    //                 if(_SoundCheckIdle == false)
    //                 {
    //                     _AudioSource2.loop = true;
    //                     _AudioSource2.clip = _SecondState;
    //                     _AudioSource2.Play();
    //                     _SoundCheckIdle = true;
    //                 }
    //             }
    //         }
    //     }
    //     else
    //     {
    //         _AudioSource2.loop = false;
    //         _AudioSource2.clip = null;
    //         _AudioSource2.Stop();
    //         _SoundCheckIdle = false;
    //     }
    // }

    private void Update()
    {

        // if(IsGrounded())
        // {
        //     CheckState();
        // }

        if(_Jumped == true)
        {
            TimerForJump += Time.deltaTime;
        }
        
        if(TimerForJump >= 0.25f && IsGrounded() == true)
        {
            _Jumped = false;
            TimerForJump = 0f;
        }

        if(Grabbing == true)
        {
            _Anim.SetBool("Grabbing", true);
        }
        else if(Grabbing == false)
        {
            _Anim.SetBool("Grabbing", false);
        }
        
        if (Device == null)
        {
            return;
        }
        else
        {
            if (IsGrounded())
            {
                if (_Player1CurrentState == Player1State.Solid)
                {
                    Player1Movement(Player1State.Solid);
                }
                else if (_Player1CurrentState == Player1State.Liquid)
                {
                    Player1Movement(Player1State.Liquid);
                }

                if (_Player2CurrentState == Player2State.Fire)
                {
                    Player2Movement(Player2State.Fire);
                }
            }
            else
            {
                if (_Player1CurrentState == Player1State.Solid)
                {
                    Player1AirMovement(Player1State.Solid);
                }
                else if (_Player1CurrentState == Player1State.Liquid)
                {
                    Player1AirMovement(Player1State.Liquid);
                }

                if (_Player2CurrentState == Player2State.Fire)
                {
                    Player2AirMovement(Player2State.Fire);
                }
            }
        }
        PlayLandSound();
        PlayWaterLandSound();
        ChangeMeshandCollider();
    }

    private void PlayLandSound()
    {
        if(_SoundPlayed == false && IsGrounded() == true && _Player1CurrentState != Player1State.Liquid)
        {  
            Debug.Log("land played");
            _AudioSource.PlayOneShot(_Land[Random.Range(0, _Land.Length)]);
            _SoundPlayed = true;  
        }
        else if(_SoundPlayed == true && IsGrounded() == false && _Player1CurrentState != Player1State.Liquid)
        {
            _SoundPlayed = false;
        }
    }

    private void PlayWaterLandSound()
    {
        if(_SoundPlayed == false && IsWaterGrounded() == true && _Player1CurrentState == Player1State.Liquid)
        {  
            Debug.Log("water land played");
            _AudioSource2.PlayOneShot(_WaterLand[Random.Range(0, _WaterLand.Length)]);
            _SoundPlayed = true;  
        }
        else if(_SoundPlayed == true && IsWaterGrounded() == false && _Player1CurrentState == Player1State.Liquid)
        {
            _SoundPlayed = false;
        }
    }

    bool IsGrounded()
    {
		Vector3 size = Vector3.one*0.6f;
		Vector3 center = transform.position;
		RaycastHit hit = new RaycastHit();

        if (Physics.BoxCast(center + Vector3.up, new Vector3(size.x - 0.2f, size.y - 0.1f, size.z), Vector3.down, out hit, Quaternion.identity, _DistanceGround, GroundBlockingLayers, QueryTriggerInteraction.Ignore))
        {
            //hit ground
            _IsGrounded = true;
            _Anim.SetBool("IsGrounded", true);
            return true;
        }
        else
        {
		    //missed ground
            _StartJumpTimer = true;
            _IsGrounded = false;
            _Anim.SetBool("IsGrounded", false);
            return false;
        }
    }

    bool IsWaterGrounded()
    {
		Vector3 size = Vector3.one;
        Vector3 center = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
		RaycastHit hit = new RaycastHit();

        if (Physics.BoxCast(center, size, Vector3.down, out hit, Quaternion.identity, _DistanceGround, GroundBlockingLayers, QueryTriggerInteraction.Ignore))
        {
            //hit ground 
            return true;
        }
		//missed ground
		return false;
    }

    public float slopeThreshold = 0.75f;
    private bool _IsMoving;

    private void CollidedWithWall()
    {
        bool IsPushing = false;
        Vector3 horizontalMove = _Rb.velocity;
        horizontalMove.y = 0;
        float distance = horizontalMove.magnitude * Time.fixedDeltaTime;
        horizontalMove.Normalize();
        RaycastHit hit;
        BoxCollider TriggerCollider = GetComponent<BoxCollider>();

        if(Device == null) return;
        if(this.gameObject.CompareTag("Player1") || this.gameObject.CompareTag("Player2") && _Player2CurrentState == Player2State.Fire)
        {
            if(_Rb.SweepTest(horizontalMove, out hit, distance, QueryTriggerInteraction.Ignore))
            {
                if(this.gameObject.GetComponent<CapsuleCollider>().isTrigger == false)
                {
                    CapsuleCollider _ThisCollider = GetComponent<CapsuleCollider>();
                    if(hit.transform.CompareTag("Untagged") && IsGrounded() == true && _Jumped == false)
                    {
                        Debug.Log("1");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.CompareTag("Untagged") && IsGrounded() == true && _Jumped == true)
                    {
                        Debug.Log("2");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.CompareTag("Untagged") && IsGrounded() == false && _Jumped == true)
                    {
                        Debug.Log("3");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.CompareTag("Untagged") && IsGrounded() == false && _Jumped == false)
                    {
                        Debug.Log("4");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.CompareTag("Slope") && IsGrounded() == true && _Jumped == false)
                    {
                        Debug.Log("5");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = _Rb.velocity + new Vector3(0.5f, 0.5f, 0.5f);
                    }
                    else if(hit.transform.gameObject.CompareTag("Bomb") && IsGrounded() == true && _Jumped == false)
                    {
                        Debug.Log("6");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = _Rb.velocity;
                    }
                    else if(hit.transform.gameObject.CompareTag("Bomb") && IsGrounded() == true && _Jumped == true)
                    {
                        Debug.Log("7");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.gameObject.CompareTag("Bomb") && IsGrounded() == false && _Jumped == false)
                    {
                        Debug.Log("8");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.gameObject.CompareTag("Bomb") && IsGrounded() == false && _Jumped == true)
                    {
                        Debug.Log("9");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.gameObject.CompareTag("PushableBlock") && IsGrounded() == true && _Jumped == false)
                    {
                        Debug.Log("Block 6");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = _Rb.velocity;
                    }
                    else if(hit.transform.gameObject.CompareTag("PushableBlock") && IsGrounded() == true && _Jumped == true)
                    {
                        Debug.Log("Block 7");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.gameObject.CompareTag("PushableBlock") && IsGrounded() == false && _Jumped == false)
                    {
                        Debug.Log("Block 8");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.gameObject.CompareTag("PushableBlock") && IsGrounded() == false && _Jumped == true)
                    {
                        Debug.Log("Block 9");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(hit.transform.gameObject.CompareTag("Player1") || hit.transform.gameObject.CompareTag("Player2"))
                    {
                        Debug.Log("10");
                        TriggerCollider.enabled = false;
                        IsPushing = true;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(IsGrounded() == false && _Jumped == false)
                    {
                        Debug.Log("11");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(IsGrounded() == true && _Jumped == true)
                    {
                        Debug.Log("12");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(IsGrounded() == true && _Jumped == false)
                    {
                        Debug.Log("13");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else if(IsGrounded() == false && _Jumped == true)
                    {
                        Debug.Log("14");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = new Vector3(0f, _Rb.velocity.y);
                    }
                    else
                    {
                        Debug.Log("15");
                        TriggerCollider.enabled = false;
                        IsPushing = false;
                        _Rb.velocity = _Rb.velocity;
                    }
                }
            }
        }

        if(IsPushing == true && Device.Direction.X > 0 || IsPushing == true && Device.Direction.X < 0)
        {
            _Anim.SetBool("IsPushing", true);
        }
        else if(IsPushing == false && Device.Direction.X == 0)
        {
            _Anim.SetBool("IsPushing", false);
        }

        if(IsPushing == true && Device.Direction.X > 0 || IsPushing == true && Device.Direction.X < 0 && _Player1CurrentState == Player1State.Liquid)
        {
            _Anim2.SetBool("IsPushing", true);
        }
        else if(IsPushing == false && Device.Direction.X == 0 && _Player1CurrentState == Player1State.Liquid)
        {
            _Anim2.SetBool("IsPushing", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.right);
    }

    void FixedUpdate()
    {
        if(_Anim == null)
        {
            _Anim = GetComponentInChildren<Animator>();
        }
        CollidedWithWall();
    }

    private void ChangeMeshandCollider()
    {
        if(this.CompareTag("Player1"))
        {
            if(_Player1CurrentState == Player1State.Liquid)
            {
                _CC.center = new Vector3(0f, -0.14f, 0f);
                _CC.height = 1f;
                _State1.SetActive(false);
                _State2.SetActive(true);
            }
            else if(_Player1CurrentState == Player1State.Solid)
            {
                _CC.center = new Vector3(0f, 0f, 0f);
                _CC.height = _OriginalCapsuleHeight;
                _State1.SetActive(true);
                _State2.SetActive(false);
            }
        }

        if (this.CompareTag("Player2"))
        {
            if (_Player2CurrentState == Player2State.Smoke)
            {
                _State1.SetActive(false);
                _State2.SetActive(true);
                _Rb.velocity = Vector3.up * _Float;
                this.gameObject.layer = 15;

                if (_IsGrounded == false || _IsGrounded == true)
                {
                    _Rb.velocity = new Vector3(Device.Direction.X * _SmokeSpeed, _Rb.velocity.y);
                }
            }
            else if(_Player2CurrentState == Player2State.Fire)
            {
                _State1.SetActive(true);
                _State2.SetActive(false);
                this.gameObject.layer = 11;
            }
        }
    }

    public void Player1Movement(Player1State p1)
    {
        if (this.gameObject.tag == "Player1")
        {
            if (p1 == Player1State.Solid)
            {
                _AudioSource.loop = false;
                _SoundCheckWaterMoving = false;
                //walking into a wall
                //posiiton at wall
                _Rb.velocity = new Vector3(Device.Direction.X * _Speed, _Rb.velocity.y);

                if (Device.Direction.X >= 0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }

                if(Grabbing == false)
                {
                    if(_IsGrounded == true && Device.Direction.X >= 0.001)
                    {
                        _Anim.SetFloat("Moving", Device.Direction.X);
                    }
                    else
                    {
                        _Anim.SetFloat("Moving", Device.Direction.X);
                    }
                }
                else if(Grabbing == true)
                {
                    if(_IsGrounded == true && Device.Direction.X >= 0.001)
                    {
                        _Anim.SetFloat("Grab_Moving", Device.Direction.X);
                    }
                    else
                    {
                        _Anim.SetFloat("Grab_Moving", Device.Direction.X);
                    }
                }
                
                if (Device.Action1.WasPressed && IsGrounded() && Grabbing == false)
                {
                    _AudioSource.PlayOneShot(_Jump[Random.Range(0, _Jump.Length)], 0.4f);
                    _AudioSource.PlayOneShot(_VoiceJump[Random.Range(0, _VoiceJump.Length)], 1f);
                    _Jumped = true;
                    _Anim.SetTrigger("Jump");
                    _Rb.velocity = Vector3.up * _JumpHeight;
                }
                else if(Device.Action1.WasPressed && IsGrounded() && Grabbing == true)
                {
                    _AudioSource.PlayOneShot(_Jump[Random.Range(0, _Jump.Length)], 0.4f);
                    _AudioSource.PlayOneShot(_VoiceJump[Random.Range(0, _VoiceJump.Length)], 1f);
                    _Jumped = true;
                    _Anim.SetTrigger("Grabbing_Jump");
                    _Rb.velocity = Vector3.up * _JumpHeight;
                }
            }
            else if (p1 == Player1State.Liquid)
            {
                Grabbing = false;
                _Rb.velocity = new Vector3(Device.Direction.X * _LiquidSpeed, _Rb.velocity.y);

                if (Device.Direction.X >= 0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    _SecondStateObject.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                    _SecondStateObject.gameObject.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
                }

                if(_IsGrounded == true && Device.Direction.X >= 0.001)
                {
                    if(_SoundCheckWaterMoving == false)
                    {
                        _AudioSource.loop = true;
                        _AudioSource.clip = _WaterMove;
                        _AudioSource.Play();
                        _SoundCheckWaterMoving = true;
                    }
                    _Anim.SetFloat("Moving", Device.Direction.X);
                }
                else if(_IsGrounded == true && Device.Direction.X <= -0.001)
                {
                    if(_SoundCheckWaterMoving == false)
                    {
                        _AudioSource.loop = true;
                        _AudioSource.clip = _WaterMove;
                        _AudioSource.Play();
                        _SoundCheckWaterMoving = true;
                    }
                    _Anim.SetFloat("Moving", Device.Direction.X);
                }
                else if(_IsGrounded == true && Device.Direction.X == 0)
                {
                    _AudioSource.loop = false;
                    _AudioSource.Stop();
                    _SoundCheckWaterMoving = false;
                }
            }
        }
    }

    public void Player2Movement(Player2State p2)
    {
        if (this.gameObject.tag == "Player2")
        {
            if (p2 == Player2State.Fire)
            {
                var _BoxCollider = GetComponent<BoxCollider>();
                _BoxCollider.enabled = true;
                _Rb.velocity = new Vector3(Device.Direction.X * _Speed, _Rb.velocity.y);

                if (Device.Direction.X >= 0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }

                if (Device.Action1.WasPressed && IsGrounded() && Grabbing == false)
                {
                    _AudioSource.PlayOneShot(_Jump[Random.Range(0, _Jump.Length)], 0.4f);
                    _AudioSource.PlayOneShot(_VoiceJump[Random.Range(0, _VoiceJump.Length)], 1f);
                    _Anim.SetTrigger("Jump");
                    _Jumped = true;
                    _Rb.velocity = Vector3.up * _JumpHeight;
                }
                else if(Device.Action1.WasPressed && IsGrounded() && Grabbing == true)
                {
                    _AudioSource.PlayOneShot(_Jump[Random.Range(0, _Jump.Length)], 0.4f);
                    _AudioSource.PlayOneShot(_VoiceJump[Random.Range(0, _VoiceJump.Length)], 1f);
                    _Anim.SetTrigger("Grabbing_Jump");
                    _Jumped = true;
                    _Rb.velocity = Vector3.up * _JumpHeight;
                }

                if(Grabbing == false)
                {
                    if(_IsGrounded == true && Device.Direction.X >= 0.001)
                    {
                        _Anim.SetFloat("Moving", Device.Direction.X);
                    }
                    else
                    {
                        _Anim.SetFloat("Moving", Device.Direction.X);
                    }
                }
                else if(Grabbing == true)
                {
                    if(_IsGrounded == true && Device.Direction.X >= 0.001)
                    {
                        _Anim.SetFloat("Grab_Moving", Device.Direction.X);
                    }
                    else
                    {
                        _Anim.SetFloat("Grab_Moving", Device.Direction.X);
                    }
                }
            }

            if(p2 == Player2State.Smoke)
            {
                Grabbing = false;
                var _BoxCollider = GetComponent<BoxCollider>();
                var _SmokeGirl = GameObject.Find("m_char_smokegirl");
                _BoxCollider.enabled = false;
                if (Device.Direction.X >= 0.01)
                {
                    if(_SoundCheckWaterMoving == false)
                    {
                        _AudioSource.loop = true;
                        _AudioSource.clip = _WaterMove;
                        _AudioSource.Play();
                        _SoundCheckWaterMoving = true;
                    }
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    _SecondStateObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    if(_SoundCheckWaterMoving == false)
                    {
                        _AudioSource.loop = true;
                        _AudioSource.clip = _WaterMove;
                        _AudioSource.Play();
                        _SoundCheckWaterMoving = true;
                    }
                    _SecondStateObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
                }
            }
        }
    }

    public void Player1AirMovement(Player1State p1)
    {
        if (this.gameObject.tag == "Player1")
        {
            if (p1 == Player1State.Solid)
            {
                _Rb.velocity = new Vector3(Device.Direction.X * _AirSpeed, _Rb.velocity.y);
                if (Device.Direction.X >= 0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }
            }
            else if (p1 == Player1State.Liquid)
            {
                _Rb.velocity = new Vector3(Device.Direction.X * _LiquidAirSpeed, _Rb.velocity.y);
                if (Device.Direction.X >= 0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }
            }
        }
    }

    public void Player2AirMovement(Player2State p2)
    {
        if (this.gameObject.tag == "Player2")
        {
            if (p2 == Player2State.Fire)
            {
                _Rb.velocity = new Vector3(Device.Direction.X * _AirSpeed, _Rb.velocity.y);
                if (Device.Direction.X >= 0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    _SecondStateObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (Device.Direction.X <= -0.01)
                {
                    this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                    _SecondStateObject.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
                }
            }
        }
    }

    private void OnDisable()
    {
        _Anim.SetTrigger("Victory");
    }
}