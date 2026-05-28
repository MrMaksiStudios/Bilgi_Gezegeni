using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public ItemData item;
    [SerializeField] private string pickupableID; // Unique ID for this pickupable

    private bool playerInRange;

    void Start()
    {
        // Generate ID from position if not set (for debugging)
        if (string.IsNullOrEmpty(pickupableID))
        {
            pickupableID = gameObject.name + "_" + transform.position.GetHashCode();
        }
    }

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

        InventoryManager.Instance.AddItem(item, pickupableID);
        InteractionUI.Instance.HideButton();
        Destroy(gameObject);
    }

    public string GetPickupableID()
    {
        return pickupableID;
    }
}