using UnityEngine;
using UnityEngine.EventSystems;

public class CovalentDropZone : MonoBehaviour, IDropHandler
{
    public CovalentBondZone zone;

    public void OnDrop(PointerEventData eventData)
    {
        Draggable2 electron = eventData.pointerDrag.GetComponent<Draggable2>();
        if (electron == null) return;

        zone.OnElectronPlaced();
        electron.enabled = false;
    }
}
