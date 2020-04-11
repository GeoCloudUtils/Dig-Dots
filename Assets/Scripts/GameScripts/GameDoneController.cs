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
    public GameObject[] CanvasContents;
    public Image screenShootImage;
    public Button nextLevelButton;
    public event UnityAction NextLevelEvent;
    public TextMeshProUGUI textLevel;
    private void Start()
    {
        nextLevelButton.onClick.AddListener(LoadNextLevel);
    }
    private void LoadNextLevel()
    {
        nextLevelButton.interactable = false;
        if (NextLevelEvent != null)
            NextLevelEvent.Invoke();
    }
    public void SetContent(bool good)
    {
        CanvasContents[good ? 0 : 1].SetActive(true);
        int levelIndex = PlayerPrefs.GetInt("levelIndex");
        textLevel.text = "Level" + " " + (levelIndex + 1) + "\n" + "COMPLETE";
        if (good)
            PlayerPrefs.SetInt("level" + (levelIndex + 1).ToString(), 1);
        else
            PlayerPrefs.SetInt("level" + (levelIndex + 1).ToString(), 0);
    }
}
