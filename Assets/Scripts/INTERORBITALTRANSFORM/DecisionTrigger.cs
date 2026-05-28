using UnityEngine;

public class DecisionTrigger : MonoBehaviour
{
    [Header("Lane Objects")]
    public GameObject leftLane;      // Lane 0
    public GameObject middleLane;    // Lane 1
    public GameObject rightLane;     // Lane 2

    [Header("Correct Lane")]
    public int correctLane = 1; // 0=left, 1=middle, 2=right

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            used = true;
            Debug.Log($"[TRIGGER] Decision trigger activated! Correct lane is: {correctLane}");
            LaneDecisionManager.Instance.ReachDecision(this);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    // 🧠 DETECT PLAYER LANE BASED ON ASSIGNED OBJECTS
    public int GetPlayerLane()
    {
        Vector3 playerPos = LaneDecisionManager.Instance.playerMovement.transform.position;

        // Calculate distance to each lane object
        float distToLeft = Vector3.Distance(playerPos, leftLane.transform.position);
        float distToMiddle = Vector3.Distance(playerPos, middleLane.transform.position);
        float distToRight = Vector3.Distance(playerPos, rightLane.transform.position);

        // Determine closest lane
        if (distToLeft < distToMiddle && distToLeft < distToRight)
            return 0; // left
        else if (distToRight < distToMiddle && distToRight < distToLeft)
            return 2; // right
        else
            return 1; // middle

    }
}