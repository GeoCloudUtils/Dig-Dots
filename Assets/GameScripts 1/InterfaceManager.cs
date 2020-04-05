using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ScriptUtils.Interface;
using ScriptUtils.GameUtils;
using UnityEngine.Events;

public class InterfaceManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Settings settingCanvas;
    public Button settingButton;
    public ScrolSnapContent snapContent;
    public LevelCounter level;
    private void Start()
    {
        settingButton.onClick.AddListener(OpenSettings);
        settingCanvas.contentAction += SettingCanvas_contentAction;
        snapContent.ShowLevelsContent += SnapContent_ShowLevelsContent;
        level.backToSectionsEvent += Level_backToSectionsEvent;
        level.DispatchLevelOpen += Level_DispatchLevelOpen;
    }

    private void Level_DispatchLevelOpen()
    {
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("MainGame");
    }

    private void Level_backToSectionsEvent()
    {
        snapContent.gameObject.SetActive(true);
        level.gameObject.SetActive(false);
    }

    private void SnapContent_ShowLevelsContent(int index)
    {
        snapContent.gameObject.SetActive(false);
        level.gameObject.SetActive(true);
        level.counter = index;
    }

    private void SettingCanvas_contentAction()
    {
        settingCanvas.gameObject.SetActive(false);
    }

    private void OpenSettings()
    {
        settingCanvas.gameObject.SetActive(true);
    }
}
