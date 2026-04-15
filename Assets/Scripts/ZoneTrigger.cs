using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private ZoneIndicatorManager manager;
    private string text;

    public void Init(ZoneIndicatorManager m, string t)
    {
        manager = m;
        text = t;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ShowZone(text);
        }
    }
}