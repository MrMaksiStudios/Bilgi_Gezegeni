using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrictionDisplay : MonoBehaviour
{
    public PhysicsMaterial2D material;
    public TMP_Text frictionText;

    public PhysicsMaterial2D originalMaterial;
    private PhysicsMaterial2D runtimeMaterial;
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();  

        // Asset'in kopyasını oluşturuyoruz
        runtimeMaterial = new PhysicsMaterial2D();
        runtimeMaterial.friction = originalMaterial.friction;
        runtimeMaterial.bounciness = originalMaterial.bounciness;

        col.sharedMaterial = runtimeMaterial;
    }

    public void ChangeFriction(float value)
    {
        runtimeMaterial.friction = value;
        frictionText.text = "Friction = " + value.ToString("F2");
    }
}