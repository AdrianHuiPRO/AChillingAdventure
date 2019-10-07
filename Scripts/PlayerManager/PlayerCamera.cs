using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Targets")]
    public List<Transform> _Targets;
    [Header("Camera")]
    public Vector3 _Offset;
    public float _SmoothTime;
    public float _MinZoom = 10f;
    public float _MaxZoom = 10f;
    public float _Limiter;
    [Header("Clamp Camera Position")]
    public float _MinYPos;
    public float _HighestYPos;
    public float _MinXPos;
    public float _HighestXPos;

    private GameObject _Player1;
    private GameObject _Player2;
    public GameObject[] _Objects;
    private Vector3 _Velocity;
    private Camera _Cam;
    private bool _DoOnce1;
    private bool _DoOnce2;
    public bool _IsStatic = false;
    private bool Shaking = false;
    private AudioSource _AS;
    public AudioClip _Explosion;

    private void Awake()
    {
        _Cam = GetComponent<Camera>();
        _AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _Player1 = GameObject.FindWithTag("Player1");
        _Player2 = GameObject.FindWithTag("Player2");
        KeepTrackOfTargets();
    }

    private void FixedUpdate() 
    {
        if(_Targets.Count == 0)
        {
            return;
        }

        if(_IsStatic == false)
        {
            if(Shaking == false)
            {
                Move();
                Zoom();
            }
        }
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(_MaxZoom, _MinZoom, (GetGreatestDistanceX() + GetGreatestDistanceY()) / _Limiter);
        _Cam.fieldOfView = Mathf.Lerp(_Cam.fieldOfView, newZoom, Time.deltaTime);
        _Cam.orthographicSize = Mathf.Lerp(_Cam.orthographicSize, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + _Offset;

        Vector3 finalPosition = Vector3.SmoothDamp(transform.position, newPosition, ref _Velocity, _SmoothTime);

        finalPosition.y = Mathf.Clamp(finalPosition.y, _MinYPos, _HighestYPos);
        finalPosition.x = Mathf.Clamp(finalPosition.x, _MinXPos, _HighestXPos);

        transform.position = finalPosition;
    }

    float GetGreatestDistanceX()
    {
        var bounds = new Bounds(_Targets[0].position, Vector3.zero);
        for (int i = 0; i < _Targets.Count; i++)
        {
            bounds.Encapsulate(_Targets[i].position);
        }
        if(_Targets.Count == 1)
        {
            return 1f;
        }

        return bounds.size.x;
    }
    float GetGreatestDistanceY()
    {
        var bounds = new Bounds(_Targets[0].position, Vector3.zero);
        for (int i = 0; i < _Targets.Count; i++)
        {
            bounds.Encapsulate(_Targets[i].position);
        }
        if(_Targets.Count == 1)
        {
            return 1f;
        }

        return bounds.size.y;
    }

    Vector3 GetCenterPoint()
    {
        if(_Targets.Count == 1)
        {
            return _Targets[0].position;
        }

        var bounds = new Bounds(_Targets[0].position, Vector3.zero);
        for (int i = 0; i < _Targets.Count; i++)
        {
            bounds.Encapsulate(_Targets[i].position);
        }
        return bounds.center;
    }

    private void KeepTrackOfTargets()
    {
        if(_Player1 != null && _Player1.activeInHierarchy == true && _DoOnce1 == false)
        {
            _DoOnce1 = true;
            _Targets.Add(_Player1.transform);
        }

        if(_Player2 != null && _Player2.activeInHierarchy == true && _DoOnce2 == false)
        {
            _DoOnce2 = true;
            _Targets.Add(_Player2.transform);
        }
    }

    public void DoShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Shaking = true;
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;
        if(_AS != null && _Explosion != null)
        {
            _AS.PlayOneShot(_Explosion, 1f);
        }

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x + originalPosition.x, y + originalPosition.y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPosition;
        yield return new WaitForSeconds(duration);
        Shaking = false;
    }
}
