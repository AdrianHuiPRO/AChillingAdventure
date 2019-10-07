using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_OpenDoor : MonoBehaviour, IOnWire, IOffWire
{
    [SerializeField]
    private GameObject _Door;

    public Vector3 _DoorDistance;

    private Vector3 _Origin;

    bool _Opened = false;

    private void Start() 
    {
        _Origin = _Door.transform.position;    
    }

    public void OnWire()
    {
        if(_Opened == false)
        {
            _Door.transform.position = _Door.transform.position + _DoorDistance;
            _Opened = true;
        }
    }

    public void OffWire()
    {
        if(_Opened == true)
        {
            _Door.transform.position = _Origin;
            _Opened = false;
        }
    }
}
