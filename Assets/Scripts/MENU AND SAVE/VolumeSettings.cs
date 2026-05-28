using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
    

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider SFXSlider;
    public GameObject settings;
    public GameObject creatorTab;
    public GameObject settingsTab;
    public void Start()
    {
        setMusicVolume();
        setSFXVolume();
    }
    public void OpenSettings()
    {
        settings.SetActive(false);
        settingsTab.SetActive(true);
    }

    public void OpenCreator()
    {
        settingsTab.SetActive(false);
        creatorTab.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsTab.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseCreator()
    {
        creatorTab.SetActive(false);
        settingsTab.SetActive(true);
    }

    public void setMusicVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
