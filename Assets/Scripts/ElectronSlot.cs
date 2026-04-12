using UnityEngine;

public class ElectronSlot : MonoBehaviour
{
    public SlotType slotType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ElectronDrag electron = other.GetComponent<ElectronDrag>();
        if (!electron) return;

        BondGameController.Instance.TryPlaceElectron(electron, this);
    }
}
