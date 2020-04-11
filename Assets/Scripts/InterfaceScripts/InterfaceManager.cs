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
    public UtilitiesController utilitiesCanvas;
    public Button[] InterfaceButtons;
    public ScrolSnapContent snapContent;
    public LevelsContent level;
    private void Start()
    {
        foreach (Button btn in InterfaceButtons)
            btn.onClick.AddListener(() =>
            {
                OpenUtilitiesCanvas(btn);
            });
        utilitiesCanvas.contentAction += SettingCanvas_contentAction;
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
        level.counter = index;
        snapContent.gameObject.SetActive(false);
        level.gameObject.SetActive(true);
    }

    private void SettingCanvas_contentAction()
    {
        utilitiesCanvas.gameObject.SetActive(false);
        for (int i = 0; i < utilitiesCanvas.settingContent.transform.childCount - 1; i++)
            utilitiesCanvas.settingContent.transform.GetChild(i).gameObject.SetActive(false);
    }

    private void OpenUtilitiesCanvas(Button button)
    {
        int targetIndex = System.Array.IndexOf(InterfaceButtons, button);
        utilitiesCanvas.gameObject.SetActive(true);
        utilitiesCanvas.settingContent.transform.GetChild(targetIndex).gameObject.SetActive(true);
    }
}
