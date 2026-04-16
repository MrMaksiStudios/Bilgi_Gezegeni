using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{
    [Header("Map Settings")]
    public int textureSize = 512;      // 256 / 512 / 1024
    public float worldSize = 100f;     // Map width in world units (for example, 100 means -50..+50)
    public Vector2 mapOrigin = Vector2.zero; // World position of the map center in XZ plane

    [Header("References")]
    public Transform player;
    public RawImage expandedMapFog;

    [Header("Fog Settings")]
    public float revealRadius = 5f;    // Açılma yarıçapı in world units
    public float updateThreshold = 1f; // Kaç birim hareket edince güncellensin

    private Texture2D fogTexture;
    private Color32[] fogPixels;

    private Vector3 lastPlayerPos;

    void Start()
    {
        // Texture oluştur
        fogTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        fogTexture.filterMode = FilterMode.Bilinear;
        fogTexture.alphaIsTransparency = true;
        fogPixels = new Color32[textureSize * textureSize];
        // Full siyah fog
        for (int i = 0; i < fogPixels.Length; i++)
            fogPixels[i] = new Color32(0, 0, 0, 255);

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();

        expandedMapFog.texture = fogTexture;

        lastPlayerPos = player.position;

    }

    void Update()
    {
        // Sadece oyuncu hareket edince güncelle
        if (Vector3.Distance(player.position, lastPlayerPos) > updateThreshold)
        {
            RevealAtPlayer();
            fogTexture.Apply();

            lastPlayerPos = player.position;
        }
    }

    void RevealAtPlayer()
    {
        Vector2 uv = WorldToUV(player.position);
        int pixelRadius = Mathf.Clamp(Mathf.RoundToInt(revealRadius * textureSize / worldSize), 1, textureSize);
        RevealCircle(uv, pixelRadius);
        Debug.Log("Revealing pixel: " + uv + " with radius: " + pixelRadius );
    }

    // 🔥 MERKEZ TABANLI MAPPING
    Vector2 WorldToUV(Vector3 pos)
    {
        float u = ((pos.x - mapOrigin.x) / worldSize) + 0.5f;
        float v = ((pos.z - mapOrigin.y) / worldSize) + 0.5f;

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
    }
}