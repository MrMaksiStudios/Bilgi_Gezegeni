using UnityEngine;

public class MapTeleport : MonoBehaviour
{
    public Transform player;
    public Transform targetPoint;
    public MapController mapController;

    private Rigidbody rb;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        
        // Auto-find MapController if not assigned
        if (mapController == null)
            mapController = FindObjectOfType<MapController>();
    }

    public void Teleport()
    {
        // Only allow teleport if the expanded map is active
        if (mapController != null && mapController.state != MapState.Expanded)
        {
            return;
        }

        if (rb != null)
        {
            // Stop all motion
            //rb.linearVelocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;

            // Move safely
            rb.position = targetPoint.position + Vector3.up * 0.5f;
            Physics.SyncTransforms();

            rb.Sleep();
        }
        else
        {
            player.position = targetPoint.position;
            Physics.SyncTransforms();
        }

        //Debug.Log("Teleported safely");
    }
}