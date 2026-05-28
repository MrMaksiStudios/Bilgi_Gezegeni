using System.Collections.Generic;
using UnityEngine;

public class MagneticClimbZone : MonoBehaviour
{
    [Header("Magnetic Force")]
    public float upwardForce = 5f;

    [Header("Movement")]
    public bool allowHorizontalMovement = true;
    public float horizontalSpeedMultiplier = 0.5f;

    [Header("Visuals")]
    public Material arrowMaterial;
    public Color arrowColor = new Color(0.1f, 0.8f, 1f, 0.8f);
    public int arrowColumns = 3;
    public int arrowRows = 2;
    public int arrowLayers = 2;
    public float arrowScale = 0.4f;
    public float arrowSpeed = 1.2f;

    private bool playerInside = false;

    private Rigidbody playerRb;
    private ProtonController playerMovement;
    private readonly List<ArrowInstance> arrowInstances = new List<ArrowInstance>();
    private float arrowLoopHeight = 1f;

    private struct ArrowInstance
    {
        public Transform transform;
        public Vector3 baseLocalPosition;
        public float phase;

        public ArrowInstance(Transform transform, Vector3 baseLocalPosition, float phase)
        {
            this.transform = transform;
            this.baseLocalPosition = baseLocalPosition;
            this.phase = phase;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            playerRb = other.GetComponent<Rigidbody>();
            playerMovement = other.GetComponent<ProtonController>();

            if (playerRb != null)
            {
                // Disable gravity while inside the magnetic field
                playerRb.useGravity = false;
                playerRb.drag = 3f; // softer floaty feel
            }

            if (playerMovement != null)
            {
                playerMovement.SetMovementMultiplier(allowHorizontalMovement ? horizontalSpeedMultiplier : 0f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerRb != null)
        {
            playerInside = false;

            // Restore gravity
            playerRb.useGravity = true;
            playerRb.drag = 0f;

            if (playerMovement != null)
            {
                playerMovement.ResetMovementMultiplier();
            }

            playerRb = null;
            playerMovement = null;
        }
    }

    void Start()
    {
        InitializeArrowVisuals();
    }

    void Update()
    {
        AnimateArrows();
    }

    void FixedUpdate()
    {
        if (!playerInside || playerRb == null) return;

        // Apply a steady upward magnetic pull
        playerRb.AddForce(Vector3.up * upwardForce, ForceMode.Acceleration);
    }

    private void InitializeArrowVisuals()
    {
        Collider zoneCollider = GetComponent<Collider>();
        if (zoneCollider == null)
        {
            Debug.LogWarning("MagneticClimbZone requires a Collider to generate visual arrows.");
            return;
        }

        if (!zoneCollider.isTrigger)
        {
            zoneCollider.isTrigger = true;
        }

        Material material = null;
        if (arrowMaterial != null)
        {
            material = new Material(arrowMaterial);
            material.color = arrowColor;
        }
        else
        {
            Shader shader = Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
            if (shader == null)
            {
                Debug.LogWarning("Unable to find a shader for magnetic arrow visuals.");
                return;
            }

            material = new Material(shader) { color = arrowColor };
        }

        Vector3 center = Vector3.zero;
        Vector3 size = Vector3.one;

        if (zoneCollider is BoxCollider box)
        {
            center = box.center;
            size = box.size;
        }
        else
        {
            center = transform.InverseTransformPoint(zoneCollider.bounds.center);
            size = transform.InverseTransformDirection(zoneCollider.bounds.size);
        }

        float xStep = size.x / (arrowColumns + 1);
        float zStep = size.z / (arrowRows + 1);
        arrowLoopHeight = Mathf.Max(0.1f, size.y * 0.8f);
        float yBottom = center.y - size.y / 2f + 0.12f;

        for (int layer = 0; layer < arrowLayers; layer++)
        {
            for (int x = 0; x < arrowColumns; x++)
            {
                for (int z = 0; z < arrowRows; z++)
                {
                    Vector3 localPos = new Vector3(
                        center.x - size.x / 2f + xStep * (x + 1),
                        yBottom + layer * (arrowLoopHeight / arrowLayers),
                        center.z - size.z / 2f + zStep * (z + 1)
                    );

                    GameObject arrowRoot = CreateArrowRoot(localPos, material);
                    arrowInstances.Add(new ArrowInstance(arrowRoot.transform, localPos, Random.Range(0f, arrowLoopHeight)));
                }
            }
        }
    }

    private GameObject CreateArrowRoot(Vector3 localPosition, Material material)
    {
        GameObject root = new GameObject("MagneticArrow");
        root.transform.SetParent(transform, false);
        root.transform.localPosition = localPosition;
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one * arrowScale;

        GameObject shaft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        shaft.name = "ArrowShaft";
        shaft.transform.SetParent(root.transform, false);
        shaft.transform.localPosition = new Vector3(0f, 0.12f, 0f);
        shaft.transform.localScale = new Vector3(0.12f, 0.3f, 0.12f);
        DestroyImmediate(shaft.GetComponent<Collider>());
        var shaftRenderer = shaft.GetComponent<Renderer>();
        shaftRenderer.material = material;

        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.name = "ArrowHead";
        head.transform.SetParent(root.transform, false);
        head.transform.localPosition = new Vector3(0f, 0.34f, 0f);
        head.transform.localScale = new Vector3(0.24f, 0.16f, 0.12f);
        head.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
        DestroyImmediate(head.GetComponent<Collider>());
        var headRenderer = head.GetComponent<Renderer>();
        headRenderer.material = material;

        return root;
    }

    private void AnimateArrows()
    {
        if (arrowInstances.Count == 0) return;

        for (int i = 0; i < arrowInstances.Count; i++)
        {
            ArrowInstance arrowInstance = arrowInstances[i];
            Vector3 localPos = arrowInstance.baseLocalPosition;
            localPos.y = arrowInstance.baseLocalPosition.y + Mathf.Repeat(Time.time * arrowSpeed + arrowInstance.phase, arrowLoopHeight);
            arrowInstance.transform.localPosition = localPos;
            arrowInstances[i] = arrowInstance;
        }
    }
}