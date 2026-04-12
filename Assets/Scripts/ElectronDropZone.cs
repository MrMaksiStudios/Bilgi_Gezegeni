using UnityEngine;
using UnityEngine.EventSystems;

public class ElectronDropZone : MonoBehaviour, IDropHandler
{
    public IonicBondZone ionicZone;

    public void OnDrop(PointerEventData eventData)
    {
        Draggable2 electron = eventData.pointerDrag.GetComponent<Draggable2>();
        if (electron == null) return;

        ionicZone.OnElectronPlaced();
        electron.enabled = false; // artık taşınamasın
    }
}

