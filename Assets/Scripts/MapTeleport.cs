using UnityEngine;

public class MapTeleport : MonoBehaviour
{
    public Transform player;
    public Transform targetPoint;

    private Rigidbody rb;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    public void Teleport()
    {
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