using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLevel : MonoBehaviour
{
    public TextMeshProUGUI bottomIndicatorText;
    public TextMeshProUGUI levelCountText;
    public TextMeshProUGUI infoText;
    public Image hoverImage;
    public Image checkImage;
    public Image levelImage;
    public bool isFirstLevel = false;
    public bool passed = false;
    private int passedLevelsIndex = 0;
    public bool isBlocked = true;

    private void Awake()
    {
        passed = isPassed();
    }
    private void Start()
    {
        if (isFirstLevel)
        {
            isBlocked = false;
            hoverImage.gameObject.SetActive(false);
        }
        LoadInfo();
    }
    public void DoBlock()
    {
        GetComponent<Button>().interactable = false;
        hoverImage.gameObject.SetActive(true);
        if (checkImage.GetComponent<_2dxFX_GrayScale>() == null)
            checkImage.gameObject.AddComponent<_2dxFX_GrayScale>();
        if (levelImage.GetComponent<_2dxFX_GrayScale>() == null)
            levelImage.gameObject.AddComponent<_2dxFX_GrayScale>();
    }
    public void Unblock()
    {
        isBlocked = false;
        GetComponent<Button>().interactable = true;
        hoverImage.gameObject.SetActive(false);
        if (checkImage.GetComponent<_2dxFX_GrayScale>())
            Destroy(checkImage.GetComponent<_2dxFX_GrayScale>());
        if (levelImage.GetComponent<_2dxFX_GrayScale>())
            Destroy(levelImage.GetComponent<_2dxFX_GrayScale>());
    }

    private void LoadInfo()
    {
        if (isBlocked && !isFirstLevel)
        {
            infoText.SetText(0 + " from " + 12 + " levels completed!");
            return;
        }
        for (int i = 1; i < System.Convert.ToInt32(levelCountText.text); i++)
        {
            int x = PlayerPrefs.GetInt("level" + i.ToString());
            if (x != 0)
                passedLevelsIndex++;
        }
        infoText.SetText(passedLevelsIndex + " from " + levelCountText.text + " levels completed!");
    }
    private void Update()
    {
        passed = isPassed(); ;
    }
    private bool isPassed()
    {
        for (int i = 1; i < System.Convert.ToInt32(levelCountText.text); i++)
        {
            int x = PlayerPrefs.GetInt("level" + i.ToString());
            if (x == 0)
                return false;
        }
        return true;
    }
}
