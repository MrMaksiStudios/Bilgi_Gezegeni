using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public Transform player;
    public ZoneIndicatorManager ZoneIndicatorManager;
    public List<UIEditable> uiElements;
    public PauseManager pauseManager;
    public ProtonController playerController;

    /*void Awake()
    {
        //Debug.Log("GameLoader AWAKE");
    }*/
    void Start()
    {
        //Debug.Log("GameLoader START");

        StartCoroutine(LoadRoutine());
    }

    /*void OnEnable()
    {
        Debug.Log("GameLoader ENABLED");
    }

    void OnDisable()
    {
        Debug.Log("GameLoader DISABLED");
    }*/

    IEnumerator LoadRoutine()
    {
        yield return null;
        yield return null; 

        int rawValue = PlayerPrefs.GetInt("ShouldLoad", -1);
        bool shouldLoad = rawValue == 1;

        if (shouldLoad)
        {
            SaveManager.Instance.LoadGame(player, ZoneIndicatorManager.zones);

            yield return null;

            SaveManager.Instance.LoadUI(uiElements);
            
            // Wait one more frame for physics to settle before resetting controller
            yield return new WaitForFixedUpdate();
            
            // Reset controller state after loading
            if (playerController != null)
            {
                playerController.ResetControllerState();
            }
        }
    }
}