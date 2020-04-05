using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public GameObject loadinScreen;
    public Button backButton;
    public Button reloadButton;
    public bool canClick = true;
    void Start()
    {
        backButton.onClick.AddListener(GoHome);
        reloadButton.onClick.AddListener(ReloadLevel);
    }
    private void ReloadLevel()
    {
        if (!canClick)
            return;
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadinScreen);
        Navigator.getInstance().LoadLevel("MainGame");
    }

    private void GoHome()
    {
        if (!canClick)
            return;
        Debug.Log("Click");
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadinScreen);
        Navigator.getInstance().LoadLevel("Menu");
    }

    private void FixedUpdate()
    {
        canClick = GameObject.FindObjectOfType<LoadingScreen>() == null;
    }
}
