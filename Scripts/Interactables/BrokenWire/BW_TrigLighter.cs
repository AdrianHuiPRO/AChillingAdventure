using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_TrigLighter : MonoBehaviour, IOffWire
{
    [SerializeField]
    private Lighter _Lighter;

    private bool _CheckLighter = false;
    private BrokenWireConnect _BrokenWire;

    private void Awake()
    {
        _BrokenWire = GetComponent<BrokenWireConnect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() == null) return;
        
        if(other.GetComponent<PlayerController>()._Player1CurrentState == PlayerController.Player1State.Liquid)
        {
            if(_CheckLighter == false)
            {
                if(_Lighter._LighterCurrentState == Lighter.LighterState.Off)
                {
                    _Lighter._LighterCurrentState = Lighter.LighterState.On;
                }
                _CheckLighter = true;
            }

            _BrokenWire._WaterIsConnected = true;
        }
    }
    public void OffWire()
    {
        if(_Lighter._LighterCurrentState == Lighter.LighterState.On)
        {
            _Lighter._LighterCurrentState = Lighter.LighterState.Off;
        }
        _CheckLighter = false;
    }
}
