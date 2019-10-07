using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SmoketoFire : MonoBehaviour
{
    private AudioSource _AS;
    public AudioClip[] _SmokeToFire;
    public AudioClip[] _SmokeToFireVoice;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player2")
        {
            if(other.GetComponent<PlayerController>()._Player2CurrentState == PlayerController.Player2State.Smoke)
            {
                other.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Fire;
                _AS.PlayOneShot(_SmokeToFire[Random.Range(0, _SmokeToFire.Length)]);
                _AS.PlayOneShot(_SmokeToFireVoice[Random.Range(0, _SmokeToFireVoice.Length)]);
            }
        }
    }
}
