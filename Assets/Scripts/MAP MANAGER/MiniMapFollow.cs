using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public float height = 50f;

    void LateUpdate()
    {
        if (!player) return;

        transform.position = new Vector3(player.position.x, height, player.position.z);
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        transform.position = new Vector3(
            player.position.x,
            height,
            player.position.z
        );
    }
}