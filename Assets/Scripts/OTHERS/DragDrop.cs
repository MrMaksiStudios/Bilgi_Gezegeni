/*using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;

    void OnMouseDown()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // 2D için z = 0
            transform.position = mousePos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Etrafında başka obje var mı kontrol et
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                // ChemicalSpawner’a haber ver
                ChemicalSpawner spawner = FindObjectOfType<ChemicalSpawner>();
                spawner.CheckBond(gameObject, hit.gameObject);
                return;
            }
        }

        // Hiçbir şeyle birleşmediyse eski yerine dön
        transform.position = startPosition;
    }
}
*/