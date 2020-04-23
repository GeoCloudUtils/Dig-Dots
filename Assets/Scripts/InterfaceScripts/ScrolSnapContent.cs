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
    private void Start()
    {
        foreach (Button btn in SectionsBtn)
            btn.onClick.AddListener(() =>
            {
                OpenSectionContent(btn);
            });
    }
    private void OpenSectionContent(Button btn)
    {
        ScrollLevel level = btn.GetComponent<ScrollLevel>();
        if (ShowLevelsContent != null)
            ShowLevelsContent.Invoke(level.min-1);
    }
}
