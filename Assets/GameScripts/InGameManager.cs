using DG.Tweening;
using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public ScreenshotHandler scHandler;
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
    void Start()
    {
        backButton.onClick.AddListener(GoHome);
        reloadButton.onClick.AddListener(LoadLevel);
        currentLevel = PlayerPrefs.GetInt("levelIndex");
        ResultCanvas.NextLevelEvent += ResultCanvas_NextLevelEvent;
        ShowLevel();
    }

    private void ResultCanvas_NextLevelEvent()
    {
        PlayerPrefs.SetInt("levelIndex", currentLevel + 1);
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
        ResultCanvas.gameObject.SetActive(true);
        ResultCanvas.SetContent(gameDone);
        foreach (DOTweenAnimation buttonTween in buttonTweens)
            buttonTween.DOPlay();
        //scHandler.TakeScreenShot(1920, 1080);
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
