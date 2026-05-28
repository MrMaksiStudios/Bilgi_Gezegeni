using UnityEngine;

public class UIEditManager : MonoBehaviour
{
    public static UIEditManager Instance;

    public enum EditMode { None, Position, Transparency, Size }
    public EditMode currentMode = EditMode.None;

    public GameObject editPanel; // top UI (Position, Size, etc.)
    public GameObject sliderPanel;
    public GameObject PauseMenu;

    public UIEditable[] allUIElements;
    void Awake()
    {
        Instance = this;
    }

    public void OpenEditMode()
    {
        editPanel.SetActive(true);
        currentMode = EditMode.Position; // default
        PauseMenu.SetActive(false); // hide pause menu if open

    }

    public void CloseEditMode()
    {
        editPanel.SetActive(false);
        sliderPanel.SetActive(false);
        currentMode = EditMode.None;
        PauseMenu.SetActive(true); // show pause menu again
    }

    public void SetPositionMode()
    {
        currentMode = EditMode.Position;
    }

    public void SetTransparencyMode()
    {
        currentMode = EditMode.Transparency;
    }

    public void SetSizeMode()
    {
        currentMode = EditMode.Size;
    }

    public void ResetAllUI()
    {
        foreach (var ui in allUIElements)
        {
            ui.ResetToDefault();
        }

        // wipe saved layout
        for (int i = 0; i < allUIElements.Length; i++)
        {
            string id = allUIElements[i].id;

            PlayerPrefs.DeleteKey(id + "_PosX");
            PlayerPrefs.DeleteKey(id + "_PosY");
            PlayerPrefs.DeleteKey(id + "_Scale");
            PlayerPrefs.DeleteKey(id + "_Alpha");
        }

        PlayerPrefs.Save();

        Debug.Log("UI Reset + Save Cleared");
    }
}