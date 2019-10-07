using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Lighter : MonoBehaviour
{
    public enum LighterState{On, Off}
    public LighterState _LighterCurrentState;
    public GameObject _Fire;
    private AudioSource _AS;
    public AudioClip[] _LighterOnSounds;
    public AudioClip[] _LighterOffSounds;
    public AudioClip[] _SolidtoLiquid;
    public AudioClip[] _SolidtoLiquidVoice;
    public AudioClip _FireIsActive;
    private bool _PlaySound2 = false;
    private bool _PlaySound = false;
    private bool _PlaySound3 = false;
    private bool _IsVisible = false;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void OnBecameInvisible()
    {
        _IsVisible = false;
        Debug.Log(name + " is not Visible");
    }

    private void OnBecameVisible()
    {
        _IsVisible = true;
        Debug.Log(name + " is Visible");
    }

    private void Update() 
    {
        if(_LighterCurrentState == LighterState.Off)
        {
            _Fire.SetActive(false);
            if(_IsVisible == false)
            {
                if(_PlaySound3 == false)
                {
                    _AS.PlayOneShot(_LighterOffSounds[Random.Range(0, _LighterOffSounds.Length)]);
                    _PlaySound3 = true;
                }
                _AS.Stop();
                _AS.loop = false;
                _PlaySound2 = false;
                _PlaySound = false;
            }
        }
        else if(_LighterCurrentState == LighterState.On)
        {
            if(_IsVisible == true)
            {
                if(_PlaySound2 == false)
                {
                    _AS.PlayOneShot(_LighterOnSounds[Random.Range(0, _LighterOnSounds.Length)]);
                    _PlaySound2 = true;
                }
                _PlaySound3 = false;
            }
            _Fire.SetActive(true);
        }

        if(_PlaySound == false)
        {
            if(_IsVisible == true)
            {
                _AS.loop = true;
                _AS.clip = _FireIsActive;
                _AS.Play();
                _PlaySound = true;
            }
        }

        if(_IsVisible == false)
        {
            _AS.loop = false;
            _AS.Stop();
            _PlaySound = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_LighterCurrentState == LighterState.On)
        {
            if(other.gameObject.tag == "Player1")
            {
                if(other.gameObject.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Solid)
                {
                    other.gameObject.GetComponent<PlayerController>()._Player1CurrentState = PlayerController.Player1State.Liquid;
                    _AS.PlayOneShot(_SolidtoLiquid[Random.Range(0, _SolidtoLiquid.Length)], 0.7f);
                    _AS.PlayOneShot(_SolidtoLiquidVoice[Random.Range(0, _SolidtoLiquidVoice.Length)], 1f);
                }
            }
        }
    }
}
