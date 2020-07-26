using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkyboxCard : MonoBehaviour
{
    public LeanToggle cardToggle;
    public GameObject cardCoinButton;
    public bool unlocked = false;
    public event UnityAction<LeanToggle, int> toogleStateChanged;
    public event UnityAction<int, SkyboxCard> onCardClickEvent;
    public int unlockPrice;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoCardClick);
        SetCardState();
    }
    private void DoCardClick()
    {
        if (unlocked)
            return;
        if (onCardClickEvent != null)
            onCardClickEvent.Invoke(unlockPrice, this);
    }

    public void UnlockCard()
    {
        PlayerPrefs.SetInt("card" + transform.GetSiblingIndex(), 1);
        SetCardState();
    }

    public void SetCardState()
    {
        if(transform.GetSiblingIndex() !=0)
            unlocked = PlayerPrefs.HasKey("card" + transform.GetSiblingIndex());
        cardToggle.gameObject.SetActive(unlocked);
        cardCoinButton.SetActive(!unlocked);
        TextMeshProUGUI tmp = cardCoinButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tmp.SetText((1000 * transform.GetSiblingIndex()).ToString());
        unlockPrice = System.Convert.ToInt32(tmp.text);
    }
    public void OnToogleOn()
    {
        if (toogleStateChanged != null)
            toogleStateChanged.Invoke(cardToggle, transform.GetSiblingIndex());
    }
}
