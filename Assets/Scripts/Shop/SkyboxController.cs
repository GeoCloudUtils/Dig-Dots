using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkyboxController : MonoBehaviour
{
    public SkyboxCard[] cards;
    public LeanToggle lastToggle;
    public int totalCoins;
    public event UnityAction onNoCoins;
    void Start()
    {
        foreach (SkyboxCard card in cards)
        {
            card.toogleStateChanged += ToggleCard;
            card.onCardClickEvent += Card_onCardClickEvent;
        }
        lastToggle = cards[0].cardToggle;
        lastToggle.On = true;
        if (!PlayerPrefs.HasKey("initToggle"))
            PlayerPrefs.SetInt("initToggle", 0);    
        cards[0].cardToggle.On = true;
    }

    private void Card_onCardClickEvent(int price, SkyboxCard card)
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins");
        Debug.Log("You have " + totalCoins);
        Debug.Log("Ulock price is " + price);
        if (price > totalCoins)
        {
            Debug.Log("NO MORE COINS");
            if (onNoCoins != null)
                onNoCoins.Invoke();
            return;
        }
        PlayerPrefs.SetInt("TotalCoins", totalCoins - price);
        Debug.Log("Remaining coins: " + PlayerPrefs.GetInt("TotalCoins"));
        card.UnlockCard();
    }

    private void ToggleCard(LeanToggle target_card, int index)
    {
        if (PlayerPrefs.HasKey("initToggle"))
        {
            int key = PlayerPrefs.GetInt("initToggle");
            if (key == index)
                PlayerPrefs.DeleteKey("initToggle");
            return;
        }
        if (target_card != lastToggle)
            lastToggle.GetComponent<LeanButton>().interactable = true;
        target_card.Set(true);
        lastToggle.Set(false);
        lastToggle = target_card;
        lastToggle.GetComponent<LeanButton>().interactable = false;
    }
}
