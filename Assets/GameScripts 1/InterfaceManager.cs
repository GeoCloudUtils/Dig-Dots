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
    public GameObject loadingScreenPrefab;
    public Button settingButton;
    public Button rateButton;
    public Button likeButton;
    public Button giftButton;
    public Button playButton;
    public GameObject reloadButton;
    public GameObject settingPanel;

    public Transform cloudsTransform;
    public Transform level;
    public GameObject[] AllInterfaceObjects;

    public bool canClick = true;
    void Start()
    {
        settingButton.onClick.AddListener(OpenSettingPanel);
        rateButton.onClick.AddListener(rate);
        likeButton.onClick.AddListener(likeAs);
        giftButton.onClick.AddListener(showGifts);
        playButton.onClick.AddListener(Play);
        reloadButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Reload);
    }

    private void Reload()
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
        level.transform.DOMoveY(0f, 1.0f).OnComplete(() =>
        {
            reloadButton.gameObject.SetActive(true);
        });
    }
}
