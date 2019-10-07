using UnityEngine;

public class OpenDoor : MonoBehaviour, IOnPlate, IOffPlate
{
    [SerializeField]
    private GameObject _Door;

    public Vector3 _DoorDistance;

    private Vector3 _Origin;

    bool _Opened = false;

    private void Start() 
    {
        _Origin = _Door.transform.position;
        if(_Door == null) return;
    }

    public void OnPlate()
    {
        if(_Opened == false)
        {
            _Door.transform.position = _Door.transform.position + _DoorDistance;
            _Opened = true;
        }
    }

    public void OffPlate()
    {
        if(_Opened == true)
        {
            _Door.transform.position = _Origin;
            _Opened = false;
        }
    }
}
