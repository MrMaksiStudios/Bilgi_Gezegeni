using UnityEngine;

public class ElectronRandomMotion : MonoBehaviour
{
    public float speed = 2f;
    public float randomness = 1.5f;

    public Collider2D movementArea;

    private Vector2 currentDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        // yönü biraz rastgele değiştir
        currentDirection += Random.insideUnitCircle * randomness * Time.fixedDeltaTime;
        currentDirection.Normalize();

        rb.velocity = currentDirection * speed;

        KeepInsideArea();
    }

    void KeepInsideArea()
    {
        if (!movementArea.bounds.Contains(transform.position))
        {
            Vector2 centerDir = (movementArea.bounds.center - transform.position).normalized;
            currentDirection = centerDir;
        }
    }
}