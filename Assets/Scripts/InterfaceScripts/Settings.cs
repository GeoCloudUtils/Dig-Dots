using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;
using ScriptUtils.Interface;
using ScriptUtils.GameUtils;

public class Settings : MonoBehaviour
{
    public GameObject alertPanel;
    public GameObject resetPanel;
    public GameObject loadingScreen;
    public Button soundButton;
    public Sprite soundSwitchSourceImageON;
    public Sprite soundSwitchSourceImageOFF;
    public Button resetProgressButton;
    public TextMeshProUGUI ON_Text;
    public TextMeshProUGUI OFF_Text;
    public Slider volumeSlider;
    public Toggle[] qualityToggleGroup;
    public float resetDelay = 1.5f;

    public const float DefaultVolumeLevel = 0.5f;
    private SoundGameManager sManager;
    void Start()
    {
        volumeSlider.normalizedValue = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : DefaultVolumeLevel;
        resetProgressButton.onClick.AddListener(ResteGame);
        soundButton.onClick.AddListener(delegate { SetSound(true); });
        if (!PlayerPrefs.HasKey("SoundState"))
            PlayerPrefs.SetInt("SoundState", 1);
        SetSound(false);
        foreach (Toggle toggle in qualityToggleGroup)
            toggle.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(toggle);
            });
        sManager = FindObjectOfType<SoundGameManager>();
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });
    }

    private void changeVolume(float value)
    {
        sManager.SetMusicVolume(value);
        PlayerPrefs.SetFloat("Volume", value);
    }
    private void ResteGame()
    {
        alertPanel.SetActive(true);
        Button[] alertButtons = GetTargetButtons();
        foreach (Button btn in alertButtons)
            btn.onClick.AddListener(() =>
           { DoAlertButtonAction(btn); });
    }

    private Button[] GetTargetButtons()
    {
        Button[] alertButtons = alertPanel.GetComponentsInChildren<Button>();
        return alertButtons;
    }

    private void DoAlertButtonAction(Button btn)
    {
        if (btn.name == "noButton")
        {
            alertPanel.SetActive(false);
            Button[] alertButtons = GetTargetButtons();
            foreach (Button alButton in alertButtons)
                alButton.onClick.RemoveAllListeners();
        }
        else
            StartCoroutine(DoReset());
    }

    private IEnumerator DoReset()
    {
        alertPanel.SetActive(false);
        resetPanel.SetActive(true);
        sManager.SetMusicVolume(0);
        yield return new WaitForSeconds(resetDelay);
        PlayerPrefs.DeleteAll();
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("GameLogo");
    }

    private void ToggleValueChanged(Toggle m_Toggle)
    {
        QualitySettings.SetQualityLevel(Array.IndexOf(qualityToggleGroup, m_Toggle));
    }

    private void SetSound(bool handleChanged)
    {
        if (handleChanged)
        {
            if (PlayerPrefs.GetInt("SoundState") == 1)
                PlayerPrefs.SetInt("SoundState", 0);
            else
                PlayerPrefs.SetInt("SoundState", 1);
        }
        Debug.Log(PlayerPrefs.GetInt("SoundState"));
        soundButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt("SoundState") == 1 ? soundSwitchSourceImageON : soundSwitchSourceImageOFF;
        ON_Text.gameObject.SetActive(soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON);
        OFF_Text.gameObject.SetActive(!ON_Text.gameObject.activeSelf);

    }
}
