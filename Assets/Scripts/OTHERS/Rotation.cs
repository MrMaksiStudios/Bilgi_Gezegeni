using UnityEngine;

public class Rotation : MonoBehaviour
{
    public int spinState = 0; // 0: yukarı (↑), 1: aşağı (↓)
    private Draggable draggable;

    private void Start()
    {
        draggable = GetComponent<Draggable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && draggable.isBeingDragged && draggable != null)
        {
            RotateElectron();
        }
    }

    private void RotateElectron()
    {
        transform.Rotate(0f, 0f, 180f);
        spinState = 1 - spinState;
    }
}