using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class HintTextOption
{
    [Tooltip("Hint text to display.")]
    public string text;

    [Tooltip("If enabled, this hint will auto-show when the checkpoint is active.")]
    public bool isEssential;
}

[System.Serializable]
public class CheckpointHintEntry
{
    [Tooltip("Checkpoint transform that matches the player's saved spawn point.")]
    public Transform checkpoint;

    [Tooltip("Possible hints to show when this checkpoint is active.")]
    public List<HintTextOption> hints = new List<HintTextOption>();
}

public class HelperTexts : MonoBehaviour
{
    private const string SpawnPointXKey = "LastSpawnPointX";
    private const string SpawnPointYKey = "LastSpawnPointY";
    private const string SpawnPointZKey = "LastSpawnPointZ";

    [Header("UI")]
    [Tooltip("Button that represents the companion on screen.")]
    public Button helperButton;

    [Tooltip("Text box that displays the hint.")]
    public TMP_Text hintText;

    [Tooltip("Image that appears behind the hint text.")]
    public Image hintBackground;

    [Tooltip("Canvas group used to fade the hint box in and out.")]
    public CanvasGroup hintCanvasGroup;

    [Tooltip("How long the hint stays visible before fading out.")]
    public float displayDuration = 2.5f;

    [Tooltip("How long the hint takes to fade out.")]
    public float fadeDuration = 0.75f;

    [Tooltip("Checkpoint-specific hint lists assigned in the inspector.")]
    public List<CheckpointHintEntry> checkpointHints = new List<CheckpointHintEntry>();

    [Tooltip("Position tolerance used when matching the saved spawn point to a checkpoint.")]
    public float checkpointMatchDistance = 0.5f;

    [Tooltip("How many characters per second the hint reveals.")]
    public float typewriterCharactersPerSecond = 24f;

    [Tooltip("Padding added around the hint text when sizing the textbox background.")]
    public Vector2 backgroundPadding = new Vector2(30f, 20f);

    private Coroutine fadeRoutine;
    private Coroutine typewriterRoutine;
    private string lastTrackedCheckpointIdentifier = string.Empty;

    private void Awake()
    {
        if (helperButton != null)
        {
            helperButton.onClick.AddListener(ShowHintForCurrentCheckpoint);
        }

        if (hintText != null)
        {
            hintText.text = string.Empty;
            hintText.maxVisibleCharacters = 0;
        }

        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 0f;
            hintCanvasGroup.interactable = false;
            hintCanvasGroup.blocksRaycasts = false;
        }

        if (hintBackground != null)
        {
            Color backgroundColor = hintBackground.color;
            backgroundColor.a = 0f;
            hintBackground.color = backgroundColor;
        }

        UpdateTrackedCheckpoint();
    }

    private void Update()
    {
        CheckForCheckpointChange();
    }

    private void OnDestroy()
    {
        if (helperButton != null)
        {
            helperButton.onClick.RemoveListener(ShowHintForCurrentCheckpoint);
        }
    }

    public void ShowHintForCurrentCheckpoint()
    {
        string hint = GetRandomHintForCurrentCheckpoint(false);

        if (string.IsNullOrWhiteSpace(hint))
        {
            Debug.LogWarning("No hint found for the current checkpoint.", this);
            return;
        }

        ShowHint(hint);
    }

    private void CheckForCheckpointChange()
    {
        string currentCheckpointIdentifier = GetCurrentCheckpointIdentifier();

        if (currentCheckpointIdentifier == lastTrackedCheckpointIdentifier)
            return;

        lastTrackedCheckpointIdentifier = currentCheckpointIdentifier;

        if (string.IsNullOrWhiteSpace(currentCheckpointIdentifier))
            return;

        ShowEssentialHintForCurrentCheckpoint();
    }

    private void UpdateTrackedCheckpoint()
    {
        lastTrackedCheckpointIdentifier = GetCurrentCheckpointIdentifier();
    }

    private void ShowEssentialHintForCurrentCheckpoint()
    {
        CheckpointHintEntry activeCheckpoint = GetCheckpointHintEntryForCurrentCheckpoint();

        if (activeCheckpoint == null)
            return;

        string hint = GetRandomHintFromCheckpoint(activeCheckpoint, true);

        if (string.IsNullOrWhiteSpace(hint))
            return;

        ShowHint(hint);
    }

    private void ShowHint(string hint)
    {
        if (hintText != null)
        {
            hintText.text = hint;
            hintText.alpha = 1f;
            hintText.maxVisibleCharacters = 0;
            hintText.ForceMeshUpdate();
        }

        if (hintBackground != null)
        {
            Color backgroundColor = hintBackground.color;
            backgroundColor.a = 1f;
            hintBackground.color = backgroundColor;
        }

        UpdateHintBackgroundSize();

        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 1f;
            hintCanvasGroup.interactable = true;
            hintCanvasGroup.blocksRaycasts = true;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }

        if (typewriterRoutine != null)
        {
            StopCoroutine(typewriterRoutine);
            typewriterRoutine = null;
        }

        typewriterRoutine = StartCoroutine(TypewriterReveal());
    }

    private IEnumerator TypewriterReveal()
    {
        if (hintText == null)
        {
            yield break;
        }

        hintText.maxVisibleCharacters = 0;
        hintText.ForceMeshUpdate();

        int totalCharacters = hintText.textInfo.characterCount;

        if (totalCharacters == 0)
        {
            fadeRoutine = StartCoroutine(FadeHintOut());
            yield break;
        }

        float characterInterval = 1f / Mathf.Max(typewriterCharactersPerSecond, 0.01f);

        while (hintText.maxVisibleCharacters < totalCharacters)
        {
            hintText.maxVisibleCharacters++;
            yield return new WaitForSeconds(characterInterval);
        }

        if (typewriterRoutine != null)
        {
            typewriterRoutine = null;
        }

        fadeRoutine = StartCoroutine(FadeHintOut());
    }

    private void UpdateHintBackgroundSize()
    {
        if (hintText == null || hintBackground == null)
            return;

        hintText.ForceMeshUpdate();
        Vector2 preferredSize = hintText.GetPreferredValues(hintText.text);

        RectTransform backgroundRect = hintBackground.rectTransform;
        if (backgroundRect == null)
            return;

        backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x + backgroundPadding.x);
        backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y + backgroundPadding.y);
    }

    private IEnumerator FadeHintOut()
    {
        yield return new WaitForSeconds(displayDuration);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            if (hintCanvasGroup != null)
            {
                hintCanvasGroup.alpha = alpha;
            }

            if (hintText != null)
            {
                hintText.alpha = alpha;
            }

            if (hintBackground != null)
            {
                Color backgroundColor = hintBackground.color;
                backgroundColor.a = alpha;
                hintBackground.color = backgroundColor;
            }

            yield return null;
        }

        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 0f;
            hintCanvasGroup.interactable = false;
            hintCanvasGroup.blocksRaycasts = false;
        }

        if (hintText != null)
        {
            hintText.alpha = 0f;
        }

        if (hintBackground != null)
        {
            Color backgroundColor = hintBackground.color;
            backgroundColor.a = 0f;
            hintBackground.color = backgroundColor;
        }

        fadeRoutine = null;
    }

    private string GetRandomHintForCurrentCheckpoint(bool essentialOnly)
    {
        CheckpointHintEntry activeCheckpoint = GetCheckpointHintEntryForCurrentCheckpoint();

        if (activeCheckpoint == null)
            return string.Empty;

        return GetRandomHintFromCheckpoint(activeCheckpoint, essentialOnly);
    }

    private CheckpointHintEntry GetCheckpointHintEntryForCurrentCheckpoint()
    {
        Vector3 savedCheckpointPosition = ReadSavedCheckpointPosition();

        for (int i = 0; i < checkpointHints.Count; i++)
        {
            CheckpointHintEntry checkpointHint = checkpointHints[i];

            if (checkpointHint == null || checkpointHint.checkpoint == null)
                continue;

            if (Vector3.Distance(savedCheckpointPosition, checkpointHint.checkpoint.position) <= checkpointMatchDistance)
            {
                return checkpointHint;
            }
        }

        return null;
    }

    private string GetRandomHintFromCheckpoint(CheckpointHintEntry checkpointHint, bool essentialOnly)
    {
        if (checkpointHint == null || checkpointHint.checkpoint == null)
            return string.Empty;

        if (checkpointHint.hints == null || checkpointHint.hints.Count == 0)
        {
            Debug.LogWarning($"Checkpoint '{checkpointHint.checkpoint.name}' has no hint entries.", this);
            return string.Empty;
        }

        List<HintTextOption> availableHints = new List<HintTextOption>();

        for (int j = 0; j < checkpointHint.hints.Count; j++)
        {
            HintTextOption hintOption = checkpointHint.hints[j];

            if (hintOption == null)
                continue;

            if (essentialOnly && !hintOption.isEssential)
                continue;

            availableHints.Add(hintOption);
        }

        if (availableHints.Count == 0)
        {
            if (essentialOnly)
                return string.Empty;

            Debug.LogWarning($"Checkpoint '{checkpointHint.checkpoint.name}' has no matching hints.", this);
            return string.Empty;
        }

        int randomIndex = Random.Range(0, availableHints.Count);
        return availableHints[randomIndex].text;
    }

    private Vector3 ReadSavedCheckpointPosition()
    {
        if (!PlayerPrefs.HasKey(SpawnPointXKey) || !PlayerPrefs.HasKey(SpawnPointYKey) || !PlayerPrefs.HasKey(SpawnPointZKey))
        {
            return Vector3.zero;
        }

        return new Vector3(
            PlayerPrefs.GetFloat(SpawnPointXKey),
            PlayerPrefs.GetFloat(SpawnPointYKey),
            PlayerPrefs.GetFloat(SpawnPointZKey)
        );
    }

    private string GetCurrentCheckpointIdentifier()
    {
        CheckpointHintEntry activeCheckpoint = GetCheckpointHintEntryForCurrentCheckpoint();

        if (activeCheckpoint == null || activeCheckpoint.checkpoint == null)
            return string.Empty;

        return GetCheckpointIdentifier(activeCheckpoint.checkpoint);
    }

    private string GetCheckpointIdentifier(Transform checkpoint)
    {
        if (checkpoint == null)
            return string.Empty;

        if (!string.IsNullOrWhiteSpace(checkpoint.name))
            return checkpoint.name;

        return $"{checkpoint.position.x:F2}_{checkpoint.position.y:F2}_{checkpoint.position.z:F2}";
    }
}
