using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ScriptUtils.Interface;
using ScriptUtils.GameUtils;

public class InterfaceManager : MonoBehaviour
{
    public Level[] Levels;
    public GameObject loadingScreenPrefab;
    public Button settingButton;
    public Button rateButton;
    public Button likeButton;
    public Button giftButton;
    public Button playButton;
    public GameObject reloadButton;
    public GameObject settingPanel;

    public Transform cloudsTransform;
    public GameObject[] AllInterfaceObjects;
    private Level CurrentLevel;

    public bool canClick = true;
    public int currentLevelIndex = 0;
    void Start()
    {
        settingButton.onClick.AddListener(OpenSettingPanel);
        rateButton.onClick.AddListener(rate);
        likeButton.onClick.AddListener(likeAs);
        giftButton.onClick.AddListener(showGifts);
        playButton.onClick.AddListener(Play);
        reloadButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Reload);

    }
    private void Awake()
    {
        ShowCurrentLevel();
    }

    private void ShowCurrentLevel()
    {
        if (CurrentLevel != null)
            Destroy(CurrentLevel.gameObject);
        CurrentLevel = Instantiate(Levels[currentLevelIndex], Levels[currentLevelIndex].levelPosition, Quaternion.identity);
    }

    public void Reload()
    {
        reloadButton.SetActive(false);
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreenPrefab);
        Navigator.getInstance().LoadLevel("Menu");
    }
    private void Play()
    {
        if (!canClick)
            return;
        foreach (GameObject obj in AllInterfaceObjects)
            obj.SetActive(false);
        ShowLevel();
    }
    private void showGifts()
    {
        if (!canClick)
            return;
    }
    private void likeAs()
    {
        if (!canClick)
            return;
    }
    private void rate()
    {
        if (!canClick)
            return;
    }
    private void OpenSettingPanel()
    {
        if (!canClick)
            return;
        settingPanel.SetActive(true);
    }
    void Update()
    {
        canClick = FindObjectOfType<LoadingScreen>() == null;
    }
    private void ShowLevel()
    {
        cloudsTransform.DOMoveY(0.5f, 1.0f);
        CurrentLevel.transform.DOMoveY(CurrentLevel.levelY, 1.0f).OnComplete(() =>
        {
            RuntimeCircleClipper clipper = FindObjectOfType<RuntimeCircleClipper>();
            clipper.canDig = true;
            reloadButton.gameObject.SetActive(true);
        });
    }
}
