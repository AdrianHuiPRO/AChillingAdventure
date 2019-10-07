using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager _CM;

    public Animator _Anim;

    public enum States { Solid_Fire, Solid_Smoke, Liquid_Fire, Liquid_Smoke}

    public States _PlayersStates;

    private bool _Player1Hit = false;
    private bool _Player2Hit = false;
    private bool _Hit = false;
    public ParticleSystem _StartingParticle;
    public ParticleSystem _ExplosionParticle;

    private void Start() 
    {
        _CM = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player1"))
        {
            _Player1Hit = true;
            _Anim.SetBool("IcebertHit", true);
        }
        if(other.CompareTag("Player2"))
        {
            _Player2Hit = true;
            _Anim.SetBool("SpiceGirlHit", true);
        }
    }

    private void FixedUpdate()
    {
        if(_Player1Hit == true && _Player2Hit == true)
        {
            if(_Hit == false)
            {
                _CM._LastCheckpointPosP1 = transform.position;
                _CM._LastCheckpointPosP2 = transform.position;
                _Hit = true;
                _StartingParticle.Stop();
                _ExplosionParticle.Play();
            }
        }
    }
    
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
