using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MassController : MonoBehaviour
{
    public Rigidbody2D rb;
    public TMP_Text massText;

    public void ChangeMass(float value)
    {
        rb.mass = value;
        massText.text = "Kütle = " + value.ToString("F1") + " kg";
    }
}