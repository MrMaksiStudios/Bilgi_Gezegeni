using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public Transform player;
    public ZoneIndicatorManager ZoneIndicatorManager;
    public List<UIEditable> uiElements;
    public PauseManager pauseManager;

    IEnumerator Start()
    {
        yield return null;
        SaveManager.Instance.LoadGame(player, ZoneIndicatorManager.zones);
        SaveManager.Instance.LoadUI(uiElements);
        pauseManager.ResumeGame();
    }
}