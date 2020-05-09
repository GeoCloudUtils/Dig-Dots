﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelCounter;
    public Image checkImage;
    public Image levelScreenShotImage;
    public event UnityAction<int> OnButtonClick;
    public bool passed = false;
    public int levelToOpen = 0;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenGame);
    }
    private void OpenGame()
    {
        if (levelToOpen < 1)
        {
            Debug.LogError("Something goes wrong. Check LevelsContent Class for setting this levelIndex");
            return;
        }
        if (OnButtonClick != null)
            OnButtonClick.Invoke(levelToOpen - 1);
        PlayerPrefs.SetInt("levelIndex", levelToOpen - 1);
    }

    public void SetActive()
    {
        if (checkImage.GetComponent<_2dxFX_GrayScale>() != null)
            Destroy(checkImage.GetComponent<_2dxFX_GrayScale>());
        if (levelScreenShotImage.sprite != null)
        {
            if (levelScreenShotImage.GetComponent<_2dxFX_GrayScale>() != null)
                Destroy(levelScreenShotImage.gameObject.GetComponent<_2dxFX_GrayScale>());
        }
    }
    public void SetInactive()
    {
        if (checkImage.gameObject.GetComponent<_2dxFX_GrayScale>() == null)
            checkImage.gameObject.AddComponent<_2dxFX_GrayScale>();
        if (levelScreenShotImage.sprite != null)
        {
            if (levelScreenShotImage.GetComponent<_2dxFX_GrayScale>() == null)
                levelScreenShotImage.gameObject.AddComponent<_2dxFX_GrayScale>();
        }
    }
}
