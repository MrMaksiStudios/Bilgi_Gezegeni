using UnityEngine;
using UnityEngine.EventSystems;

public class MobileDpadInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 InputVector { get; private set; }

    public RectTransform area;

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateInput(eventData);
        //Debug.Log("TOUCH DOWN");
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateInput(eventData);
        //Debug.Log("TOUCH DRAG");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector2.zero;
        //Debug.Log("TOUCH UP");
    }

    void UpdateInput(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            area,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        Vector2 normalized = localPoint / (area.sizeDelta / 2f);

        normalized = Vector2.ClampMagnitude(normalized, 1f);

        InputVector = new Vector2(
            Mathf.Abs(normalized.x) > 0.3f ? Mathf.Sign(normalized.x) : 0,
            Mathf.Abs(normalized.y) > 0.3f ? Mathf.Sign(normalized.y) : 0
        );

        if (normalized.magnitude < 0.3f)
        {
            InputVector = Vector2.zero;
            return;
        }

        //Debug.Log("INPUT: " + InputVector);
    }
}