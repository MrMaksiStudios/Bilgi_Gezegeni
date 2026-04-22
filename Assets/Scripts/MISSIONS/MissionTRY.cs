using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTRY : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameEvents.Trigger("başla1");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameEvents.Trigger("bitir1");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameEvents.Trigger("başla2");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameEvents.Trigger("bitir2");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameEvents.Trigger("başla3");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameEvents.Trigger("bitir3");  
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameEvents.Trigger("başla4");
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            GameEvents.Trigger("bitir4");
        }
    }
}
