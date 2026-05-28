using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ForceDrag2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private LineRenderer lr;

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isDragging = false;

    public float forceMultiplier = 5f;

    public TMP_Text forceText;
    public TMP_Text accelerationText;
    public TMP_Text speedText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        speedText.text = "v = " + "0.00" + " m/s";
        accelerationText.text = "a = " + "0.00" + " m/s²";
        forceText.text = "F = " + 0f.ToString("F1") + " N";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
                
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            lr.enabled = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, endPos);

            float distance = Vector2.Distance(startPos, endPos);
            distance = Mathf.Clamp(distance, 0, 5f);
            float force = Mathf.Round(distance * forceMultiplier);
            forceText.text = "F = " + force.ToString("F1") + " N";
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            lr.enabled = false;

            float distance = Vector2.Distance(startPos, endPos);
            distance = Mathf.Clamp(distance, 0, 5f);
            float force = Mathf.Round(distance * forceMultiplier);

            Vector2 direction = (endPos - startPos).normalized;
            rb.AddForce(direction * force, ForceMode2D.Impulse);

            float acceleration = force / rb.mass;
            accelerationText.text = "a = " + acceleration.ToString("F2") + " m/s²";
            float speed = rb.velocity.magnitude;
            speedText.text = "v = " + speed.ToString("F2") + " m/s";
        }
    }
}