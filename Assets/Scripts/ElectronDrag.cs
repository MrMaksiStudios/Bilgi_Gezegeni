using UnityEngine;

public class ElectronDrag : MonoBehaviour
{
    private bool placed;

    void OnMouseDrag()
    {
        if (placed) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    public void Place(Vector3 position)
    {
        placed = true;
        transform.position = position;
    }
}
