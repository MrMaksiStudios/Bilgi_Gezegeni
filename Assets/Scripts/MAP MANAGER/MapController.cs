using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public GameObject minimapUI;
    public GameObject expandedMapUI;
    public GameObject teleportButtonsContainer; // Container with all teleport buttons
    private RectTransform expandedMapRect;

    public MapState state = MapState.Minimap;

    void Start()
    {
        SetMinimap();
        expandedMapRect = expandedMapUI.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (state == MapState.Expanded && Input.GetMouseButtonDown(0))
        {
            // Only close if clicking outside the expanded map UI
            if (!RectTransformUtility.RectangleContainsScreenPoint(expandedMapRect, Input.mousePosition))
            {
                SetMinimap();
            }
        }
    }

    public void OpenMap()
    {
        if (state == MapState.Minimap)
            SetExpanded();
    }

    void SetMinimap()
    {
        state = MapState.Minimap;

        minimapUI.SetActive(true);
        expandedMapUI.SetActive(false);
        
        // Hide teleport buttons
        if (teleportButtonsContainer != null)
            teleportButtonsContainer.SetActive(false);
    }

    void SetExpanded()
    {
        state = MapState.Expanded;

        minimapUI.SetActive(false);
        expandedMapUI.SetActive(true);
        
        // Show teleport buttons
        if (teleportButtonsContainer != null)
            teleportButtonsContainer.SetActive(true);
    }
}