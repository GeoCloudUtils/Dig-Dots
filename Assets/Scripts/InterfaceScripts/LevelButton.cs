using System;
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
    private void Update()
    {
        passed = PlayerPrefs.GetInt(("level" + levelCounter.text.ToString())) != 0;
        if (passed)
        {
            if (checkImage.GetComponent<_2dxFX_GrayScale>() != null)
                Destroy(checkImage.GetComponent<_2dxFX_GrayScale>());
        }
        else
        {
            if (checkImage.gameObject.GetComponent<_2dxFX_GrayScale>() == null)
                checkImage.gameObject.AddComponent<_2dxFX_GrayScale>();
        }
    }
}
