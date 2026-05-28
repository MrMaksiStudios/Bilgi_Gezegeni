using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public GameObject Main4;
    public GameObject YapimcilarPanel;

    void Start()
    {
        continueButton.interactable = SaveManager.Instance.HasSave();
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("ShouldLoad", 0);
        PlayerPrefs.Save();

        LoadingSceneLoader.sceneToLoad = "Orbital Intro";
        SceneManager.LoadScene("LoadingScene");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("ShouldLoad", 1);
        PlayerPrefs.Save();

        LoadingSceneLoader.sceneToLoad = "Orbital Intro";
        SceneManager.LoadScene("OrbitalRPG");
    }

    public void Yapimcilar()
    {
        Main4.SetActive(false);
        YapimcilarPanel.SetActive(true);
    }

    public void ExitYapimcilar()
    {
        Main4.SetActive(true);
        YapimcilarPanel.SetActive(false);

    }
}