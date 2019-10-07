using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBSensor : MonoBehaviour
{
    [HideInInspector] public HoldRigidbodies _Carry;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null && rb != _Carry._RB)
        {
            _Carry.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null && rb != _Carry._RB)
        {
            _Carry.Remove(rb);
        }
    }
}
