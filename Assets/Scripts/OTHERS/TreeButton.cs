using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchToggle : MonoBehaviour
{
    public Button mainButton;                     // Ana buton
    public List<GameObject> childButtons;         // Alt butonlar
    public List<GameObject> lines;                // Çizgiler
    public float fadeDuration = 0.3f;             // Fade süresi

    private bool isOpen = false;

    void Start()
    {
        // Başta hepsi gizli
        HideObjects(childButtons);
        HideObjects(lines);

        mainButton.onClick.AddListener(ToggleChildren);
    }

    void ToggleChildren()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            ShowObjects(childButtons);
            ShowObjects(lines);
        }
        else
        {
            HideObjects(childButtons, true);
            HideObjects(lines, true);
        }
    }

    void ShowObjects(List<GameObject> objs)
    {
        foreach (var obj in objs)
        {
            obj.SetActive(true);
            StartCoroutine(FadeIn(obj));
        }
    }

    void HideObjects(List<GameObject> objs, bool withFade = false)
    {
        foreach (var obj in objs)
        {
            if (withFade)
            {
                StartCoroutine(FadeOut(obj));
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    IEnumerator FadeIn(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        cg.alpha = 0;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1;
    }

    IEnumerator FadeOut(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        cg.alpha = 1;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 0;
        obj.SetActive(false);
    }
}
