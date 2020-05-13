using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Timeline;

public class AdCaller : MonoBehaviour
{
    private string adId = "3574560";
    private GameAdCounter adCounter;
    [Obsolete]
    void Start()
    {
        Advertisement.Initialize(adId, true);
        if (adCounter == null)
            adCounter = FindObjectOfType<GameAdCounter>();
    }
    public void CountAds()
    {
        adCounter.counter++;
        if (adCounter.counter == adCounter.nextStetp)
        {
            ShowAd();
            adCounter.nextStetp += adCounter.step;
        }
    }
    public void ShowAd()
    {
        Advertisement.Show();
    }
}
