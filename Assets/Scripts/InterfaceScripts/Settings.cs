using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class Settings : MonoBehaviour
{
    public Button soundButton;
    public Sprite soundSwitchSourceImageON;
    public Sprite soundSwitchSourceImageOFF;
    public TextMeshProUGUI ON_Text;
    public TextMeshProUGUI OFF_Text;
    public Slider volumeSlider;

    void Start()
    {
        soundButton.onClick.AddListener(SetSound);
    }
    private void SetSound()
    {
        soundButton.GetComponent<Image>().sprite = soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON ? soundSwitchSourceImageOFF : soundSwitchSourceImageON;
        ON_Text.gameObject.SetActive(soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON);
        OFF_Text.gameObject.SetActive(!ON_Text.gameObject.activeSelf);
    }
}
