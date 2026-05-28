using UnityEngine;

public class CovalentBondZone : MonoBehaviour
{
    public int totalRequired = 4;
    int current = 0;

    public void OnElectronPlaced()
    {
        current++;

        if (current >= totalRequired)
        {
            ProgressManager.Instance.covalentDone = true;
            Debug.Log("✅ Covalent bond completed");
        }
    }
}
