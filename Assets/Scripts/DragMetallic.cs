using UnityEngine;

public class DragMetallic : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;

    private Rigidbody2D rb;
    private ElectronRandomMotion seaMotion;

    private bool isPlaced = false;
    bool isDragging;
    public Collider2D electronArea;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        seaMotion = GetComponent<ElectronRandomMotion>();

        seaMotion.enabled = false; // başlangıçta kapalı
    }

    void OnMouseDown()
    {
        if (isPlaced) return;

        isDragging = true;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;

        rb.velocity = Vector2.zero;
    }

    void OnMouseDrag()
    {
        if (isPlaced) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos + offset;
    }

    void OnMouseUp()
    {
        if (isPlaced) return;
    }

    void PlaceElectron()
    {
        isPlaced = true;

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        seaMotion.enabled = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == electronArea)
        {
            PlaceElectron();
        }
    }
}