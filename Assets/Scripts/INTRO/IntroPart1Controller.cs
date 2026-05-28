using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroPart1Controller : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;
    public Transform cameraStart;
    public Transform cameraEnd;
    public Transform proton;

    [Header("UI")]
    public Image blackImage;
    public TextMeshProUGUI text;
    public IntroPart2Controller part2Controller;
    public GameObject Holder;
    public GameObject text1;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip narration1;
    public AudioClip narration2;
    public AudioClip narration3;



    void Start()
    {
        StartCoroutine(PlayPart1());
    }

    IEnumerator PlayPart1()
    {
        // --- 1. Black screen 2 seconds ---
        blackImage.color = Color.black;
        blackImage.gameObject.SetActive(true);
        text.text = "";

        yield return new WaitForSeconds(2f);

        // --- 2. Fade out black image ---
        yield return StartCoroutine(FadeOutBlack(1f));

        blackImage.gameObject.SetActive(false);

        // --- 3. Camera moves to proton (12s) + narration1 ---
        audioSource.clip = narration1;
        audioSource.Play();

        yield return StartCoroutine(MoveCamera(12f));

        // --- 4. Wait 1 second ---
        yield return new WaitForSeconds(1f);

        // --- 5. Narration 2 (2s) ---
        audioSource.clip = narration2;
        audioSource.Play();

        yield return new WaitForSeconds(2f);

        // --- 6. Black screen pops in ---
        blackImage.gameObject.SetActive(true);
        blackImage.color = Color.black;

        // --- 7. Wait 1 second ---
        yield return new WaitForSeconds(1f);

        // --- 8. Show text + narration3 ---
        StartCoroutine(WaveText("Burası orbitaller!"));

        audioSource.clip = narration3;
        audioSource.Play();

        yield return new WaitForSeconds(3f);

        // --- End (extra 1 sec hold for total 4s) ---
        yield return new WaitForSeconds(1f);

        // STOP HERE (end of part 1)
        // Start Part 2
        part2Controller.StartPart2();
        Holder.SetActive(true);
        text1.SetActive(false);
        
    }

    IEnumerator MoveCamera(float duration)
    {
        float t = 0f;

        Vector3 startPos = cameraStart.position;
        Vector3 endPos = cameraEnd.position;

        Quaternion startRot = cameraStart.rotation;
        Quaternion endRot = cameraEnd.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            cam.position = Vector3.Lerp(startPos, endPos, t);
            cam.rotation = Quaternion.Slerp(startRot, endRot, t);

            // Always look at proton (extra safety)
            cam.LookAt(proton);

            yield return null;
        }
    }

    IEnumerator FadeOutBlack(float duration)
    {
        float t = 0f;
        Color c = blackImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1 - (t / duration);
            blackImage.color = c;
            yield return null;
        }
    }

    IEnumerator WaveText(string message)
    {
        text.text = message;

        //float time = 0f;

        while (true)
        {
            text.ForceMeshUpdate();
            var textInfo = text.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int index = charInfo.vertexIndex;
                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                // Slower, more subtle wave for a cinematic, serious (horror-like) effect
                float wave = Mathf.Sin(Time.time * 0.5f + i * 0.1f) * 1.5f;

                vertices[index + 0].y += wave;
                vertices[index + 1].y += wave;
                vertices[index + 2].y += wave;
                vertices[index + 3].y += wave;
            }

            text.UpdateVertexData();

            yield return null;
        }
    }
}