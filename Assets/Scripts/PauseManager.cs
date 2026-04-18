using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject soundPanel;
    public Transform player;
    public List<UIEditable> uiElements;
    public Button settings;

    public static bool IsPaused = false;
    public ZoneIndicatorManager zoneManager;



    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        /*Optional: ESC key for testing (PC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }*/
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
        IsPaused = true;
        settings.interactable = false;

        /* Show cursor (important for PC)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;*/
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        IsPaused = false;
        settings.interactable = true;

        /* Hide cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    public void OpenSoundMenu()
    {
        soundPanel.SetActive(true);
    }

    public void CloseSoundMenu()
    {
        soundPanel.SetActive(false);
    }
    public void SaveAndExit()
    {
        SaveManager.Instance.SaveGame(player,zoneManager.zones);

        Time.timeScale = 1f;

        SaveManager.Instance.SaveUI(uiElements);

        SceneManager.LoadScene("RPGOpening"); 
    }
}