using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public Transform player;
    public float activeDistance = 50f;

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        gameObject.GetComponent<MeshRenderer>().enabled = dist < activeDistance;
    }
}