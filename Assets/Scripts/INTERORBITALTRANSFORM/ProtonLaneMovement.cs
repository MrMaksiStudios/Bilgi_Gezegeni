using UnityEngine;

public class ProtonLaneMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 10f;

    [Header("Lane Settings")]
    public float laneDistance = 3f; // distance between lanes
    public float laneChangeSpeed = 10f;

    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private float targetX;

    void Start()
    {
        UpdateTargetPosition();
    }

    void Update()
    {
        // 🚀 Constant forward movement
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 🎯 Smooth lane movement
        Vector3 pos = transform.position;
        float newX = Mathf.Lerp(pos.x, targetX, laneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, pos.y, pos.z);
    }

    // 👉 Call this from your UI buttons
    public void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
            UpdateTargetPosition();
        }
    }
    public void MoveRight()
    {
        if (currentLane < 2)
        {
            currentLane++;
            UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition()
    {
        // Convert lane index to position
        // Lane 0 = left, 1 = center, 2 = right
        targetX = (currentLane - 1) * laneDistance;
    }
    public int GetCurrentLane()
    {
        return currentLane;
    }
}