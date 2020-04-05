﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class LevelCounter : MonoBehaviour
{
    public LevelButton[] LevelButtons;
    public Button backButton;
    public int counter;
    public event UnityAction backToSectionsEvent;
    public event UnityAction DispatchLevelOpen;

    private void Start()
    {
        backButton.onClick.AddListener(DoBack);
        foreach (LevelButton btn in LevelButtons)
            btn.OnButtonClick += Btn_OnButtonClick;
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
    }
    private void Update()
    {
        for (int i = 0; i < LevelButtons.Length; i++)
            LevelButtons[i].leveCounter.SetText(((counter + i) + 1).ToString());
    }
    private void DoBack()
    {
        backButton.gameObject.SetActive(false);
        for (int i = 0; i < LevelButtons.Length; i++)
            LevelButtons[i].transform.DOMove(Vector3.zero, 0.25f);
        Invoke("LoadSectionInterface", 0.26f);
    }
    private void LoadSectionInterface()
    {
        for (int i = 0; i < LevelButtons.Length; i++)
            LevelButtons[i].gameObject.SetActive(false);
        if (backToSectionsEvent != null)
            backToSectionsEvent.Invoke();
    }
}
