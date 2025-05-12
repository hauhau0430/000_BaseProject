using System;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;

public class GoogleAdMobManager : MonoBehaviour
{

#if DEBUG
    #if UNITY_ANDROID
        private string debugBannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        private string debugInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        private string debugRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    #elif UNITY_IPHONE
        private string debugBannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
        private string debugInterstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
        private string debugRewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    #else
        private string debugBannerAdUnitId = "unused";
        private string debugInterstitialAdUnitId = "unused";
        private string debugRewardedAdUnitId = "unused";
    #endif
#endif

    
    [Header("Android")]
    [SerializeField]
    private string androidBannerAdUnitId = string.Empty;
    [SerializeField]
    private string androidInterstitialAdUnitId = string.Empty;
    [SerializeField]
    private string androidRewardedAdUnitId = string.Empty;

    [Header("iOS")]
    [SerializeField]
    private string iosBannerAdUnitId = string.Empty;
    [SerializeField]
    private string iosInterstitialAdUnitId = string.Empty;
    [SerializeField]
    private string iosRewardedAdUnitId = string.Empty;

    [SerializeField]
    private AdPosition bannerPosition = AdPosition.Top;

    public bool CanShowBannerAd => isLoadedBannerAd;
    public bool CanShowInterstitialAd => interstitialAd != null && interstitialAd.CanShowAd();
    public bool CanShowRewardedAd => rewardedAd != null && rewardedAd.CanShowAd();

    private string bannerAdUnitId = string.Empty;
    private string interstitialAdUnitId = string.Empty;
    private string rewardedAdUnitId = string.Empty;

    private BannerView bannerView = null;
    private InterstitialAd interstitialAd = null;
    private RewardedAd rewardedAd = null;

    private bool isLoadedBannerAd = false;

    [SerializeField] private TextMeshProUGUI logText = null;

    public void Initialize(Action initializedAction = null)
    {
        #if DEBUG
            bannerAdUnitId = debugBannerAdUnitId;
            interstitialAdUnitId = debugInterstitialAdUnitId;
            rewardedAdUnitId = debugRewardedAdUnitId;
        #elif UNITY_ANDROID
            bannerAdUnitId = androidBannerAdUnitId;
            interstitialAdUnitId = androidInterstitialAdUnitId;
            rewardedAdUnitId = androidRewardedAdUnitId;
        #elif UNITY_IPHONE
            bannerAdUnitId = iosBannerAdUnitId;
            interstitialAdUnitId = iosInterstitialAdUnitId;
            rewardedAdUnitId = iosRewardedAdUnitId;
        #else
            bannerAdUnitId = "unused";
            interstitialAdUnitId = "unused";
            rewardedAdUnitId = "unused";
        #endif
        
        isLoadedBannerAd = false;

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            initializedAction?.Invoke();
        });
    }


#region Banner Ad.

    public void LoadBannerAd()
    {
        if (bannerView == null)
        {
            CreateBannerView(bannerPosition);
        }

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
        
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroy banner view.");
            bannerView.Destroy();
            bannerView = null;
            isLoadedBannerAd = false;
        }
    }

    private void CreateBannerView(AdPosition adPosition)
    {
        Debug.Log("Creating banner view.");

        isLoadedBannerAd = false;

        if (bannerView != null)
        {
            DestroyBanner();
        }

        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, adPosition);
        RegisterBannerEventHandlers();
    }

    private void RegisterBannerEventHandlers()
    {
        if (bannerView == null) return;

        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
            isLoadedBannerAd = true;
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += error =>
        {
            Debug.LogError("Banner view failed to load an ad with error : " + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid +=  adValue =>
        {
            Debug.Log(string.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

#endregion


#region Interstitial Ad.

    public void LoadInterstitialAd()
    {
        DestroyInterstitialAd();

        Debug.Log("Loading interstitial ad.");

        var adRequest = new AdRequest();

        InterstitialAd.Load(
            interstitialAdUnitId,
            adRequest,
            (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterInterstitialEventHandlers();
            });
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            Debug.Log("Destroy interstitial ad.");
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterInterstitialEventHandlers()
    {
        if (interstitialAd == null) return;

        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += adValue =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += error =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : " + error);
            LoadInterstitialAd();
        };
    }

#endregion


#region Rewarded Ad.

    public void LoadRewardedAd()
    {
        DestroyRewardedAd();

        Debug.Log("Loading rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(
            rewardedAdUnitId,
            adRequest,
            (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterRewardedEventHandler();
            }
        );
    }

    public void DestroyRewardedAd()
    {
        if (rewardedAd != null)
        {
            Debug.Log("Destroy rewarded ad.");
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    public void ShowRewardAd(Action<string, double> rewardedCallback)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            rewardedAd.Show(reward =>
            {
                rewardedCallback?.Invoke(reward.Type, reward.Amount);
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    private void RegisterRewardedEventHandler()
    {
        if (rewardedAd == null) return;

        // Raised when the ad is estimated to have earned money.
        rewardedAd.OnAdPaid += adValue =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        rewardedAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        rewardedAd.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        rewardedAd.OnAdFullScreenContentFailed += error =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
            LoadRewardedAd();
        };
    }

#endregion

}
