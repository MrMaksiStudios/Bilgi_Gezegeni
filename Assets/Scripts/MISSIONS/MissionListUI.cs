using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class MissionListUI : MonoBehaviour
{
    //public Transform contentParent;
    public GameObject missionButtonPrefab;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public GameObject detailsPanel;
    public Transform mainContentParent;
    public Transform sideContentParent;

    private List<GameObject> spawnedButtons = new();

    void Start()
    {
        detailsPanel.SetActive(false);

        MissionManager.Instance.onMissionChanged += RefreshList;
    }

    void OnDestroy()
    {
        MissionManager.Instance.onMissionChanged -= RefreshList;
    }

    void OnEnable()
    {
        StartCoroutine(DelayedRefresh());
    }

    IEnumerator DelayedRefresh()
    {
        yield return null; // wait 1 frame
        RefreshList();
    }

    public void RefreshList()
    {
        foreach (var obj in spawnedButtons)
            Destroy(obj);

        spawnedButtons.Clear();

        var missions = MissionManager.Instance.GetActiveMissions();

        foreach (var mission in missions)
        {
            Transform parent = mission.missionType == MissionData.MissionType.Main
                ? mainContentParent
                : sideContentParent;

            GameObject btnObj = Instantiate(missionButtonPrefab, parent);

            var btn = btnObj.GetComponent<MissionButtonUI>();
            btn.Setup(mission, this);

            spawnedButtons.Add(btnObj);
        }
    }

    public void ShowDetails(MissionData mission)
    {
        detailsPanel.SetActive(true);

        titleText.text = mission.missionTitle;
        descriptionText.text = mission.missionDescription;
    }
}