using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SountInit : MonoBehaviour
{
    public AudioClip bgMusic;
    private SoundGameManager sManager;
    public bool started = false;
    void Start()
    {
        if (sManager == null)
            sManager = FindObjectOfType<SoundGameManager>();
    }

    public void Init()
    {
        started = true;
        sManager.SetMusic(bgMusic);
        float musicVolume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 0.5f;
        sManager.SetMusicVolume(musicVolume);
        sManager.PlayMusic(bgMusic);
    }
}
