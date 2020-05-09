﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGameManager : MonoBehaviour
{
    private AudioSource _music;
    private AudioSource _sounds;
    private bool _musicChanged = false;
    private bool _musicIsPlaying = false;

    //Getters & Setters
    public float SoundsVolume
    {
        get => AudioListener.volume;
    }
    public float MusicVolume
    {
        get => _music.volume;
    }
    private void Awake()
    {
        _music = gameObject.AddComponent<AudioSource>();
        _music.ignoreListenerVolume = true;
        _music.loop = true;
        _sounds = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }


    public void SetEffectsVolume(float newValue)
    {
        AudioListener.volume = newValue;
    }
    public void SetMusicVolume(float newValue)
    {
        _music.volume = newValue;
    }
    public void SetMusic(AudioClip sound)
    {
        _music.clip = sound;
        _musicChanged = true;
    }
    public void PlayMusic(bool play)
    {
        if (!_musicChanged && _musicIsPlaying) return;
        _musicIsPlaying = true;
        _musicChanged = false;

        if (play)
            _music.Play();
        else
            _music.Stop();
    }

    public void PlaySound(AudioClip sound)
    {
        bool isSoundEnabled = true;
        if (PlayerPrefs.HasKey("SoundState"))
            isSoundEnabled = PlayerPrefs.GetInt("SoundState") == 1;
        if (isSoundEnabled)
            _sounds.PlayOneShot(sound);
    }

    public bool IsMusicPlaying()
    {
        return _musicIsPlaying;
    }
}
