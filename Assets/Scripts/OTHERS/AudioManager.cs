using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource Sfx;

    public AudioClip backgroundMusicClip;
    public AudioClip clickSfxClip;

    void Start()
    {
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.Play();
    }
    public void playSFX()
    {
        Sfx.PlayOneShot(clickSfxClip);
    }
}
