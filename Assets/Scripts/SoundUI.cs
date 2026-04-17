using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        masterSlider.value = AudioManager3D.Instance.masterVolume;
        musicSlider.value = AudioManager3D.Instance.musicVolume;
        sfxSlider.value = AudioManager3D.Instance.sfxVolume;
    }
}