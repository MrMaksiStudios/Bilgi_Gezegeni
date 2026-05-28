using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipIntro : MonoBehaviour
{
    public Button skipButton;
    public CanvasGroup buttonGroup;

    private bool hasTouched = false;
    private bool buttonVisible = false;
    private float buttonTimer = 0f;

    void Update()
    {
        // Check for touch input (mobile) or mouse click (Editor testing)
        bool touchDetected = Input.touchCount > 0 || Input.GetMouseButtonDown(0);

        if (!hasTouched && touchDetected)
        {

            hasTouched = true;
            StartCoroutine(FadeInButton());
        }

        if (buttonVisible)
        {
            buttonTimer += Time.deltaTime;
            if (buttonTimer >= 2f)
            {
                buttonVisible = false;
                StartCoroutine(FadeOutButton());
            }
        }
    }

    public void OnSkipButtonPressed()
    {
        // Skip intro by loading the main scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("OrbitalRPG");
    }

    IEnumerator FadeInButton()
    {
        buttonGroup.gameObject.SetActive(true);
        skipButton.interactable = true;
        float duration = 0.5f; // Fade in duration
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            buttonGroup.alpha = t / duration;
            yield return null;
        }
        buttonGroup.alpha = 1f;
        buttonVisible = true;
        buttonTimer = 0f;
    }

    IEnumerator FadeOutButton()
    {
        skipButton.interactable = false;
        float duration = 0.5f; // Fade out duration
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            buttonGroup.alpha = 1 - (t / duration);
            yield return null;
        }
        buttonGroup.alpha = 0f;
        buttonGroup.gameObject.SetActive(false);
        hasTouched = false; // Reset for potential future intros (if player returns to main menu)
    }
}
