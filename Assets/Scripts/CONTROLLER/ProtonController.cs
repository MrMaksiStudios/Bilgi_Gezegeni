using UnityEngine;
using System.Collections.Generic;

public class ProtonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float movementMultiplier = 1f;
    public float acceleration = 10f;
    public float airControl = 0.5f;

    public MobileDpadInput mobileInput;

    [Header("Jump")]
    public float jumpForce = 6f;
    public float extraGravity = 20f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    [SerializeField] private float coyoteCounter;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip moveSound;
    public AudioClip DashSound;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashCooldown = 1f;
    [SerializeField] private float dashTimer;
    private float trailResetTimer = 0f;

    [Header("Audio Variation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private Rigidbody rb;
    private TrailRenderer trailRenderer;
    [SerializeField] private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (PauseManager.IsPaused) return;

        HandleCoyoteTime();

        dashTimer -= Time.deltaTime;
        trailResetTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            Dash();
            dashTimer = dashCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0f)
        {
            HandleJump();
        }

        // Reset trail if timer reached
        if (trailResetTimer <= 0f && trailRenderer != null)
        {
            trailRenderer.time = 0.4f;
            trailResetTimer = -1f; // Only do this once
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetMovementMultiplier(float multiplier)
    {
        movementMultiplier = Mathf.Clamp01(multiplier);
    }

    public void ResetMovementMultiplier()
    {
        movementMultiplier = 1f;
    }

    public void ResetControllerState()
    {
        // Reset timers and state when loading
        dashTimer = 0f;
        coyoteCounter = coyoteTime;
        isGrounded = true; // Assume grounded after loading
        trailResetTimer = -1f;
        movementMultiplier = 1f;
        
        if (rb != null)
        {
            // Reset vertical velocity but keep horizontal
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    void FixedUpdate()
    {
        Move();
        ApplyExtraGravity();

        if (isGrounded && rb.velocity.magnitude > 1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(moveSound);
            }
        }
    }

    void Move()
    {
        float hKeyboard = Input.GetAxis("Horizontal");
        float vKeyboard = Input.GetAxis("Vertical");

        float hMobile = mobileInput != null ? mobileInput.InputVector.x : 0;
        float vMobile = mobileInput != null ? mobileInput.InputVector.y : 0;

        float h = Mathf.Abs(hMobile) > 0 ? hMobile : hKeyboard;
        float v = Mathf.Abs(vMobile) > 0 ? vMobile : vKeyboard;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        float control = isGrounded ? 1f : airControl;

        Vector3 targetVelocity = moveDir * moveSpeed * movementMultiplier * control;
        Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);

        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * 10f);
        }
    }

    public void Dash()
    {
        if (PauseManager.IsPaused) return;
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (dashTimer <= 0f)
        {
            Vector3 dashDirection = transform.forward;
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

            PlayRandomPitchSound(DashSound);

            if (trailRenderer == null)
                trailRenderer = GetComponent<TrailRenderer>();

            if (trailRenderer != null)
            {
                trailRenderer.time = 0.8f;
                trailResetTimer = 0.2f;
            }

            dashTimer = dashCooldown;
        }
    }

    void PlayRandomPitchSound(AudioClip clip)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    public void HandleJump()
    {
        if (PauseManager.IsPaused) return;
        if (rb == null) rb = GetComponent<Rigidbody>();
        
        if (coyoteCounter > 0f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            coyoteCounter = 0f;
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void ApplyExtraGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraGravity);
        }
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


}