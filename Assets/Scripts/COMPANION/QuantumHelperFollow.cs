using UnityEngine;

public class QuantumHelperFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Positioning")]
    public float backDistance = 1.2f;
    public float leftDistance = 0.8f;
    public float heightOffset = 1.4f;

    [Header("Movement")]
    public float followSpeed = 5f;
    public float movementThreshold = 0.05f;

    [Header("Idle Motion")]
    public float idleDelay = 2f;
    public float idleOrbitSpeed = 1.4f;
    public float idleOrbitRadius = 1.0f;
    public float idleOrbitRadiusVariation = 0.35f;
    public float idleOrbitRadiusFrequency = 0.65f;
    public float idleBounceAmplitude = 0.28f;
    public float idleBounceFrequency = 2.1f;

    private Vector3 velocity;
    private Vector3 previousPlayerPosition;
    private float idleTimer;
    private float idlePhase;

    void Start()
    {
        if (player != null)
        {
            previousPlayerPosition = player.position;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        float movementDistance = Vector3.Distance(player.position, previousPlayerPosition);

        if (movementDistance > movementThreshold)
        {
            idleTimer = 0f;
            idlePhase = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }

        previousPlayerPosition = player.position;

        Vector3 targetPos = player.position
            - player.forward * backDistance
            - player.right * leftDistance
            + Vector3.up * heightOffset;

        if (idleTimer >= idleDelay)
        {
            idlePhase += Time.deltaTime * idleOrbitSpeed;

            float currentRadius = idleOrbitRadius
                + Mathf.Sin(idlePhase * idleOrbitRadiusFrequency) * idleOrbitRadiusVariation;

            float currentBounce = Mathf.Sin(idlePhase * idleBounceFrequency) * idleBounceAmplitude;

            Vector3 orbitCenter = player.position + Vector3.up * heightOffset;
            Vector3 rightDir = Vector3.ProjectOnPlane(player.right, Vector3.up).normalized;
            Vector3 forwardDir = Vector3.ProjectOnPlane(player.forward, Vector3.up).normalized;

            if (rightDir == Vector3.zero)
            {
                rightDir = Vector3.right;
            }

            if (forwardDir == Vector3.zero)
            {
                forwardDir = Vector3.forward;
            }

            Vector3 orbitOffset = Mathf.Cos(idlePhase) * rightDir * currentRadius
                + Mathf.Sin(idlePhase) * forwardDir * currentRadius;

            targetPos = orbitCenter + orbitOffset + Vector3.up * currentBounce;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            1f / followSpeed
        );

        transform.rotation = Quaternion.identity;
    }
}