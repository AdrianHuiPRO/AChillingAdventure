using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

public class BreakableWall : MonoBehaviour
{
    [SerializeField]
    private GameObject _DestroyedWall;

    public Particle _Particle;
    public AudioSource _AS;
    public AudioClip _BrownieBreak;

    private void OnDisable()
    {
        var clone = PoolManager.GetObjectFromPool(_Particle.gameObject);
        clone.gameObject.SetActive(true);
        clone.transform.position = transform.position;
        DontDestroyOnLoad(clone);
        _DestroyedWall.SetActive(true);
        this.gameObject.SetActive(false);
        _DestroyedWall.transform.position = transform.position;

        if(_BrownieBreak != null)
        {
            _AS.PlayOneShot(_BrownieBreak);
            Debug.Log("Brownie Sound is playing");
        }
        foreach (var item in _DestroyedWall.GetComponentsInChildren<Rigidbody>())
        {
            item.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));
        }
    }
}
