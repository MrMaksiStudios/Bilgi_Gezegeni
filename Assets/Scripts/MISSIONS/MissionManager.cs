using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("All Missions")]
    public List<MissionData> allMissions;

    private Dictionary<string, MissionData> missionLookup = new();
    public List<MissionData> activeMissions = new();

    private HashSet<string> completedMissions = new();

    public System.Action onMissionChanged;

    void Awake()
    {
        Instance = this;

        foreach (var mission in allMissions)
        {
            missionLookup[mission.missionID] = mission;

            GameEvents.Subscribe(mission.startEvent, () => StartMission(mission.missionID));
            GameEvents.Subscribe(mission.endEvent, () => CompleteMission(mission.missionID));
        }
    }

    void StartMission(string id)
    {
        // ❗ Prevent restarting completed missions
        if (completedMissions.Contains(id)) return;

        if (!missionLookup.ContainsKey(id)) return;

        var mission = missionLookup[id];

        if (activeMissions.Contains(mission)) return;

        activeMissions.Add(mission);

        MissionUI.Instance.ShowMissionStart(mission);
        onMissionChanged?.Invoke();
    }

    void CompleteMission(string id)
    {
        if (!missionLookup.ContainsKey(id)) return;

        var mission = missionLookup[id];

        if (!activeMissions.Contains(mission)) return;

        activeMissions.Remove(mission);

        // ✅ Mark as completed
        completedMissions.Add(id);

        MissionUI.Instance.ShowMissionComplete(mission);
        onMissionChanged?.Invoke();
    }

    public List<MissionData> GetActiveMissions()
    {
        return activeMissions;
    }

    public List<string> GetActiveMissionIDs()
    {
        List<string> ids = new();

        foreach (var m in activeMissions)
            ids.Add(m.missionID);

        return ids;
    }

    public List<string> GetCompletedMissionIDs()
    {
        return new List<string>(completedMissions);
    }

    public void LoadMissions(List<string> activeIDs, List<string> completedIDs)
    {
        completedMissions = new HashSet<string>(completedIDs);

        activeMissions.Clear();

        foreach (var id in activeIDs)
        {
            if (missionLookup.ContainsKey(id))
            {
                activeMissions.Add(missionLookup[id]);
            }
        }

        onMissionChanged?.Invoke();
    }
}