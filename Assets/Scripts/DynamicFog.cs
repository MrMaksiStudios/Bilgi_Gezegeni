using UnityEngine;

public class DynamicFog : MonoBehaviour
{
    public Transform player;
    public Transform nucleus;

    public float maxFogDensity = 0.05f;
    public float minFogDensity = 0.005f;

    public float maxDistance = 50f;

    void Update()
    {
        float distance = Vector3.Distance(player.position, nucleus.position);

        float t = distance / maxDistance;

        float fog = Mathf.Lerp(maxFogDensity, minFogDensity, t);

        RenderSettings.fogDensity = fog;
    }
}