using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WarpController : MonoBehaviour
{
    public static WarpController Instance;

    [Header("Warp")]
    public RectTransform warpTransform;
    public CanvasGroup warpGroup;

    [Header("Black Fade")]
    public CanvasGroup blackOverlay;

    [Header("UI")]
    public CanvasGroup[] uiGroups;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip whooshSound;

    [Header("Timing")]
    public float warpDuration = 0.8f;
    public float blackFadeDuration = 0.4f;
    public float waitBeforeLoad = 1f;

    [Header("Scale")]
    public float startScale = 0.6f;
    public float endScale = 4f;

    public Transform player;
    public ZoneIndicatorManager ZoneIndicatorManager;
    public List<UIEditable> uiElements;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartWarp(string sceneName)
    {
        StartCoroutine(WarpRoutine(sceneName));
    }

    IEnumerator WarpRoutine(string sceneName)
    {
        float t = 0;

        // Initial states
        warpGroup.alpha = 1f;
        warpTransform.localScale = Vector3.one * startScale;

        blackOverlay.alpha = 0f;

        // Play sound
        if (audioSource && whooshSound)
            audioSource.PlayOneShot(whooshSound);

        // 🚀 WARP PHASE
        while (t < warpDuration)
        {
            t += Time.deltaTime;
            float n = t / warpDuration;

            // Scale up
            float scale = Mathf.Lerp(startScale, endScale, n);
            warpTransform.localScale = Vector3.one * scale;

            // Optional rotation (feels much better)
            warpTransform.Rotate(0, 0, 300 * Time.deltaTime);

            // Fade UI out
            foreach (var group in uiGroups)
            {
                if (group != null)
                    group.alpha = 1f - n;
            }

            yield return null;
        }

        // 🌑 BLACK FADE PHASE
        t = 0;

        while (t < blackFadeDuration)
        {
            t += Time.deltaTime;
            float n = t / blackFadeDuration;

            blackOverlay.alpha = n;

            yield return null;
        }

        blackOverlay.alpha = 1f;

        // ⏳ WAIT (very important for feel)
        yield return new WaitForSeconds(waitBeforeLoad);

        // 🎬 LOAD SCENE
        SceneManager.LoadScene(sceneName);
        //blackOverlay.alpha = 0f;
        //warpGroup.alpha = 0f;
    }

    public void StartWarpBack()
    {
        StartCoroutine(WarpBackRoutine());
    }

    IEnumerator WarpBackRoutine()
    {
        float t = 0;

        // Reset visuals
        warpGroup.alpha = 1f;
        blackOverlay.alpha = 0f;
        warpTransform.localScale = Vector3.one * startScale;

        // Play sound
        if (audioSource && whooshSound)
            audioSource.PlayOneShot(whooshSound);

        // 🚀 Warp animation (same as before)
        while (t < warpDuration)
        {
            t += Time.deltaTime;
            float n = t / warpDuration;

            float scale = Mathf.Lerp(startScale, endScale, n);
            warpTransform.localScale = Vector3.one * scale;

            warpTransform.Rotate(0, 0, 300 * Time.deltaTime);

            yield return null;
        }

        // 🌑 Black fade
        t = 0;
        while (t < blackFadeDuration)
        {
            t += Time.deltaTime;
            float n = t / blackFadeDuration;

            blackOverlay.alpha = n;

            yield return null;
        }

        blackOverlay.alpha = 1f;

        //SaveManager.Instance.SaveGame(player, ZoneIndicatorManager.zones);

        // VERY IMPORTANT
        PlayerPrefs.SetInt("ShouldLoad", 1);
        PlayerPrefs.Save();

        // ⏳ WAIT
        yield return new WaitForSeconds(waitBeforeLoad);

        // 🎬 LOAD MAIN SCENE
        SceneManager.LoadScene("OrbitalRPG");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetWarpVisuals();
    }

    void ResetWarpVisuals()
    {
        // Hide everything safely
        warpTransform.localScale = Vector3.one * startScale;

        warpGroup.alpha = 0f;
        blackOverlay.alpha = 0f;

        // Optional: stop rotation if any
        warpTransform.rotation = Quaternion.identity;
    }
}