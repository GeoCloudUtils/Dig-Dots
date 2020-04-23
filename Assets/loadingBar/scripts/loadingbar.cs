using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour
{
    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;
    private bool loadStart = false;
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }
    void Update()
    {
        if (imageComp.fillAmount != 1f)
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
        else
        {
            if (!loadStart)
            {
                loadStart = true;
                LoadInterface();
            }
        }
    }
    public GameObject loadingScreen;
    public void LoadInterface()
    {
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("Menu");
    }
}
