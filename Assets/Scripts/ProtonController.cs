using UnityEngine;

public class ProtonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float acceleration = 10f;
    public float airControl = 0.5f;

    public MobileDpadInput mobileInput;

    [Header("Jump")]
    public float jumpForce = 6f;
    public float extraGravity = 20f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteCounter;

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
    private float dashTimer;

    [Header("Audio Variation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleCoyoteTime();

        dashTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            Dash();
            dashTimer = dashCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0f)
        {
            HandleJump();
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

        Vector3 targetVelocity = moveDir * moveSpeed * control;
        Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);

        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * 10f);
        }
    }

    public void Dash()
    {
        Vector3 dashDirection = transform.forward;
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        PlayRandomPitchSound(DashSound);

        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.time = 0.8f;
        Invoke("ResetTrail", 0.2f);
    }

    void ResetTrail()
    {
        GetComponent<TrailRenderer>().time = 0.4f;
    }

    void PlayRandomPitchSound(AudioClip clip)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    public void HandleJump()
    {
        
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y , rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        coyoteCounter = 0f;

        audioSource.PlayOneShot(jumpSound);
        
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