using DG.Tweening;
using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using ScriptUtils.Visual;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public GameDataSave dataSave;
    public AdCaller adCaller;
    public ParticleCleanerEvent FX;
    public GameDoneController ResultCanvas;
    public LevelController[] AllLevels;
    public DOTweenAnimation[] buttonTweens;
    public TextMeshProUGUI levelText;
    public GameObject loadinScreen;
    public Button backButton;
    public Button reloadButton;
    public bool canClick = true;
    public int currentLevel = 0;
    private LevelController level = null;
    public int testLevelIndex = 0;
    public bool test = false;
    bool done = false;

    void Start()
    {
        dataSave = GameObject.FindObjectOfType<GameDataSave>();
        backButton.onClick.AddListener(GoHome);
        reloadButton.onClick.AddListener(DoReload);
        currentLevel = PlayerPrefs.GetInt("levelIndex");
        ResultCanvas.NextLevelEvent += ResultCanvas_NextLevelEvent;
        ShowLevel();
    }
    private void ResultCanvas_NextLevelEvent(bool disableAdOnLoad)
    {
        PlayerPrefs.SetInt("levelIndex", currentLevel + 1);
        LoadLevel(false, disableAdOnLoad);
    }
    private void DoReload()
    {
        if (!PlayerPrefs.HasKey("ReloadLevel"))
            PlayerPrefs.SetInt("ReloadLevel", 0);
        LoadLevel(true, false);
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
        foreach (DOTweenAnimation buttonTween in buttonTweens)
            buttonTween.DOPlay();
    }
    private void LoadLevel(bool reload, bool disableAdOnLoad)
    {
        if (!canClick)
            return;
        if (!reload)
        {
            if ((dataSave.levelIndex > 2 && dataSave.levelIndex % 4 == 0) && !disableAdOnLoad)
                adCaller.ShowAd(false);
            dataSave.levelIndex++;
        }
        else
        {
            if (!ResultCanvas.gameObject.activeSelf)
            {
                if (dataSave.reloadIndex > 3 && dataSave.reloadIndex % 4 == 0)
                    adCaller.ShowAd(false);
            }
            dataSave.reloadIndex++;
        }
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
