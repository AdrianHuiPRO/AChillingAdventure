using UnityEngine;

public class MovePlatform : MonoBehaviour, IOnPlate, IOffPlate
{
    [SerializeField]
    private GameObject[] _MoveLocation;

    public GameObject _Platform;

    private int _Current = 0;
    private float _PointRadius = 0.001f;

    public float _Speed = 5f;
    private bool _IsPressed = false;

    private void Update() 
    {

        if(_IsPressed == true)
        {
            if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Platform.transform.position) <= _PointRadius)
            {
                _Current = 0;
                if(_Current == 0)
                {
                    _IsPressed = false;
                }
            }
            _Platform.transform.position = Vector3.MoveTowards(_Platform.transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
        }
        
        if(_MoveLocation == null) return;
    }

    public void OnPlate()
    {
        if(_IsPressed == false)
        {
            if(Vector3.Distance(_MoveLocation[_Current].transform.position, _Platform.transform.position) <= _PointRadius)
            {
                _Current++;
                if(_Current >= _MoveLocation.Length)
                {
                    _Current = 0;
                }
            }
            _Platform.transform.position = Vector3.MoveTowards(_Platform.transform.position, _MoveLocation[_Current].transform.position, Time.deltaTime * _Speed);
        }
    }

    public void OffPlate()
    {
        _IsPressed = true;
    }
}
