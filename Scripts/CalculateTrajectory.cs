using UnityEngine;
 
[RequireComponent(typeof(LineRenderer))]
public class CalculateTrajectory : MonoBehaviour
{
    [SerializeField]
    private int _MaxSimulationLength = 200;
 
    [SerializeField]
    private float _HitCheckRadius = 0.05f;
 
    [SerializeField]
    private LayerMask _HitMask;
 
    private LineRenderer _Line;
    private Pickable _Pickable;
 
 
    [SerializeField] private Vector3 _Dir;
 
    private void Awake()
    {
        _Line = GetComponent<LineRenderer>();
        _Pickable = GetComponent<Pickable>();
        _Line.positionCount = _MaxSimulationLength;
    }
 
    private void FixedUpdate()
    {
        if(_Pickable._Picked == true)
        {
            _Line.enabled = true;
            SimulatePath(transform.parent.gameObject.GetComponentInParent<PlayerController>().ThrowTransform.position, _Dir);
        }
        else
        {
            _Line.enabled = false;
        }

        if(_Pickable._IsFacingRight == true)
        {
            _Dir = new Vector3(11f,11f,0f);
        }
        else
        {
            _Dir = new Vector3(-11f,11f,0f);
        }
    }
 
    public void SimulatePath(Vector3 pos, Vector3 vel)
    {
        int count = 0;
        _Line.positionCount = 1;
        while (true)
        {
            pos += vel * Time.fixedDeltaTime;
            vel += Physics.gravity * Time.fixedDeltaTime;
            _Line.SetPosition(count, pos);
            if (Physics.OverlapSphere(pos, _HitCheckRadius, _HitMask).Length > 0) return;
            if (++count > _MaxSimulationLength) return;
            _Line.positionCount = count + 1;
        }
    }
}