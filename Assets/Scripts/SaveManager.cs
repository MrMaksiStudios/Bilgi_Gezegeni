using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public bool HasSave()
    {
        return PlayerPrefs.HasKey("PlayerX");
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SaveGame(Transform player,List<ZoneInfo> zones)
    {
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        for (int i = 0; i < zones.Count; i++)
        {
            PlayerPrefs.SetInt("Zone_" + i, zones[i].discovered ? 1 : 0);
        }

        PlayerPrefs.SetFloat("MasterVolume", AudioManager3D.Instance.masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", AudioManager3D.Instance.musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager3D.Instance.sfxVolume);

        PlayerPrefs.Save();

        //Debug.Log("Game Saved");
    }

    public void LoadGame(Transform player, List<ZoneInfo> zones)
    {
        if (!PlayerPrefs.HasKey("PlayerX")) return;

        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat("PlayerX"),
            PlayerPrefs.GetFloat("PlayerY"),
            PlayerPrefs.GetFloat("PlayerZ")
        );

        player.position = pos;

        AudioManager3D.Instance.masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioManager3D.Instance.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        AudioManager3D.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        for (int i = 0; i < zones.Count; i++)
        {
            int value = PlayerPrefs.GetInt("Zone_" + i, 0);
            zones[i].discovered = (value == 1);

            if (zones[i].mapIcon != null)
                zones[i].mapIcon.SetActive(zones[i].discovered);
        }

        //Debug.Log("Game Loaded");
    }

    public void SaveUI(List<UIEditable> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var rect = elements[i].GetComponent<RectTransform>();
            var canvas = elements[i].GetComponent<CanvasGroup>();

            PlayerPrefs.SetFloat("UI_PosX_" + i, rect.anchoredPosition.x);
            PlayerPrefs.SetFloat("UI_PosY_" + i, rect.anchoredPosition.y);
            PlayerPrefs.SetFloat("UI_Scale_" + i, rect.localScale.x);
            PlayerPrefs.SetFloat("UI_Alpha_" + i, canvas.alpha);
        }
    }

    public void LoadUI(List<UIEditable> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var rect = elements[i].GetComponent<RectTransform>();
            var canvas = elements[i].GetComponent<CanvasGroup>();

            rect.anchoredPosition = new Vector2(
                PlayerPrefs.GetFloat("UI_PosX_" + i, rect.anchoredPosition.x),
                PlayerPrefs.GetFloat("UI_PosY_" + i, rect.anchoredPosition.y)
            );

            float scale = PlayerPrefs.GetFloat("UI_Scale_" + i, 1f);
            rect.localScale = Vector3.one * scale;

            canvas.alpha = PlayerPrefs.GetFloat("UI_Alpha_" + i, 1f);
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        //Debug.Log("Save Deleted");
    }
}