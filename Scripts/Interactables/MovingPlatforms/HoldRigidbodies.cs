using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldRigidbodies : MonoBehaviour
{
    public bool _SensorEnabled = false;
    public List<Rigidbody> _Rigidbodies = new List<Rigidbody>();

    public bool _InTrigger = true;

    public Vector3 _LastPosition;
    private Transform _Transform;
    [HideInInspector] public Rigidbody _RB;

    private void Start()
    {
        _Transform = transform;
        _LastPosition = _Transform.position;
        _RB = GetComponent<Rigidbody>();

        if(_SensorEnabled == true)
        {
            foreach (RBSensor sensor in GetComponentsInChildren<RBSensor>())
            {
                sensor._Carry = this;
            }
        }
    }

    private void LateUpdate()
    {
        if(_Rigidbodies.Count > 0)
        {
            for (int i = 0; i < _Rigidbodies.Count; i++)
            {
                Rigidbody rb = _Rigidbodies[i];
                Vector3 velocity = (_Transform.position - _LastPosition);
                rb.transform.Translate(velocity, _Transform);
            }
        }
        _LastPosition = _Transform.position;

        foreach (Rigidbody _rb in _Rigidbodies)
        {
            if(_rb.gameObject.activeInHierarchy == false)
            {
                Remove(_rb);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Rigidbody _rb = other.collider.GetComponent<Rigidbody>();
        if(_rb != null)
        {
            Add(_rb);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Rigidbody _rb = other.collider.GetComponent<Rigidbody>();
        if(_rb != null)
        {
            Remove(_rb);
        }
    }

    public void Add(Rigidbody rb)
    {
        if(!_Rigidbodies.Contains(rb))
        {
            _Rigidbodies.Add(rb);
        }
    }

    public void Remove(Rigidbody rb)
    {
        if(_Rigidbodies.Contains(rb))
        {
            _Rigidbodies.Remove(rb);
        }
    }
}
