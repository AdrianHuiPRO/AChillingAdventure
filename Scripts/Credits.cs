using UnityEngine;
using DG.Tweening;

public class Credits : MonoBehaviour
{
    public GameObject _CreditsText;
    [SerializeField]
    private float _ScrollingTime = 60f;
    [SerializeField]
    private float _NewPos;
    private Vector3 _OriginalSpot;

    private void Awake()
    {
        _OriginalSpot = _CreditsText.transform.position;
    }

    private void OnEnable()
    {
        if(_CreditsText.transform.position.y < _NewPos)
        {
            _CreditsText.transform.GetComponent<RectTransform>().DOAnchorPosY(_NewPos, _ScrollingTime);
        }
    }

    private void OnDisable()
    {
        _CreditsText.transform.DORestart();
        _CreditsText.transform.DOPause();
        _CreditsText.transform.position = _OriginalSpot;
    }
}
