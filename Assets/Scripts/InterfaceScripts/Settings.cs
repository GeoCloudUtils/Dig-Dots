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
    void Start()
    {
        resetProgressButton.onClick.AddListener(ResteGame);
        soundButton.onClick.AddListener(SetSound);
        foreach (Toggle toggle in qualityToggleGroup)
            toggle.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(toggle);
            });
    }

    private void ResteGame()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
            button.interactable = false;
        resetPanel.SetActive(true);
        StartCoroutine(DoReset());
    }

    private IEnumerator DoReset()
    {
        yield return new WaitForSeconds(resetDelay);
        PlayerPrefs.DeleteAll();
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("Menu");
    }

    private void ToggleValueChanged(Toggle m_Toggle)
    {
        QualitySettings.SetQualityLevel(Array.IndexOf(qualityToggleGroup, m_Toggle));
    }

    private void SetSound()
    {
        soundButton.GetComponent<Image>().sprite = soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON ? soundSwitchSourceImageOFF : soundSwitchSourceImageON;
        ON_Text.gameObject.SetActive(soundButton.GetComponent<Image>().sprite == soundSwitchSourceImageON);
        OFF_Text.gameObject.SetActive(!ON_Text.gameObject.activeSelf);
    }
}
