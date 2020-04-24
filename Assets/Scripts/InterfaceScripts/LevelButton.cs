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
    public event UnityAction OnButtonClick;
    public bool passed = false;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenGame);
    }
    private void OpenGame()
    {
        if (OnButtonClick != null)
            OnButtonClick.Invoke();
        PlayerPrefs.SetInt("levelIndex", Convert.ToInt32(levelCounter.text) - 1);
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
