using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{
    public GameObject minimapUI;
    public GameObject expandedMapUI;

    public MapState state = MapState.Minimap;

    void Start()
    {
        SetMinimap();
    }

    void Update()
    {
        if (state == MapState.Expanded && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            SetMinimap();
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
    }

    void SetExpanded()
    {
        state = MapState.Expanded;

        minimapUI.SetActive(false);
        expandedMapUI.SetActive(true);
    }
}