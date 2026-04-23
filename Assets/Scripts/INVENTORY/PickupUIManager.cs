using UnityEngine;
using TMPro;
using System.Collections;

public class PickupUIManager : MonoBehaviour
{
    public static PickupUIManager Instance;

    public TextMeshProUGUI pickupText;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        pickupText.gameObject.SetActive(false);
    }

    public void ShowPickupText(string itemName)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(itemName));
    }

    IEnumerator ShowRoutine(string itemName)
    {
        pickupText.text = itemName + " envantere eklendi";
        pickupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        pickupText.gameObject.SetActive(false);
    }
}