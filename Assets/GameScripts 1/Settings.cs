using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Button soundButton;
    public Button musicButton;
    public Button settingCloseButton;
    public Button settingApplyButton;
    void Start()
    {
        settingCloseButton.onClick.AddListener(CloseSettingPanel);
        settingApplyButton.onClick.AddListener(ApplySettings);
        soundButton.onClick.AddListener(SetSound);
        musicButton.onClick.AddListener(SetMusic);
    }

    private void SetSound()
    {
        bool isOn = soundButton.GetComponent<Image>().color == Color.white;
        soundButton.GetComponent<Image>().color = isOn ? Color.red : Color.white;
    }

    private void SetMusic()
    {
        bool isOn = musicButton.GetComponent<Image>().color == Color.white;
        musicButton.GetComponent<Image>().color = isOn ? Color.red : Color.white;
    }

    private void ApplySettings()
    {
        gameObject.SetActive(false);
    }

    private void CloseSettingPanel()
    {
        gameObject.SetActive(false);
    }
}
