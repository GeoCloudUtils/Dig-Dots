using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelCounter;
    public event UnityAction OnButtonClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenGame);
    }

    private void OpenGame()
    {
        if (OnButtonClick != null)
            OnButtonClick.Invoke();
        PlayerPrefs.SetInt("levelIndex", Convert.ToInt32(levelCounter.text));
    }

    private void Update()
    {
        if (levelCounter == null)
            levelCounter = GetComponentInChildren<TextMeshProUGUI>();
    }

}
