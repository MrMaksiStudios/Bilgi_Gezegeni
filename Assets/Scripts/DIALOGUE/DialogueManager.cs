using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private List<DialogueLine> currentDialogue;
    private int currentIndex;
    private bool isDialogueActive;

    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public GameObject canvas;
    public GameObject force;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // 🔥 Finish instantly
                FinishTyping();
            }
            else
            {
                // 🔥 Go to next line
                ShowNextLine();
            }
        }
    }

    public void StartDialogue(List<DialogueLine> dialogue, HasDialogue npc)
    {
        canvas.SetActive(false);
        force.SetActive(false);
        currentDialogue = dialogue;
        currentIndex = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentIndex >= currentDialogue.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue[currentIndex];

        // Stop any previous typing
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));

        if (line.givesItem && line.itemToGive != null)
        {
            InventoryManager.Instance.AddItem(line.itemToGive);
        }

        currentIndex++;
    }

    IEnumerator TypeLine(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";

        // 🔥 Trigger mission/event at start of line
        if (line.triggerEvent && !string.IsNullOrEmpty(line.eventName))
        {
            GameEvents.Trigger(line.eventName);
        }

        foreach (char letter in line.text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void FinishTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Show full line instantly
        dialogueText.text = currentDialogue[currentIndex - 1].text;

        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        canvas.SetActive(true);
        force.SetActive(true);
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}