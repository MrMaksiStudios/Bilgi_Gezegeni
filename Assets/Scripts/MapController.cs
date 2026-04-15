using UnityEngine;

public class MapController : MonoBehaviour
{
    public Camera minimapCamera;
    public Camera worldMapCamera;

    public CanvasGroup minimapUI;
    public CanvasGroup expandedMapUI;

    public MapState state = MapState.Minimap;

    void Start()
    {
        SetMinimapMode();
    }

    void Update()
    {
        // Click to expand
        if (state == MapState.Minimap && Input.GetMouseButtonDown(0))
        {
            SetExpandedMode();
        }
        // Click anywhere to close
        else if (state == MapState.Expanded && Input.GetMouseButtonDown(0))
        {
            SetMinimapMode();
        }
    }

    void SetMinimapMode()
    {
        state = MapState.Minimap;

        minimapCamera.enabled = true;
        worldMapCamera.enabled = false;

        minimapUI.alpha = 1;
        minimapUI.blocksRaycasts = true;

        expandedMapUI.alpha = 0;
        expandedMapUI.blocksRaycasts = false;
    }

    void SetExpandedMode()
    {
        state = MapState.Expanded;

        minimapCamera.enabled = false;
        worldMapCamera.enabled = true;

        minimapUI.alpha = 0;
        minimapUI.blocksRaycasts = false;

        expandedMapUI.alpha = 1;
        expandedMapUI.blocksRaycasts = true;
    }
}