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
    public Button settingCloseButton;
    public Transform settingContent;
    public Image contentBg;
    public event UnityAction contentAction;
    private Tween contentTween;
    void Start()
    {
        settingCloseButton.onClick.AddListener(HideContent);
        soundButton.onClick.AddListener(SetSound);

    }
    private void OnEnable()
    {
        ShowContent();
    }

    private void ShowContent()
    {
        if (contentTween != null)
            contentTween.Kill();
        contentBg.DOFade(0.5f, 0.25f);
        contentTween = settingContent.DORotateQuaternion(Quaternion.Euler(Vector3.zero), 0.2f).SetEase(Ease.Linear);
    }
    private void HideContent()
    {
        if (contentTween != null)
            contentTween.Kill();
        contentBg.DOFade(0f, 0.25f);
        contentTween = settingContent.DORotateQuaternion(Quaternion.Euler(0f, 0f, 25f), 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (contentAction != null)
                contentAction.Invoke();
        });
    }
    private void SetSound()
    {
        soundButton.GetComponent<Image>().sprite = soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON ? soundSwitchSourceImageOFF : soundSwitchSourceImageON;
        ON_Text.gameObject.SetActive(soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON);
        OFF_Text.gameObject.SetActive(!ON_Text.gameObject.activeSelf);
    }
}
