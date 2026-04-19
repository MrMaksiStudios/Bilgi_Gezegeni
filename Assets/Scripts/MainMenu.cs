using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;

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
        SceneManager.LoadScene("LoadingScene");
    }
}