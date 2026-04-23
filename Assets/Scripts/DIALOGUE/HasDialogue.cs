using System.Collections.Generic;
using UnityEngine;

public class HasDialogue : MonoBehaviour
{
    [Header("Mission Link")]
    public string missionID;

    [Header("Dialogues")]
    public List<DialogueLine> beforeMission;
    public List<DialogueLine> duringMission;
    public List<DialogueLine> afterMission;

    private bool playerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractionUI.Instance.ShowButton(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionUI.Instance.HideButton();
        }
    }

    public void Interact()
    {
        if (!playerInRange) return;
        if (DialogueManager.Instance.IsDialogueActive()) return;

        DialogueManager.Instance.StartDialogue(GetDialogue(), this);
    }

    List<DialogueLine> GetDialogue()
    {
        var active = MissionManager.Instance.GetActiveMissionIDs();
        var completed = MissionManager.Instance.GetCompletedMissionIDs();

        if (completed.Contains(missionID))
            return afterMission;

        if (active.Contains(missionID))
            return duringMission;

        return beforeMission;
    }
}