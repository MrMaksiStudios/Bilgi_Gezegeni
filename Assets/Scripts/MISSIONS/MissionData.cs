using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission System/Mission")]
public class MissionData : ScriptableObject
{
    public string missionID;

    [Header("UI")]
    public string missionTitle;
    [TextArea] public string missionDescription;

    [Header("Events")]
    public string startEvent;
    public string endEvent;
    public MissionType missionType;

    public enum MissionType
    {
        Main,
        Side
    }
}