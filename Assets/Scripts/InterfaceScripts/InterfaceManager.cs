using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ScriptUtils.Interface;
using ScriptUtils.GameUtils;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InterfaceManager : MonoBehaviour
{
    public ScrollLevel[] AllChapters;
    public GameObject loadingScreen;
    public UtilitiesController utilitiesCanvas;
    public Button[] InterfaceButtons;
    public Button ratingButton;
    public ScrolSnapContent snapContent;
    public LevelsContent level;
    public bool clearLocalStorage = false;
    private bool interactable = true;
    public int[] chaptersLevelsMax;
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
        interactable = true;
        chaptersLevelsMax = new int[AllChapters.Length];
        GameDataSave saveData = FindObjectOfType<GameDataSave>();
        if (saveData != null)
            Destroy(saveData.gameObject);
        SetChaptersMax();
    }
    private void SetChaptersMax()
    {
        chaptersLevelsMax[0] = 12;
        for (int i = 1; i < chaptersLevelsMax.Length; i++)
        {
            int last = chaptersLevelsMax[i - 1] + 12;
            chaptersLevelsMax[i] = last;
        }
    }

    private void Level_DispatchLevelOpen(int levelIndex)
    {
        int max = 0;
        for (int i = 0; i < chaptersLevelsMax.Length; i++)
        {
            if ((levelIndex + 1) <= chaptersLevelsMax[i])
            {
                max = chaptersLevelsMax[i];
                break;
            }
        }
        PlayerPrefs.SetInt("CHAPTER_MAX", max);
        Debug.Log("Max chapter:" + PlayerPrefs.GetInt("CHAPTER_MAX"));
        LoadInterface();
    }
    private void CheckChapters()
    {
        for (int i = 1; i < AllChapters.Length; i++)
        {
            ScrollLevel chapter = AllChapters[i];
            ScrollLevel lastChapter = AllChapters[i - 1];
            if (!lastChapter.complete)
                return;
            if (chapter.isBlocked)
                chapter.Unblock();
        }
    }

    private void LoadInterface(bool reload = false)
    {
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel(reload ? "Menu" : "MainGame");
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

    private void Update()
    {
        if (clearLocalStorage && interactable)
        {
            interactable = false;
            LoadInterface(true);
            PlayerPrefs.DeleteAll();
            clearLocalStorage = false;
        }

        CheckChapters();
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "ratingScreen")
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
