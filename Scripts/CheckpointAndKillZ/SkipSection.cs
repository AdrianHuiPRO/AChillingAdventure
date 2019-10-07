using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipSection : MonoBehaviour
{
    public Checkpoint[] _Checkpoints;
    public KeyCode _Skip;

    private GameObject _Player1;
    private GameObject _Player2;

    private int _Sections = 0;

    private void Update()
    {
        _Player1 = GameObject.FindWithTag("Player1");
        _Player2 = GameObject.FindWithTag("Player2");

        if(Input.GetKeyDown(_Skip))
        {
            _Sections++;
            StateChange();
            _Player1.transform.position = _Checkpoints[_Sections].transform.position;
            _Player2.transform.position = _Checkpoints[_Sections].transform.position + Vector3.left * 2;
        }

        if(_Sections >= _Checkpoints.Length)
        {
            _Sections = 0;
            StateChange();
            _Player1.transform.position = _Checkpoints[0].transform.position;
            _Player2.transform.position = _Checkpoints[0].transform.position + Vector3.left * 2;
        }
    }

    private void StateChange()
    {
        if(_Checkpoints[_Sections]._PlayersStates == Checkpoint.States.Solid_Fire)
        {
            _Player1.GetComponent<PlayerController>()._Player1CurrentState = PlayerController.Player1State.Solid;
            _Player2.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Fire;
        }
        else if(_Checkpoints[_Sections]._PlayersStates == Checkpoint.States.Solid_Smoke)
        {
            _Player1.GetComponent<PlayerController>()._Player1CurrentState = PlayerController.Player1State.Solid;
            _Player2.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Smoke;
        }
        else if(_Checkpoints[_Sections]._PlayersStates == Checkpoint.States.Liquid_Fire)
        {
            _Player1.GetComponent<PlayerController>()._Player1CurrentState = PlayerController.Player1State.Liquid;
            _Player2.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Fire;
        }
        else if(_Checkpoints[_Sections]._PlayersStates == Checkpoint.States.Liquid_Smoke)
        {
            _Player1.GetComponent<PlayerController>()._Player1CurrentState = PlayerController.Player1State.Liquid;
            _Player2.GetComponent<PlayerController>()._Player2CurrentState = PlayerController.Player2State.Smoke;
        }
    }
}
