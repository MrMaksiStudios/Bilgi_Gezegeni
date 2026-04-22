using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnClickOutside: MonoBehaviour
{
    public GameObject detailsPanel;

    void Update()
    {
        // If panel is not open → do nothing
        if (!detailsPanel.activeSelf) return;

        // Detect mouse / touch
        if (Input.GetMouseButtonDown(0))
        {
            // Check if click is NOT on UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                detailsPanel.SetActive(false);
            }
        }
    }
}