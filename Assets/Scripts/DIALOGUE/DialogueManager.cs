using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private List<DialogueLine> currentDialogue;
    private int currentIndex;
    private bool isDialogueActive;

    private HasDialogue currentNPC;
    public GameObject canvas;
    public GameObject force;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isDialogueActive) return;

        // Tap / click to continue
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(List<DialogueLine> dialogue, HasDialogue npc)
    {
        currentDialogue = dialogue;
        currentIndex = 0;
        currentNPC = npc;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        ShowNextLine();
        canvas.SetActive(false); // Hide other UI during dialogue
        force.SetActive(false); // Hide force UI during dialogue
    }

    void ShowNextLine()
    {
        if (currentIndex >= currentDialogue.Count)
        {
            EndDialogue();
            return;
        }

        var line = currentDialogue[currentIndex];
        dialogueText.text = line.text;

        // 🔥 EVENT-BASED MISSION TRIGGER
        if (line.triggerEvent && !string.IsNullOrEmpty(line.eventName))
        {
            GameEvents.Trigger(line.eventName);
        }

        currentIndex++;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        currentNPC = null;
        canvas.SetActive(true); // Show other UI after dialogue
        force.SetActive(true); // Show force UI after dialogue
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}