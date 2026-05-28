using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonmanager : MonoBehaviour
{
    public GameObject orb;
    public GameObject bond;
    public GameObject borb;
    public GameObject bbond;
    public GameObject orbt;
    public GameObject bondt;
    public GameObject geri;
    public GameObject kamera;


    public void Orbg()
    {
        orb.SetActive(true);
        borb.SetActive(false);
        bbond.SetActive(false);
        orbt.SetActive(true);
        geri.SetActive(true);
        kamera.SetActive(false);
    }
    public void Bondg()
    {
        bond.SetActive(true);
        borb.SetActive(false);
        bbond.SetActive(false);
        bondt.SetActive(true);
        geri.SetActive(true);
        kamera.SetActive(false);
    }
    public void Nong()
    {
        orb.SetActive(false);
        bond.SetActive(false);
        borb.SetActive(true);
        bbond.SetActive(true);
        orbt.SetActive(false);
        bondt.SetActive(false);
        geri.SetActive(false);
        kamera.SetActive(true);
    }
}
