using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable2 : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rect;
    Canvas canvas;

    public Image reactionZoneImage;
    public ReactionZoneUI reactionZone;


    [Header("Visual")]
    public Image atomImage; // 👈 child Image

    [Header("Delete Zone")]
    public RectTransform deleteZone;

    Color originalColor;
    bool overDeleteZone;

    [Header("Bond Zone")]
    public Image bondZoneImage;   // 👈 iyonik / kovalent alan
    public bool lockAfterDrop = true;

    bool overBondZone;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        originalColor = atomImage.color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (deleteZone == null) return;

        overDeleteZone = RectTransformUtility.RectangleContainsScreenPoint(
            deleteZone,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );

        atomImage.color = overDeleteZone ? Color.red : originalColor;

        if (bondZoneImage != null)
        {
            overBondZone = RectTransformUtility.RectangleContainsScreenPoint(
                bondZoneImage.rectTransform,
                Input.mousePosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
            );
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (overDeleteZone)
        {
            Destroy(gameObject);
        }
        else
        {
            atomImage.color = originalColor;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(
            reactionZoneImage.rectTransform,
            Input.mousePosition,
            eventData.pressEventCamera))
        {
            reactionZone.AddAtom(GetComponent<AtomInstance>());
        }
        else
        {
            reactionZone.RemoveAtom(GetComponent<AtomInstance>());
        }

        if (overBondZone)
        {
            HandleBondSuccess();
        }

        atomImage.color = originalColor;
    }

    void HandleBondSuccess()
    {
        // 🔹 İyonik sahne mi?
        IonicBondZone ionic = FindObjectOfType<IonicBondZone>();
        if (ionic != null)
            ionic.OnElectronPlaced();

        // 🔹 Kovalent sahne mi?
        CovalentBondZone covalent = FindObjectOfType<CovalentBondZone>();
        if (covalent != null)
            covalent.OnElectronPlaced();

        if (lockAfterDrop)
            enabled = false; // elektron artık taşınamaz
    }

}
