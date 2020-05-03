using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
public class LevelsContent : MonoBehaviour
{
    public LevelButton[] LevelButtons;
    public Button backButton;
    public int counter;
    public event UnityAction backToSectionsEvent;
    public event UnityAction<int> DispatchLevelOpen;
    public Sprite[] Icons; // icons array
    private void Start()
    {
        backButton.onClick.AddListener(DoBack);
        foreach (LevelButton btn in LevelButtons)
            btn.OnButtonClick += Btn_OnButtonClick;
        LoadIcons();
    }

    private void Awake()
    {
        LevelButtons = gameObject.GetComponentsInChildren<LevelButton>();
    }
    private void Btn_OnButtonClick(int levelIndex)
    {
        foreach (LevelButton btn in LevelButtons)
            btn.GetComponent<Button>().interactable = false;
        if (DispatchLevelOpen != null)
            DispatchLevelOpen.Invoke(levelIndex);
    }

    private void OnEnable()
    {
        for (int i = 0; i < LevelButtons.Length; i++)
            LevelButtons[i].gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        for (int i = 0; i < LevelButtons.Length; i++)
        {
            LevelButton btn = LevelButtons[i];
            btn.name = "Level" + ((counter + 1) + i);
            string levelStr = PlayerPrefs.GetString("Level" + ((counter + 1) + i).ToString());
            string levelName = btn.name;
            if (levelStr.Equals(levelName))
                btn.SetActive();
            else
                btn.SetInactive();
            if (btn.levelCounter != null)
                btn.levelCounter.SetText("   Level " + ((counter + i) + 1).ToString());
        }
    }
    private void DoBack()
    {
        backButton.gameObject.SetActive(false);
        Invoke("LoadSectionInterface", 0.26f);
    }
    private void LoadSectionInterface()
    {
        for (int i = 0; i < LevelButtons.Length; i++)
            LevelButtons[i].gameObject.SetActive(false);
        if (backToSectionsEvent != null)
            backToSectionsEvent.Invoke();
    }
    void LoadIcons()
    {
        object[] loadedIcons = Resources.LoadAll("LevelsScreenshots/section1", typeof(Sprite));
        Icons = new Sprite[LevelButtons.Length];
        for (int x = 0; x < loadedIcons.Length; x++)
            Icons[x] = (Sprite)loadedIcons[x];
        for (int i = 0; i < LevelButtons.Length; i++)
        {
            for (int k = 0; k < Icons.Length; k++)
            {
                if (Icons[k].name == "level" + (i + 1))
                    LevelButtons[i].levelScreenShotImage.sprite = Icons[k];
            }
        }
    }
}
