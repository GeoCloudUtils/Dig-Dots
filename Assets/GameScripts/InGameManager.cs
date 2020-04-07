using DG.Tweening;
using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public GameDoneController ResultCanvas;
    public LevelController[] AllLevels;
    public DOTweenAnimation[] cubeTweens;
    public DOTweenAnimation[] buttonTweens;
    public GameObject loadinScreen;
    public Button backButton;
    public Button reloadButton;
    public bool canClick = true;
    public int currentLevel = 0;
    private LevelController level = null;
    void Start()
    {
        backButton.onClick.AddListener(GoHome);
        reloadButton.onClick.AddListener(ReloadLevel);
        currentLevel = PlayerPrefs.GetInt("levelIndex");
        ShowLevel();
    }
    private void ShowLevel()
    {
        if (level != null)
            Destroy(level.gameObject);
        level = Instantiate<LevelController>(AllLevels[currentLevel - 1], AllLevels[currentLevel].transform.position, Quaternion.identity);
        level.gameObject.SetActive(true);
        level.gameDoneEvent += Level_gameDoneEvent;
    }
    private void Level_gameDoneEvent(bool gameDone)
    {
        ResultCanvas.gameObject.SetActive(true);
        ResultCanvas.SetContent(gameDone);
        foreach (DOTweenAnimation cubeTween in cubeTweens)
            cubeTween.DOPlay();
        foreach (DOTweenAnimation buttonTween in buttonTweens)
            buttonTween.DOPlay();
    }

    private void ReloadLevel()
    {
        if (!canClick)
            return;
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadinScreen);
        Navigator.getInstance().LoadLevel("MainGame");
    }

    private void GoHome()
    {
        if (!canClick)
            return;
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadinScreen);
        Navigator.getInstance().LoadLevel("Menu");
    }

    private void FixedUpdate()
    {
        canClick = GameObject.FindObjectOfType<LoadingScreen>() == null;
    }
}
