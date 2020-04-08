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
    public event UnityAction OnButtonClick;
    private int currentLevelIndex;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenGame);
        currentLevelIndex = PlayerPrefs.GetInt("levelIndex");
    }

    private void OpenGame()
    {
        if (OnButtonClick != null)
            OnButtonClick.Invoke();
        PlayerPrefs.SetInt("levelIndex", Convert.ToInt32(levelCounter.text) - 1);
    }

    private void Update()
    {
        if (levelCounter == null)
            levelCounter = GetComponentInChildren<TextMeshProUGUI>();
    }

}
