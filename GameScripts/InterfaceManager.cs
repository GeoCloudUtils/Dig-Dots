using DG.Tweening;
using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public GameObject lScreen;
    public Button soundButton;
    public Button musicButton;
    public Button playButton;
    public Button reloadButton;
    public GameObject[] InterfaceObjects;
    void Start()
    {
        soundButton.onClick.AddListener(EnableSound);
        musicButton.onClick.AddListener(EnableMusic);
        playButton.onClick.AddListener(Play);
        reloadButton.onClick.AddListener(DoReload);
    }

    private void DoReload()
    {
        reloadButton.interactable = false;
        reloadButton.gameObject.SetActive(false);
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(lScreen);
        Navigator.getInstance().LoadLevel("MainGame");
    }
    private void Play()
    {
        foreach (GameObject go in InterfaceObjects)
            go.SetActive(false);
        reloadButton.gameObject.SetActive(true);
    }

    private void EnableMusic()
    {
        musicButton.GetComponent<Image>().color = musicButton.GetComponent<Image>().color != Color.white ? Color.white : Color.red;
    }

    private void EnableSound()
    {
        soundButton.GetComponent<Image>().color = soundButton.GetComponent<Image>().color != Color.white ? Color.white : Color.red;
    }
}
