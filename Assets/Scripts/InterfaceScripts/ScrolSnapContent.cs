using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrolSnapContent : MonoBehaviour
{
    public ScrollLevel[] ContentLevels;
    public Button[] SectionsBtn;
    public event UnityAction<int> ShowLevelsContent;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        for (int i = 0; i < ContentLevels.Length; i++)
        {
            ContentLevels[i].levelCountText.SetText((((i + 1) * 10) + 2).ToString());
            ContentLevels[i].bottomIndicatorText.SetText((i == 0 ? 1 : (i * 10) + 1).ToString() + " - " + ((((i * 10) + 10) + 2).ToString()));
        }
    }
    private void Start()
    {
        foreach (Button btn in SectionsBtn)
            btn.onClick.AddListener(() =>
            {
                OpenSectionContent(btn);
            });
        OnGameLoad();
    }
    private void OpenSectionContent(Button btn)
    {
        ScrollLevel count = btn.GetComponent<ScrollLevel>();
        int index = Convert.ToInt32(count.levelCountText.text) - 12;
        if (ShowLevelsContent != null)
            ShowLevelsContent.Invoke(index);
    }

    private void OnGameLoad()
    {
        for (int i = 0; i < ContentLevels.Length - 1; i++)
        {
            if (ContentLevels[i].passed)
                ContentLevels[i + 1].Unblock();
            else
                ContentLevels[i + 1].DoBlock();
        }
    }
}
