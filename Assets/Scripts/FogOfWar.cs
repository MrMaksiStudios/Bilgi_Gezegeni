using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{
    public int textureSize = 1024;
    public float worldSize = 100f;

    public Transform player;

    [Header("UI Views")]
    public RawImage minimapFog;
    public RawImage expandedMapFog;

    private Texture2D fogTexture;
    private Color32[] fogPixels;

    void Start()
    {
        fogTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        fogPixels = new Color32[textureSize * textureSize];

        for (int i = 0; i < fogPixels.Length; i++)
            fogPixels[i] = new Color32(0, 0, 0, 255);

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();

        // 🔥 SAME texture used for BOTH maps
        minimapFog.texture = fogTexture;
        expandedMapFog.texture = fogTexture;
    }

    void Update()
    {
        RevealAtPlayer();
    }

    void RevealAtPlayer()
    {
        Vector2 uv = WorldToUV(player.position);
        RevealCircle(uv, 60);
    }

    Vector2 WorldToUV(Vector3 pos)
    {
        float u = (pos.x / worldSize) + 0.5f;
        float v = (pos.z / worldSize) + 0.5f;

        return new Vector2(u * textureSize, v * textureSize);
    }

    void RevealCircle(Vector2 center, int radius)
    {
        int cx = (int)center.x;
        int cy = (int)center.y;

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y > radius * radius)
                    continue;

                int px = cx + x;
                int py = cy + y;

                if (px < 0 || py < 0 || px >= textureSize || py >= textureSize)
                    continue;

                int index = py * textureSize + px;

                fogPixels[index].a = 0;
            }
        }

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();
    }
}