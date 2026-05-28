using UnityEngine;
using System.Collections;

public class ArrivalFade : MonoBehaviour
{
    public CanvasGroup blackOverlay;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float n = t / fadeDuration;

            blackOverlay.alpha = 1f - n;

            yield return null;
        }

        blackOverlay.alpha = 0f;
    }
}