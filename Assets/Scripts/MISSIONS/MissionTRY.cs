using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTRY : MonoBehaviour

{
    [SerializeField] private Transform destinationTransform;
    public ZoneIndicatorManager ZoneIndicatorManager;
    public Transform player;
    public List<UIEditable> uiElements;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameEvents.Trigger("başla1");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameEvents.Trigger("bitir1");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameEvents.Trigger("başla2");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameEvents.Trigger("bitir2");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameEvents.Trigger("başla3");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameEvents.Trigger("bitir3");  
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameEvents.Trigger("başla4");
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            GameEvents.Trigger("bitir4");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //SaveManager.Instance.SaveGame(player,zoneManager.zones);
            player.position = destinationTransform.position;
            //TravelData.targetPosition = destinationTransform.position;
            //TravelData.targetScene = "OrbitalRPG";
            SaveManager.Instance.SaveGame(player, ZoneIndicatorManager.zones);
            WarpData.currentWarp = WarpType.To_P_Orbital;
            WarpController.Instance.StartWarp("InterOrbitals");
            //player.position = TravelData.targetPosition;
        }
    }
}
