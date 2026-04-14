using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public LineRenderer line;
    public float radius = 2f;

    void Start()
    {
        InvokeRepeating("CreateLightning", 0f, 0.3f);
    }

    void CreateLightning()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Random.onUnitSphere * radius;

        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}