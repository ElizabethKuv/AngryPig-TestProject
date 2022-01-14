using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _joystickBg;
    [SerializeField] private Image _joystickButton;
    private Vector2 _inputPos;

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _joystickBg.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _inputPos))
        {
            var sizeDelta = _joystickBg.rectTransform.sizeDelta;
            _inputPos.x /= (sizeDelta.x);
            _inputPos.y /= (sizeDelta.y);

            if (_inputPos.magnitude > 1f)
            {
                _inputPos = _inputPos.normalized;
            }

            _joystickButton.rectTransform.anchoredPosition = new Vector2(
                _inputPos.x * sizeDelta.x / 3, _inputPos.y * sizeDelta.y / 3);
        }
    }
    


    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputPos = Vector2.zero;
        _joystickButton.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float InputHorizontal()
    {
        if (_inputPos.x != 0)
        {
            return _inputPos.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float InputVertical()
    {
        if (_inputPos.y != 0)
        {
            return _inputPos.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}