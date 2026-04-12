using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManualFriction2D : MonoBehaviour
{
    public float frictionCoefficient = 0.2f;
    public TMP_Text frictionText;

    private Rigidbody2D rb;
    private bool isTouchingSurface = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateFrictionText();
    }

    void FixedUpdate()
    {
        if (isTouchingSurface && rb.velocity.magnitude > 0.01f)
        {
            float normalForce = rb.mass * Physics2D.gravity.magnitude;
            float frictionForceMagnitude = frictionCoefficient * normalForce;

            Vector2 frictionDirection = -rb.velocity.normalized;

            rb.AddForce(frictionDirection * frictionForceMagnitude);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isTouchingSurface = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingSurface = false;
    }

    public void ChangeFriction(float value)
    {
        frictionCoefficient = value;
        UpdateFrictionText();
    }

    void UpdateFrictionText()
    {
        if (frictionText != null)
            frictionText.text = "Sürtünme = " + frictionCoefficient.ToString("F2");
    }
}