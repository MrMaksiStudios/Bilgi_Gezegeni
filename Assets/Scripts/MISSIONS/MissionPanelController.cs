using UnityEngine;

public class MissionPanelController : MonoBehaviour
{
    public RectTransform panel;
    public float speed = 10f;

    private bool isOpen = false;

    private Vector2 closedPos;
    private Vector2 openPos;

    void Start()
    {
        float width = panel.rect.width;

        // Move fully off-screen
        closedPos = new Vector2(width, 0);

        // Fully visible
        openPos = Vector2.zero;

        panel.anchoredPosition = closedPos;
    }

    void Update()
    {
        Vector2 target = isOpen ? openPos : closedPos;
    
        panel.anchoredPosition = Vector2.MoveTowards(
            panel.anchoredPosition,
            target,
            speed * 1000 * Time.deltaTime
        );
        
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
    }
}