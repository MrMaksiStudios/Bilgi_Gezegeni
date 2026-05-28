using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private ZoneIndicatorManager manager;
    private string text;
    private int zoneIndex; // ✅ THIS WAS MISSING

    public void Init(ZoneIndicatorManager m, string t, int index)
    {
        manager = m;
        text = t;
        zoneIndex = index; // ✅ store it
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.DiscoverZone(zoneIndex); // ✅ use stored value
        }
    }
}