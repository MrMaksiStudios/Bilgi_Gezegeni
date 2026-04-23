using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public ItemData item;

    private bool playerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractionUI.Instance.ShowPickup(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionUI.Instance.HideButton();
        }
    }

    public void Pickup()
    {
        if (!playerInRange) return;

        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }
}