using UnityEngine;

public class AmbientThunder : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] thunderClips;

    public float minDelay = 5f;
    public float maxDelay = 15f;

    float timer;

    void Start()
    {
        SetNewTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayThunder();
            SetNewTimer();
        }
    }

    void PlayThunder()
    {
        if (thunderClips.Length == 0) return;

        AudioClip clip = thunderClips[Random.Range(0, thunderClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    void SetNewTimer()
    {
        timer = Random.Range(minDelay, maxDelay);
    }
}