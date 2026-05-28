using UnityEngine;
using UnityEngine.UI;

public class LabButtonController : MonoBehaviour
{
    public Button labButton;

    void Start()
    {
        labButton.interactable = ProgressManager.Instance.CanEnterLab();
    }
}
