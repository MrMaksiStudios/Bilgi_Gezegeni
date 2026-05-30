using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionWarpTriggerRoute
{
    [Header("Trigger Matching")]
    [Tooltip("Optional tag requirement. If empty, any tag will pass.")]
    public string triggerTag = "Player";

    [Tooltip("Optional layer requirement. If left at default, any layer will pass.")]
    public LayerMask triggerLayers = ~0;

    [Tooltip("Enable this to require the incoming object's tag to match.")]
    public bool useTagFilter = true;

    [Tooltip("Enable this to require the incoming object's layer to match the selected layer mask.")]
    public bool useLayerFilter = false;

    [Header("Warp Settings")]
    [Tooltip("Warp type to store when this route fires.")]
    public WarpType warpType = WarpType.To_P_Orbital;

    [Tooltip("Scene name to load. Leave empty only if using warp back.")]
    public string targetSceneName = "InterOrbitals";

    [Tooltip("Enable this to use WarpController.StartWarpBack() instead of loading a scene.")]
    public bool useWarpBack = false;

    [Tooltip("If enabled, this route will only fire once.")]
    public bool triggerOnlyOnce = true;

    [Header("Teleport")]
    [Tooltip("Destination point to teleport the player to before the warp.")]
    public Transform destinationPoint;

    [HideInInspector]
    public bool hasTriggered;
}

public class MissionWarpTriggerManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Optional assigned WarpController. If empty, the singleton will be used.")]
    [SerializeField] private WarpController warpController;

    [Tooltip("Player transform to teleport and save.")]
    [SerializeField] private Transform player;

    [Tooltip("Zone indicator manager used to save the player's zone data.")]
    [SerializeField] private ZoneIndicatorManager zoneIndicatorManager;

    [Header("Routes")]
    [Tooltip("Add as many warp routes as needed in the inspector.")]
    [SerializeField] private List<MissionWarpTriggerRoute> routes = new List<MissionWarpTriggerRoute>();

    private WarpController ActiveController
    {
        get
        {
            if (warpController != null)
                return warpController;

            return WarpController.Instance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTrigger(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTrigger(other.gameObject);
    }

    private void HandleTrigger(GameObject otherObject)
    {
        if (ActiveController == null)
        {
            Debug.LogWarning($"[{nameof(MissionWarpTriggerManager)}] No WarpController assigned or available.", this);
            return;
        }

        for (int i = 0; i < routes.Count; i++)
        {
            MissionWarpTriggerRoute route = routes[i];
            if (route == null)
                continue;

            if (route.hasTriggered && route.triggerOnlyOnce)
                continue;

            if (route.useTagFilter && !string.IsNullOrEmpty(route.triggerTag) && !otherObject.CompareTag(route.triggerTag))
                continue;

            if (route.useLayerFilter && ((1 << otherObject.layer) & route.triggerLayers.value) == 0)
                continue;

            ExecuteRoute(route);

            if (route.triggerOnlyOnce)
            {
                route.hasTriggered = true;
            }

            break;
        }
    }

    private void ExecuteRoute(MissionWarpTriggerRoute route)
    {
        WarpData.currentWarp = route.warpType;

        if (player != null && route.destinationPoint != null)
        {
            player.position = route.destinationPoint.position;
        }

        if (player != null && zoneIndicatorManager != null)
        {
            SaveManager.Instance.SaveGame(player, zoneIndicatorManager.zones);
        }
        else if (player != null && zoneIndicatorManager == null)
        {
            Debug.LogWarning($"[{nameof(MissionWarpTriggerManager)}] No ZoneIndicatorManager assigned, so the save step was skipped.", this);
        }

        if (route.useWarpBack)
        {
            ActiveController.StartWarpBack();
            return;
        }

        if (string.IsNullOrWhiteSpace(route.targetSceneName))
        {
            Debug.LogWarning($"[{nameof(MissionWarpTriggerManager)}] Route '{route.warpType}' has no target scene name configured.", this);
            return;
        }

        ActiveController.StartWarp(route.targetSceneName);
    }
}
