using System;
using UnityEngine;

public class RotateDoor : MonoBehaviour, IOnPlate, IOffPlate
{
    [SerializeField]
    private GameObject _Door;

    public float _NewZ;
    public float _Speed = 0.3f;
    public bool _InverseRotation;
    public Vector3 _CenterOfMass;

    private Quaternion _Origin;
    private bool _Return = false;
    private bool _DoOnce = false;
    private Quaternion _NewRotation;
    private Rigidbody _RB;

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

    public void OnPlate()
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
                    }
                    else if(_RB.rotation == Quaternion.Lerp(_RB.rotation, _NewRotation, _Speed))
                    {
                        _DoOnce = true;
                        _RB.rotation = Quaternion.Euler(0f, 0f, _NewZ);
                    }
                }
            }
        }
    }

    public void OffPlate()
    {
        if(_DoOnce == true)
        {
            _DoOnce = false;
        }
        _Return = true;
    }
}
