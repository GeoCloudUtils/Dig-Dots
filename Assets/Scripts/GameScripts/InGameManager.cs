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

    public List<int> AdsSteps = new List<int>();
    void Start()
    {
        backButton.onClick.AddListener(GoHome);
        reloadButton.onClick.AddListener(DoReload);
        currentLevel = PlayerPrefs.GetInt("levelIndex");
        ResultCanvas.NextLevelEvent += ResultCanvas_NextLevelEvent;
        GetAds();
        ShowLevel();
    }

    private void GetAds()
    {
        foreach (int number in EvenSequence(2, AllLevels.Length))
            AdsSteps.Add(number);
    }
    public static IEnumerable<int> EvenSequence(int firstNumber, int lastNumber)
    {
        // Yield even numbers in the range.
        for (int number = firstNumber; number <= lastNumber - 1; number++)
        {
            if (number % 3 == 0)
                yield return number;
        }
    }
    private void ResultCanvas_NextLevelEvent()
    {
        if (AdsSteps.Contains(currentLevel))
        {
            AdsSteps.Clear();
            adCaller.ShowAd(false);
        }
        else
        {
            PlayerPrefs.SetInt("levelIndex", currentLevel + 1);
            LoadLevel(false);
        }
    }

    private void DoReload()
    {
        LoadLevel(true);
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
        if (done)
            PlayerPrefs.SetString("Level" + (currentLevel + 1).ToString(), "Level" + (currentLevel + 1).ToString());
        foreach (DOTweenAnimation buttonTween in buttonTweens)
            buttonTween.DOPlay();
    }

    private void LoadLevel(bool reload)
    {
        if (!canClick)
            return;
        if (AdsSteps.Contains(currentLevel))
            adCaller.ShowAd(false);
        if (reload)
        {
            if (Random.value > 0.6f)
                adCaller.ShowAd(false);
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
