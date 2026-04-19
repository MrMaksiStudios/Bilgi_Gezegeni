using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroPart2Controller : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;

    public Transform dPoint;
    public Transform pPoint;
    public Transform sPoint;

    public Transform coreStart;
    public Transform coreEnd;

    [Header("UI")]
    public Image blackImage;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource narrationSource;

    public AudioClip narration1; // starts at 26s
    public AudioClip narration2; // starts at 41s

    private bool movedToD, movedToP, movedToS, movedToCore;
    private bool playedNarration1, playedNarration2;

    public Transform lookTarget;
    public float lookSpeed = 5f;
    public GameObject d;
    public GameObject p;
    public GameObject s;
    public GameObject core;

    [Header("Logo & Text")]
    public CanvasGroup logoGroup;
    public CanvasGroup studioTextGroup;

    private bool startedLogoFadeIn, startedLogoFadeOut;
    private bool startedBlackFadeIn;
    private bool startedStudioText;
    private bool loadedScene;


    public void StartPart2()
    {
        StartCoroutine(PlayPart2());
    }


    void LateUpdate()
    {
        if (lookTarget == null) return;

        Vector3 direction = lookTarget.position - cam.position;
        //Quaternion targetRotation = Quaternion.LookRotation(direction);

        /*cam.rotation = Quaternion.Slerp(
            cam.rotation,
            targetRotation,
            Time.deltaTime * lookSpeed
        );*/
    }

    IEnumerator PlayPart2()
    {
        // --- Start music ---
        musicSource.Play();

        // --- Initial black screen (2s) ---
        blackImage.gameObject.SetActive(true);
        blackImage.color = Color.black;

        //lookTarget = d;
        lookTarget = d.transform;
        SetCameraInstant(dPoint);

        yield return new WaitForSeconds(2f);

        // Fade out black
        StartCoroutine(FadeOut(3f));

        // Start moving to D orbital
        StartCoroutine(MoveTo(dPoint, 5f));

        // MAIN LOOP (sync with music)
        while (musicSource.isPlaying)
        {
            float t = musicSource.time;

            // --- 9s → P orbital ---
            if (t >= 9f && !movedToP)
            {
                movedToP = true;
                StartCoroutine(TransitionTo(pPoint));
            }

            // --- 16s → S orbital ---
            if (t >= 16f && !movedToS)
            {
                movedToS = true;
                StartCoroutine(TransitionTo(sPoint));
            }

            // --- 24s → Core ---
            if (t >= 24f && !movedToCore)
            {
                movedToCore = true;
                StartCoroutine(MoveCore());
            }

            // --- 26s → Narration 1 ---
            if (t >= 26f && !playedNarration1)
            {
                playedNarration1 = true;
                narrationSource.clip = narration1;
                narrationSource.Play();
            }

            // --- 41s → Narration 2 ---
            if (t >= 41f && !playedNarration2)
            {
                playedNarration2 = true;
                narrationSource.clip = narration2;
                narrationSource.Play();
            }

            if (t >= 43f && !startedBlackFadeIn)
            {
                startedBlackFadeIn = true;
                StartCoroutine(FadeIn(2f));
            }

            if (t >= 45f && !startedLogoFadeIn)
            {
                startedLogoFadeIn = true;
                StartCoroutine(FadeCanvasGroup(logoGroup, 0f, 1f, 2f));
            }

            if (t >= 48f && !startedLogoFadeOut)
            {
                startedLogoFadeOut = true;
                StartCoroutine(FadeCanvasGroup(logoGroup, 1f, 0f, 1.5f));
            }

            if (t >= 51f && !startedStudioText)
            {
                startedStudioText = true;
                StartCoroutine(FadeCanvasGroup(studioTextGroup, 0f, 1f, 2f));
            }

            if (t >= 57f && startedStudioText)
            {
                StartCoroutine(FadeCanvasGroup(studioTextGroup, 1f, 0f, 2f));
            }

            if (t >= 60f && !loadedScene)
            {
                loadedScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("OrbitalRPG");
            }

            yield return null;
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float smoothT = Mathf.SmoothStep(0f, 1f, t / duration);
            cg.alpha = Mathf.Lerp(start, end, smoothT);

            yield return null;
        }

        cg.alpha = end;
    }

    IEnumerator TransitionTo(Transform target)
    {
        // Fade in (go black)
        yield return StartCoroutine(FadeIn(1f));

        // Set correct look target
        if (target == dPoint) lookTarget = d.transform;
        else if (target == pPoint) lookTarget = p.transform;
        else if (target == sPoint) lookTarget = s.transform;

        // Teleport while hidden
        SetCameraInstant(target);

        yield return new WaitForSeconds(0.2f);

        // Fade out slowly (cinematic)
        yield return StartCoroutine(FadeOut(2f));

        // SMALL drift instead of big movement (feels better)
        yield return StartCoroutine(MoveTo(target, 2f));
    }

    IEnumerator MoveCore()
    {
        // Fade to black
        yield return StartCoroutine(FadeIn(1.5f));

        lookTarget = core.transform;

        // Teleport while hidden
        SetCameraInstant(coreStart);

        yield return new WaitForSeconds(0.2f);

        // Fade out (reveal core)
        yield return StartCoroutine(FadeOut(2f));

        // Slow cinematic approach
        yield return StartCoroutine(MoveBetween(coreStart, coreEnd, 10f));
    }

    void SetCameraInstant(Transform target)
    {
        cam.position = target.position;
        cam.rotation = target.rotation;
    }

    IEnumerator MoveTo(Transform target, float duration)
    {
        Vector3 startPos = cam.position;
        Vector3 endPos = target.position;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // Smooth easing (VERY IMPORTANT)
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            cam.position = Vector3.Lerp(startPos, endPos, smoothT);

            yield return null;
        }
    }

    IEnumerator MoveBetween(Transform a, Transform b, float duration)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            cam.position = Vector3.Lerp(a.position, b.position, smoothT);

            yield return null;
        }
    }

    IEnumerator FadeIn(float duration)
    {
        blackImage.gameObject.SetActive(true);

        float t = 0f;
        Color c = blackImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            blackImage.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOut(float duration)
    {
        float t = 0f;
        Color c = blackImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;

            float smoothT = Mathf.SmoothStep(0f, 1f, t / duration);
            c.a = 1 - smoothT;

            blackImage.color = c;
            yield return null;
        }

        blackImage.gameObject.SetActive(false);
    }
}