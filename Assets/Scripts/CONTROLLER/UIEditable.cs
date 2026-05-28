using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEditable : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    RectTransform rect;
    CanvasGroup canvasGroup;

    Vector2 defaultPos;
    Vector3 defaultScale;
    float defaultAlpha;

    public string id;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIEditManager.Instance.currentMode == UIEditManager.EditMode.None)
            return;

        var mode = UIEditManager.Instance.currentMode;

        if (mode == UIEditManager.EditMode.Transparency)
        {
            UIEditSlider.Instance.Open(this, UIEditSlider.EditType.Transparency);
        }
        else if (mode == UIEditManager.EditMode.Size)
        {
            UIEditSlider.Instance.Open(this, UIEditSlider.EditType.Size);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UIEditManager.Instance.currentMode != UIEditManager.EditMode.Position)
            return;

        rect.anchoredPosition += eventData.delta;
    }

    // 🔧 APPLY TRANSPARENCY
    public void SetTransparency(float value)
    {
        canvasGroup.alpha = value;
    }

    // 🔧 APPLY SIZE
    public void SetSize(float value)
    {
        rect.localScale = Vector3.one * value;
    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // 💾 store defaults
        defaultPos = rect.anchoredPosition;
        defaultScale = rect.localScale;
        defaultAlpha = canvasGroup.alpha;
    }

    public void ResetToDefault()
    {
        rect.anchoredPosition = defaultPos;
        rect.localScale = defaultScale;
        canvasGroup.alpha = defaultAlpha;
    }
}