using UnityEngine;
using UnityEngine.UI;

public class LaneDecisionManager : MonoBehaviour
{
    public static LaneDecisionManager Instance;

    [Header("Player")]
    public ProtonController playerMovement;

    [Header("Speed")]
    public float slowSpeed = 2f;
    public float normalSpeed = 8f;

    [Header("Timer")]
    public Slider timerSlider;
    public float decisionDuration = 3f;

    [Header("Obstacle Courses")]
    public GameObject sOrbitalCourse;
    public GameObject pOrbitalCourse;
    public GameObject dOrbitalCourse;
    public GameObject specialCourse;

    private GameObject currentCourse;

    private bool inDecision = false;
    private float timer;

    private DecisionTrigger currentTrigger;

    private int currentDecision = 0;
    private const int totalDecisions = 3;
    private float decisionCooldown = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnCourse();
    }

    void Update()
    {
        // Handle decision cooldown
        if (decisionCooldown > 0f)
        {
            decisionCooldown -= Time.deltaTime;
            return;
        }

        if (!inDecision) return;

        timer -= Time.deltaTime;

        if (timerSlider != null)
            timerSlider.value = timer / decisionDuration;

        if (timer <= 0f)
        {
            EvaluateDecision();
        }
    }

    // 🔥 SPAWN BASED ON WARP
    void SpawnCourse()
    {
        switch (WarpData.currentWarp)
        {
            case WarpType.To_S_Orbital:
                currentCourse = Instantiate(sOrbitalCourse);
                currentCourse.SetActive(true);
                break;

            case WarpType.To_P_Orbital:
                currentCourse = Instantiate(pOrbitalCourse);
                currentCourse.SetActive(true);
                break;

            case WarpType.To_D_Orbital:
                currentCourse = Instantiate(dOrbitalCourse);
                currentCourse.SetActive(true);
                break;

            case WarpType.Special_Event:
                currentCourse = Instantiate(specialCourse);
                currentCourse.SetActive(true);
                break;
            
        }
    }

    // 🔥 CALLED BY TRIGGER (ONLY ONCE)
    public void ReachDecision(DecisionTrigger trigger)
    {
        if (inDecision) return;

        currentTrigger = trigger;

        inDecision = true;
        timer = decisionDuration;

        playerMovement.SetSpeed(slowSpeed);

        if (timerSlider != null)
            timerSlider.gameObject.SetActive(true);
    }

    // 🔥 EVALUATION (ONLY HERE)
    void EvaluateDecision()
    {
        inDecision = false;

        playerMovement.SetSpeed(normalSpeed);

        if (timerSlider != null)
            timerSlider.gameObject.SetActive(false);

        int playerLane = currentTrigger.GetPlayerLane();
        Debug.Log($"[DECISION {currentDecision + 1}] Player in lane: {playerLane}, Correct lane: {currentTrigger.correctLane}");

        if (playerLane == currentTrigger.correctLane)
        {
            Debug.Log("✅ CORRECT!");
        }
        else
        {
            Debug.Log("❌ WRONG!");
        }

        currentDecision++;
        decisionCooldown = 0.5f; // Small delay before next trigger can activate

        // Destroy the trigger after evaluation
        if (currentTrigger != null)
        {
            currentTrigger.DestroySelf();
        }

        // 🚀 FINISH
        if (currentDecision >= totalDecisions)
        {
            Debug.Log("🚀 ALL DECISIONS DONE - WARPING BACK!");
            WarpController.Instance.StartWarpBack();
        }
    }
}