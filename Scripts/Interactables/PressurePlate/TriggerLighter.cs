using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLighter : MonoBehaviour, IOffPlate
{
    [SerializeField]
    private Lighter _Lighter;

    private bool _CheckLighter = false;

    private void OnTriggerEnter(Collider other)
    {
        if(_CheckLighter == false)
        {
            if(_Lighter._LighterCurrentState == Lighter.LighterState.Off)
            {
                _Lighter._LighterCurrentState = Lighter.LighterState.On;
            }
            else if(_Lighter._LighterCurrentState == Lighter.LighterState.On)
            {
                _Lighter._LighterCurrentState = Lighter.LighterState.Off;
            }
            _CheckLighter = true;
        }
    }

    public void OffPlate()
    {
        _CheckLighter = false;
    }
}
