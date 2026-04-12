using UnityEngine;
using TMPro;

public class BondGameController : MonoBehaviour
{
    public static BondGameController Instance;

    [Header("Elements")]
    public ElementData leftElement;
    public ElementData rightElement;

    [Header("UI")]
    public TMP_Text feedbackText;
    public TMP_Text electronegativityText;

    [Header("Game")]
    public int lives = 3;

    private BondType currentBondType;
    private SlotType correctSlot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupBond();
    }

    void SetupBond()
    {
        DetermineBondType();
        DetermineCorrectSlot();

        electronegativityText.text = "";
        feedbackText.text = "Elektronu doğru yere yerleştir!";
    }

    void DetermineBondType()
    {
        if (!leftElement.isMetal && !rightElement.isMetal)
        {
            if (leftElement.elementName == rightElement.elementName)
                currentBondType = BondType.ApolarCovalent;
            else
                currentBondType = BondType.PolarCovalent;
        }
        else
        {
            currentBondType = BondType.Ionic;
        }
    }

    void DetermineCorrectSlot()
    {
        switch (currentBondType)
        {
            case BondType.ApolarCovalent:
                correctSlot = SlotType.Center;
                break;

            case BondType.PolarCovalent:
                correctSlot =
                    leftElement.electronegativity > rightElement.electronegativity
                    ? SlotType.LeftNear
                    : SlotType.RightNear;
                break;

            case BondType.Ionic:
                correctSlot =
                    leftElement.isMetal ? SlotType.RightAtom : SlotType.LeftAtom;
                break;
        }
    }

    public void TryPlaceElectron(ElectronDrag electron, ElectronSlot slot)
    {
        if (slot.slotType == correctSlot)
        {
            CorrectPlacement(electron, slot);
        }
        else
        {
            WrongPlacement();
        }
    }

    void CorrectPlacement(ElectronDrag electron, ElectronSlot slot)
    {
        electron.Place(slot.transform.position);

        feedbackText.text = "✔ Doğru!";
        electronegativityText.text =
            $"{leftElement.elementName}: {leftElement.electronegativity}\n" +
            $"{rightElement.elementName}: {rightElement.electronegativity}";

        Invoke(nameof(NextQuestion), 1.5f);
    }

    void WrongPlacement()
    {
        lives--;
        feedbackText.text = "✘ Yanlış!";

        if (lives <= 0)
        {
            feedbackText.text = "Oyun Bitti!";
        }
    }

    void NextQuestion()
    {
        // burada:
        // - yeni elementler seç
        // - elektronu resetle
        // - SetupBond() çağır
        SetupBond();
    }
}
