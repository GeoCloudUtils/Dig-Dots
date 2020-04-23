using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameDoneController : MonoBehaviour
{
    public ScreenshotHandler scHandler;
    public GameObject[] CanvasContents;
    public Image screenShootImage;
    public Button nextLevelButton;
    public Button watchAdButton;
    public event UnityAction NextLevelEvent;
    public TextMeshProUGUI textLevel;
    public Sprite[] Icons; // icons array

    private int levelIndex = 0;
    private void Start()
    {
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        watchAdButton.onClick.AddListener(WatchAd);
    }

    private void WatchAd()
    {
        watchAdButton.interactable = false;
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        nextLevelButton.interactable = false;
        if (NextLevelEvent != null)
            NextLevelEvent.Invoke();
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
