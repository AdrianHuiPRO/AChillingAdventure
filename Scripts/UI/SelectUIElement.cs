using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectUIElement : MonoBehaviour
{

    [SerializeField]
    private Button _Button;

    [SerializeField]
    private bool _SelectOnEnable = true;

    private void OnEnable()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(_Button.gameObject, new BaseEventData(eventSystem));
        // EventSystem.current.isFocused = true;
    }

    private void OnDisable()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null, new BaseEventData(eventSystem));
    }

}
