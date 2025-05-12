using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Android")]
    [SerializeField]
    private string androidGameId = string.Empty;
    [SerializeField]
    private string androidBannerAdUnitId = string.Empty;
    [SerializeField]
    private string androidInterstitialAdUnitId = string.Empty;
    [SerializeField]
    private string androidRewardedAdUnitId = string.Empty;

    [Header("iOS")]
    [SerializeField]
    private string iosGameId = string.Empty;
    [SerializeField]
    private string iosBannerAdUnitId = string.Empty;
    [SerializeField]
    private string iosInterstitialAdUnitId = string.Empty;
    [SerializeField]
    private string iosRewardedAdUnitId = string.Empty;

    [SerializeField]
    private BannerPosition bannerPosition = BannerPosition.TOP_CENTER;
    [SerializeField]
    private string rewardType = string.Empty;
    [SerializeField]
    private double rewardAmount = 0;
    
    private string gameId = string.Empty;
    private string bannerAdUnitId = string.Empty;
    private string interstitialAdUnitId = string.Empty;
    private string rewardedAdUnitId = string.Empty;

    private bool testMode = true;
    private Action initializedAction = null;
    private Action onLoadCompleteInterstitialAd = null;
    private Action onLoadCompleteRewardedAd = null;
    private Action<string, double> onCompleterRewardedAd = null;

    public void Initialize(Action initializedAction = null)
    {
        #if UNITY_IPHONE
            gameId = iosGameId;
            bannerAdUnitId = iosBannerAdUnitId;
            interstitialAdUnitId = iosInterstitialAdUnitId;
            rewardedAdUnitId = iosRewardedAdUnitId;
        #else
            gameId = androidGameId;
            bannerAdUnitId = androidBannerAdUnitId;
            interstitialAdUnitId = androidInterstitialAdUnitId;
            rewardedAdUnitId = androidRewardedAdUnitId;
        #endif

        this.initializedAction = initializedAction;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("UnityAds initialization completed.");
        initializedAction?.Invoke();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("UnityAds initialization failed");
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        if (adUnitId.Equals(interstitialAdUnitId))
        {
            onLoadCompleteInterstitialAd?.Invoke();
        }
        else if (adUnitId.Equals(rewardedAdUnitId))
        {
            onLoadCompleteRewardedAd?.Invoke();
        }
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(rewardedAdUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("UnityAds Rewarded Ad Completed");
            onCompleterRewardedAd?.Invoke(rewardType, rewardAmount);
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }


#region Banner Ad.

    public void LoadBannerAd()
    {
        Debug.Log("UnityAds load banner ad.");

        BannerLoadOptions options = new()
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError,
        };

        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Load(bannerAdUnitId, options);
    }

    public void ShowBannerAd()
    {
        Debug.Log("UnityAds show banner ad.");

        BannerOptions options = new()
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown,
        };

        Advertisement.Banner.Show(bannerAdUnitId, options);
    }

    public void HideBannerAd()
    {
        Debug.Log("UnityAds hide banner ad.");
        Advertisement.Banner.Hide();
    }

    private void OnBannerLoaded()
    {
        Debug.Log("UnityAds banner loaded.");
    }

    private void OnBannerError(string message)
    {
        Debug.Log("UnityAds banner error : " + message);
    }
 
    private void OnBannerClicked() { }
    private void OnBannerShown() { }
    private void OnBannerHidden() { }

#endregion

#region Interstitial Ad.

    public void LoadInterstitialAd(Action onLoadComplete = null)
    {
        Debug.Log("UnityAds load interstitial ad.");
        onLoadCompleteInterstitialAd = onLoadComplete;
        Advertisement.Load(interstitialAdUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        Debug.Log("UnityAds show interstitial ad.");
        Advertisement.Show(interstitialAdUnitId, this);
    }
#endregion

#region Rewarded Ad.
    public void LoadRewardedAd(Action onLoadComplete = null)
    {
        Debug.Log("UnityAds load rewarded ad.");
        onLoadCompleteRewardedAd = onLoadComplete;
        Advertisement.Load(rewardedAdUnitId, this);
    }

    public void ShowRewardedAd(Action<string, double> onCompleted)
    {
        Debug.Log("UnityAds show rewarded ad.");
        onCompleterRewardedAd = onCompleted;
        Advertisement.Show(rewardedAdUnitId, this);
    }
#endregion

}
