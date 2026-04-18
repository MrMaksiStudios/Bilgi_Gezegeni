using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float speed = 10f;
    public float height = 2f;

    private float angle = 0f;

    void Update()
    {
        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(rad) * distance,
            height,
            Mathf.Sin(rad) * distance
        );

        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}