using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Pixelplacement;
public class UnityAdsController : Singleton<UnityAdsController>, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "3796612";
#elif UNITY_ANDROID
    private string gameId = "3796613";
#elif UNITY_EDITOR
    private string gameId = "1486550";
#endif
    public string rewardPlacementId = "rewardedVideo";
    private System.Action onUserEarnedReward, onAdClosed;
    protected override void OnRegistration()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, false);
    }
    public bool IsVideoRewardAdsReady()
    {
        return Advertisement.IsReady(rewardPlacementId);
    }
    public void ShowVideoAds(System.Action onUserEarnedReward, System.Action onAdClosed)
    {
        Advertisement.Show(rewardPlacementId);
        this.onUserEarnedReward = onUserEarnedReward;
        this.onAdClosed = onAdClosed;
    }
    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == rewardPlacementId)
        {
        }
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("Complete watch ads");
            if (onUserEarnedReward != null)
                onUserEarnedReward.Invoke();
            else
                Debug.LogError("no ads earned event fired");
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("Did not complete watch ads");
            if (onAdClosed != null)
                onAdClosed.Invoke();
            else
                Debug.LogError("no ads close event fired");
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }
    public void OnUnityAdsDidError(string message)
    {
        Advertisement.RemoveListener(this);
        Debug.Log("OnUnityAdsDidError: " + message);
        // Log the error.
    }
    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
