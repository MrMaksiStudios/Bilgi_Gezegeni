using UnityEngine;

public class Draggable : MonoBehaviour
{
    public AtomGame gameController;
    public int protonValue;
    public int electronValue;
    public bool isElectron = false;
    Vector2 difference = Vector2.zero;
    public Vector2 startPosition;
    public bool isBeingDragged = false;
    
    void Start()
    {
        GetValue();
    }
    public int GetValue()
    {
        return isElectron ? electronValue : protonValue;
    }
    public void SetGameController(AtomGame controller)
    {
        gameController = controller;
    }
    private void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        difference = (Vector2)mousePosition - (Vector2)transform.position;
    }
    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = (Vector2)mousePosition - difference;
        isBeingDragged = true;
    }
    public void LockToPosition(Vector3 pos)
    {
        transform.position = pos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    private void OnMouseUp()
    {
        isBeingDragged = false;
    }
    public void ResetToStart()
    {
        transform.position = startPosition;
    }
    public void ResetToStartPosition()
    {
        transform.position = startPosition;
    }
}
