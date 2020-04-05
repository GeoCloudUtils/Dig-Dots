using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrolSnapContent : MonoBehaviour
{
    public ScrollSnapContentCounter[] Counters;
    public Button[] SectionsBtn;
    public event UnityAction<int> ShowLevelsContent;

    private void Start()
    {

        for (int i = 0; i < Counters.Length; i++)
        {
            Counters[i].levelCountText.SetText((((i + 1) * 10) + 2).ToString());
            Counters[i].bottomIndicatorText.SetText((i == 0 ? 1 : (i * 10) + 1).ToString() + " / " + ((((i * 10) + 10) + 2).ToString()));
        }
        foreach (Button btn in SectionsBtn)
            btn.onClick.AddListener(() =>
            {
                OpenSectionContent(btn);
            });
    }
    private void OpenSectionContent(Button btn)
    {
        ScrollSnapContentCounter count = btn.GetComponent<ScrollSnapContentCounter>();
        int index = Convert.ToInt32(count.levelCountText.text) - 12;
        if (ShowLevelsContent != null)
            ShowLevelsContent.Invoke(index);
    }
}
