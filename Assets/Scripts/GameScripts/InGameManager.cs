using DG.Tweening;
using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using ScriptUtils.Visual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public ParticleCleanerEvent FX;
    public GameDoneController ResultCanvas;
    public GameObject hintPanel;
    public GameObject levelParent;
    public LevelController[] AllLevels;
    public TextMeshProUGUI levelText;
    public GameObject loadinScreen;
    public Button backButton;
    public Button reloadButton;
    public Button hintButton;
    public bool canClick = true;
    public int currentLevel = 0;
    private LevelController level = null;
    public int testLevelIndex = 0;
    public bool test = false;
    bool done = false;

    private void Awake()
    {
        AllLevels = levelParent.GetComponentsInChildren<LevelController>(true);
    }
    void Start()
    {
        backButton.onClick.AddListener(GoHome);
        reloadButton.onClick.AddListener(DoReload);
        hintButton.onClick.AddListener(OpenHintPanel);
        currentLevel = PlayerPrefs.GetInt("levelIndex");
        ResultCanvas.NextLevelEvent += ResultCanvas_NextLevelEvent;
        ShowLevel();
    }

    private void OpenHintPanel()
    {
        hintPanel.SetActive(true);
        Button closeHintBtn = hintPanel.GetComponentInChildren<Button>();
        closeHintBtn.onClick.AddListener(delegate { CloseHintPanel(closeHintBtn); });
    }

    private void CloseHintPanel(Button btn)
    {
        btn.onClick.RemoveAllListeners();
        hintPanel.SetActive(false);
    }

    private void ResultCanvas_NextLevelEvent()
    {
        PlayerPrefs.SetInt("levelIndex", currentLevel + 1);
        LoadLevel();
    }
    private void DoReload()
    {
        if (FindObjectOfType<LoadingScreen>() != null)
            return;
        reloadButton.interactable = false;
        LoadLevel();
    }
    private void ShowLevel()
    {
        if (level != null)
            Destroy(level.gameObject);
        level = Instantiate<LevelController>(AllLevels[test ? testLevelIndex : currentLevel], AllLevels[test ? testLevelIndex : currentLevel].transform.position, Quaternion.identity);
        level.gameObject.SetActive(true);
        level.gameDoneEvent += Level_gameDoneEvent;
        levelText.text = "Level " + (currentLevel + 1);
    }
    private void Level_gameDoneEvent(bool gameDone)
    {
        done = gameDone;
        if (done)
            (Instantiate(FX, new Vector3(0f, 0f, -2f), Quaternion.identity) as ParticleCleanerEvent).ParticleSystemDone += OnLevelDone;
        else
            OnLevelDone(null);
    }
    private void OnLevelDone(ParticleCleanerEvent target)
    {
        ResultCanvas.gameObject.SetActive(true);
        ResultCanvas.SetContent(done);
    }
    private void LoadLevel()
    {
        if (!canClick)
            return;
        int nextLevelIndex = PlayerPrefs.GetInt("levelIndex");
        if (nextLevelIndex >= AllLevels.Length)
        {
            GoHome();
            return;
        }
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadinScreen);
        Navigator.getInstance().LoadLevel("MainGame");
    }
    public void GoHome()
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
