using UnityEngine;
using UnityEngine.UI;

public class UIEditSlider : MonoBehaviour
{
    public static UIEditSlider Instance;

    public Slider slider;
    public GameObject panel;

    UIEditable currentTarget;

    public enum EditType { Transparency, Size }
    EditType currentType;

    void Awake()
    {
        Instance = this;
    }

    public void Open(UIEditable target, EditType type)
    {
        currentTarget = target;
        currentType = type;

        panel.SetActive(true);

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        if (currentTarget == null) return;

        if (currentType == EditType.Transparency)
            currentTarget.SetTransparency(value);

        else if (currentType == EditType.Size)
            currentTarget.SetSize(value);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}