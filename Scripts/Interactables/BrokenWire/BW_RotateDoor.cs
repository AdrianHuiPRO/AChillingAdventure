using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_RotateDoor : MonoBehaviour, IOnWire, IOffWire
{
    [SerializeField]
    private GameObject _Door;

    public float _NewZ;
    public float _Speed = 0.3f;
    public bool _InverseRotation;
    public AudioSource _AS;
    public AudioClip[] _Swing;
    public Vector3 _CenterOfMass;

    private Quaternion _Origin;
    private bool _Return = false;
    private bool _DoOnce = false;
    private Quaternion _NewRotation;
    private Rigidbody _RB;
    private bool _DoOnce2;

    private void Awake()
    {
        _RB = _Door.GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        _RB.centerOfMass = _CenterOfMass;
        _Origin = _Door.transform.rotation;
        _NewRotation = Quaternion.Euler(0f, 0f, _NewZ);
        if(_InverseRotation == true)
        {
            _NewRotation = Quaternion.Inverse(_NewRotation);
        }
    }

    private void Update() 
    {
        if(_Return == true)
        {
            _RB.rotation = Quaternion.Lerp(_RB.rotation, _Origin, _Speed);
            if(_RB.rotation == _Origin)
            {
                _Return = false;
            }
        }
    }

    public void OnWire()
    {
        if(_Return == false)
        {
            if(_NewZ >= 0f)
            {
                if(_DoOnce == false)
                {
                    if(_RB.rotation != Quaternion.Lerp(_RB.rotation, _NewRotation, _Speed))
                    {
                        _RB.rotation = Quaternion.Lerp(_RB.rotation, _NewRotation, _Speed);
                        if(_DoOnce2 == false)
                        {
                            _AS.PlayOneShot(_Swing[Random.Range(0, _Swing.Length)]);
                            _DoOnce2 = true;
                        }
                    }
                    else if(_RB.rotation == Quaternion.Lerp(_RB.rotation, _NewRotation, _Speed))
                    {
                        _DoOnce = true;
                        _DoOnce2 = false;
                    }
                }
            }
        }
    }

    public void OffWire()
    {
        if(_DoOnce == true)
        {
            _AS.PlayOneShot(_Swing[Random.Range(0, _Swing.Length)]);
            _DoOnce = false;
        }
        _Return = true;
    }
}
