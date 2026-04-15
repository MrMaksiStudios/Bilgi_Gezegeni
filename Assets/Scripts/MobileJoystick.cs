using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    public float maxRadius = 60f;

    Vector2 inputVector;

    public float XInput => inputVector.x;
    public float YInput => inputVector.y;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        pos = Vector2.ClampMagnitude(pos, maxRadius);

        Vector2 rawInput = pos / maxRadius;

        if (rawInput == Vector2.zero)
        {
            inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            return;
        }

        if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
        {
            inputVector = new Vector2(Mathf.Sign(rawInput.x), 0);
            pos = new Vector2(Mathf.Sign(pos.x) * Mathf.Abs(pos.x), 0);
        }
        else
        {
            inputVector = new Vector2(0, Mathf.Sign(rawInput.y));
            pos = new Vector2(0, Mathf.Sign(pos.y) * Mathf.Abs(pos.y));
        }

        handle.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}