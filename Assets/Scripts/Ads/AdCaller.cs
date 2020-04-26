using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;
public class AdCaller : MonoBehaviour, IUnityAdsListener
{
    private string adId = "3574560";
    private string videoId = "video";
    private string rewardedId = "rewardedVideo";

    [Obsolete]
    void Start()
    {
        Monetization.Initialize(adId, true);
    }

    public void ShowAd(bool rewarded)
    {
        if (Monetization.IsReady(rewarded ? rewardedId : videoId))
        {
            ShowAdPlacementContent ad = null;
            ad = Monetization.GetPlacementContent(rewarded ? rewardedId : videoId) as ShowAdPlacementContent;
            if (ad != null)
                ad.Show();
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Ads is ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ads started");
    }
    public void OnUnityAdsDidFinish(string placementId, UnityEngine.Advertisements.ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == UnityEngine.Advertisements.ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("Ads finished");
        }
        else if (showResult == UnityEngine.Advertisements.ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("Ads skipped");
        }
        else if (showResult == UnityEngine.Advertisements.ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }
}
