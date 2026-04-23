using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    public GameObject buttonObject;
    public Button interactButton;

    private HasDialogue currentNPC;
    private Pickupable currentPickup;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (buttonObject != null)
            buttonObject.SetActive(false);

        if (interactButton != null)
            interactButton.onClick.AddListener(OnPress);
    }

    public void ShowButton(HasDialogue npc)
    {
        currentNPC = npc;
        buttonObject.SetActive(true);
    }

    public void HideButton()
    {
        buttonObject.SetActive(false);
        currentNPC = null;
    }

    void OnPress()
    {
        if (currentNPC != null)
            currentNPC.Interact();
        else if (currentPickup != null)
            currentPickup.Pickup();
    }

    public void ShowPickup(Pickupable pickup)
    {
        currentPickup = pickup;
        currentNPC = null;
        buttonObject.SetActive(true);
    }
}