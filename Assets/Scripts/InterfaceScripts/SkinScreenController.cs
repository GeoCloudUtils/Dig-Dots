using DG.Tweening;
using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinScreenController : MonoBehaviour
{
    public LeanButton[] skin_switch_buttons;
    public GameObject[] contents;
    public LeanWindow noCoinsWindow;
    public SkyboxController skyBoxC;
    public TextMeshProUGUI skin_name;
    string[] skinNames = new string[3] { "Red skin Market","Blue skin Market","Colors"};
    void Start()
    {
        foreach (LeanButton skin_button in skin_switch_buttons)
            skin_button.OnClick.AddListener(delegate { ChangeSkinContent(skin_button); });
        ChangeSkinContent(skin_switch_buttons[0]);
        skyBoxC.onNoCoins += SkyBoxC_onNoCoins;
    }

    private void SkyBoxC_onNoCoins()
    {
        noCoinsWindow.On = true;
    }

    private void ChangeSkinContent(LeanButton skin_button)
    {
        int targetIndex = System.Array.IndexOf(skin_switch_buttons, skin_button);
        contents[targetIndex].transform.SetSiblingIndex(contents.Length-1);
        contents[targetIndex].SetActive(true);
        foreach (LeanButton s_button in skin_switch_buttons)
            s_button.interactable = false;
        skin_name.SetText(skinNames[targetIndex]);
        contents[targetIndex].GetComponent<RectTransform>().DOMoveX(0f, 0.5f).OnComplete(()=>
        {
            foreach (LeanButton s_button in skin_switch_buttons)
                s_button.interactable = true;
            foreach (GameObject content in contents)
            {
                if (content != contents[targetIndex])
                {
                    content.GetComponent<RectTransform>().DOLocalMoveX(1500f, 0f);
                    content.SetActive(false);
                }
            }
        });
    }
}
