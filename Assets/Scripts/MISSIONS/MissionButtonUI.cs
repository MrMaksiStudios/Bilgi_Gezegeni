using UnityEngine;
using TMPro;

public class MissionButtonUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    private MissionData mission;
    private MissionListUI listUI;

    public void Setup(MissionData data, MissionListUI ui)
    {
        mission = data;
        listUI = ui;
        titleText.text = data.missionTitle;
    }

    public void OnClick()
    {
        listUI.ShowDetails(mission);
    }
}