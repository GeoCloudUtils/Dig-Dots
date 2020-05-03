﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameDoneController : MonoBehaviour
{
    public AdCaller adCaller;
    public ScreenshotHandler scHandler;
    public GameObject[] CanvasContents;
    public GameObject alertScreen;
    public Image screenShootImage;
    public Button nextLevelButton;
    public Button nextLevelButton2;
    public Button watchAdButton;
    public event UnityAction<bool> NextLevelEvent;
    public TextMeshProUGUI textLevel;
    public Sprite[] Icons; // icons array

    private int levelIndex = 0;
    private bool disableAdOnLoad = false;
    private bool chapterCountReached = false;


    private int nextStept = 0;
    private void Start()
    {
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        nextLevelButton2.onClick.AddListener(LoadNextLevel);
        watchAdButton.onClick.AddListener(WatchAd);
        disableAdOnLoad = false;
        nextStept = PlayerPrefs.GetInt("CHAPTER_MAX");
        Debug.Log(nextStept);
    }

    private void WatchAd()
    {
        watchAdButton.gameObject.SetActive(false);
        nextLevelButton2.gameObject.SetActive(true);
        watchAdButton.interactable = false;
        disableAdOnLoad = true;
        adCaller.InitializeAd();
        adCaller.ShowAd(true);
        //LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if ((levelIndex + 1) >= nextStept)
        {
            if (!canGoNext())
            {
                Debug.Log("Need to complete all chapter levels to continue");
                alertScreen.gameObject.SetActive(true);
                return;
            }
        }
        if (NextLevelEvent != null)
            NextLevelEvent.Invoke(disableAdOnLoad);
    }

    private bool canGoNext()
    {
        for (int i = 1; i <= nextStept; i++)
        {
            if (!PlayerPrefs.HasKey("level" + i))
                return false;
        }
        return true;
    }
    public void SetContent(bool good)
    {
        LoadIcons();
        CanvasContents[good ? 0 : 1].SetActive(true);
        levelIndex = PlayerPrefs.GetInt("levelIndex");
        textLevel.text = "Level" + " " + (levelIndex + 1) + "\n" + "COMPLETE";
        if (good)
            PlayerPrefs.SetInt("level" + (levelIndex + 1).ToString(), 1);
        else
            PlayerPrefs.SetInt("level" + (levelIndex + 1).ToString(), 0);
        screenShootImage.sprite = GetSprite();
        nextLevelButton.gameObject.SetActive(good);
        watchAdButton.gameObject.SetActive(!good);
    }
    void LoadIcons()
    {
        object[] loadedIcons = Resources.LoadAll("LevelsScreenshots/section1", typeof(Sprite));
        Icons = new Sprite[loadedIcons.Length];
        for (int x = 0; x < loadedIcons.Length; x++)
            Icons[x] = (Sprite)loadedIcons[x];
    }

    private Sprite GetSprite()
    {
        string name = "level" + (levelIndex + 1).ToString();
        for (int i = 0; i < Icons.Length; i++)
        {
            if (Icons[i].name == name)
                return Icons[i];
        }
        return null;
    }
}
