using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneInfo
{
    public Collider triggerZone;
    public string zoneText;

    public Transform teleportPoint;   // where player goes
    public GameObject mapIcon;        // UI icon on map

    [HideInInspector] public bool discovered = false;
}