using UnityEngine;

public class ElectrostaticForce: MonoBehaviour
{
    public Rigidbody playerRb;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] forceSounds;

    [Header("Pitch Random")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("Force Settings")]
    public float minForce = 3f;
    public float maxForce = 10f;

    [Header("Timing")]
    public float minInterval = 2f;
    public float maxInterval = 5f;

    [Header("Camera")]
    public CameraShake cameraShake;

    private float timer;

    public Transform nucleus;

    void PlayForceSound()
    {
        if (forceSounds.Length == 0) return;

        AudioClip clip = forceSounds[Random.Range(0, forceSounds.Length)];

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    void Start()
    {
        SetNewTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ApplyForce();
            SetNewTimer();
        }
    }
    void ApplyForce()
    {
        Vector3 direction = (playerRb.position - nucleus.position).normalized;

        direction += Random.insideUnitSphere * 0.3f; // hafif kaos

        float force = Random.Range(minForce, maxForce);

        playerRb.AddForce(direction.normalized * force, ForceMode.Impulse);

        float shakeAmount = force * 0.02f;
        cameraShake.Shake(0.2f, shakeAmount);

        PlayForceSound();
    }

    void SetNewTimer()
    {
        timer = Random.Range(minInterval, maxInterval);
    }
}