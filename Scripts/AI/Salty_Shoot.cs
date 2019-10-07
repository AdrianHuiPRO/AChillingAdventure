using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Salty_Shoot : MonoBehaviour
{
    public Salty_Projectile _Salt;
    public Salty_Projectile[] _SaltyProjectiles;
    [SerializeField]
    private GameObject _MouthPosition;
    public bool _IsFacingRight = true;
    private AudioSource _AS;
    public AudioClip[] _ShootSound;
    public AudioClip[] _ShootVoiceSound;
    public AudioClip[] _ShakeSound;
    public Particle _Shot;

    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }
    
    public void SaltShoot()
    {
        var shoot = PoolManager.GetObjectFromPool(_Shot.gameObject);
        shoot.transform.position = _MouthPosition.transform.position;
        var clone = PoolManager.GetObjectFromPool(_Salt.gameObject);
        clone.transform.position = _MouthPosition.transform.position;
        if(_IsFacingRight == true)
        {
            shoot.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            shoot.gameObject.SetActive(true);
            clone.gameObject.GetComponent<Salty_Projectile>()._FacingRight = true;
        }
        else if(_IsFacingRight == false)
        {
            shoot.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            shoot.gameObject.SetActive(true);
            clone.gameObject.GetComponent<Salty_Projectile>()._FacingRight = false;
        }
        clone.gameObject.SetActive(true);
        DontDestroyOnLoad(clone);
        DontDestroyOnLoad(shoot);
        _AS.PlayOneShot(_ShootSound[Random.Range(0, _ShootSound.Length)], 0.4f);
        _AS.PlayOneShot(_ShootVoiceSound[Random.Range(0, _ShootVoiceSound.Length)], 1f);
    }

    public void HeadBentBack()
    {
        _AS.PlayOneShot(_ShakeSound[Random.Range(0, _ShakeSound.Length)]);
    }
}
