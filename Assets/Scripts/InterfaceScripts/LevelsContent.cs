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
    public event UnityAction DispatchLevelOpen;
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
    private void Btn_OnButtonClick()
    {
        foreach (LevelButton btn in LevelButtons)
            btn.GetComponent<Button>().interactable = false;
        if (DispatchLevelOpen != null)
            DispatchLevelOpen.Invoke();
    }

    private void OnEnable()
    {
        for (int i = 0; i < LevelButtons.Length; i++)
            LevelButtons[i].gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if (LevelButtons[i].levelCounter != null)
                LevelButtons[i].levelCounter.SetText(((counter + i) + 1).ToString());
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
