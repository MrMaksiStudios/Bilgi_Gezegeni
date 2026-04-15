using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneIndicatorManager : MonoBehaviour
{
    [Header("Zone Data")]
    public List<ZoneInfo> zones = new List<ZoneInfo>();

    [Header("UI")]
    public CanvasGroup popupGroup;
    public TextMeshProUGUI popupText;

    [Header("Settings")]
    public float displayTime = 3f;
    public float fadeSpeed = 6f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip popupSFX;

    private Coroutine currentRoutine;

    void Start()
    {
        // Register triggers automatically
        foreach (var zone in zones)
        {
            ZoneTrigger trigger = zone.triggerZone.gameObject.AddComponent<ZoneTrigger>();
            trigger.Init(this, zone.zoneText);
        }

        popupGroup.alpha = 0;
        popupGroup.transform.localScale = Vector3.one * 0.8f;
    }

    public void ShowZone(string text)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(text));
    }

    IEnumerator ShowRoutine(string text)
    {
        popupText.text = text;

        if (audioSource && popupSFX)
            audioSource.PlayOneShot(popupSFX);

        // Fade in + scale up
        popupGroup.gameObject.SetActive(true);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            popupGroup.alpha = t;
            popupGroup.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            popupGroup.alpha = t;
            popupGroup.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);
            yield return null;
        }

        popupGroup.gameObject.SetActive(false);
    }
}