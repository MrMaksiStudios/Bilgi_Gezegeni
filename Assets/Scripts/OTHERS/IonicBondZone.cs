using UnityEngine;

public class IonicBondZone : MonoBehaviour
{
    public void OnElectronPlaced()
    {
        ProgressManager.Instance.ionicDone = true;
        Debug.Log("İyonik bağ tamamlandı!");
    }
}
