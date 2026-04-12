using UnityEngine;

public class AtomPanelController : MonoBehaviour
{
    public GameObject atomPanel;

    public void TogglePanel()
    {
        atomPanel.SetActive(!atomPanel.activeSelf);
    }
}
