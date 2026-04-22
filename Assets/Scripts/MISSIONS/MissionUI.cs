using UnityEngine;
using TMPro;
using System.Collections;

public class MissionUI : MonoBehaviour
{
    public static MissionUI Instance;

    public GameObject missionPopup;
    public TextMeshProUGUI missionText;
    public CanvasGroup canvasGroup;
    public RectTransform rect;

    void Awake()
    {
        Instance = this;
    }

    public void ShowMissionStart(MissionData mission)
    {
        StartCoroutine(PlayAnimation("Yeni Görev: " + mission.missionTitle));
    }

    public void ShowMissionComplete(MissionData mission)
    {
        StartCoroutine(PlayAnimation("Görev Tamamlandı: " + mission.missionTitle));
    }

    IEnumerator PlayAnimation(string text)
    {
        missionPopup.SetActive(true);
        missionText.text = text;

        float duration = 0.5f;
        float time = 0;

        rect.localScale = new Vector3(0, 1, 1);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            canvasGroup.alpha = t;
            rect.localScale = new Vector3(t, 1, 1);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            canvasGroup.alpha = 1 - t;
            yield return null;
        }

        missionPopup.SetActive(false);
    }
}