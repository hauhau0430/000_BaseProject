using System;
using UnityEngine;

[RequireComponent(typeof(GoogleAdMobManager), typeof(UnityAdsManager))]
public class AdvertisementManager : MonoBehaviour
{
    private const float repeatRate = 3600f;

    [SerializeField]
    private bool isUseBannerAd = false;
    [SerializeField]
    private bool isUseInterstitialAd = false;
    [SerializeField]
    private bool isUseRewardedAd = false;

    public bool CanShowBannerAd => googleAdMobManager.CanShowBannerAd;
    public bool CanShowInterstitialAd => googleAdMobManager.CanShowInterstitialAd;
    public bool CanShowRewardedAd => googleAdMobManager.CanShowRewardedAd;

    private GoogleAdMobManager googleAdMobManager = null;
    private UnityAdsManager unityAdsManager = null;


    public void Initialize()
    {
        if (googleAdMobManager == null)
        {
            googleAdMobManager = GetComponent<GoogleAdMobManager>();
        }

        if (unityAdsManager == null)
        {
            unityAdsManager = GetComponent<UnityAdsManager>();
        }

        CancelInvoke();

        googleAdMobManager.Initialize(() =>
        {
            if (isUseBannerAd)
            {
                googleAdMobManager.LoadBannerAd();
            }

            InvokeRepeating(nameof(RepeatLoadAd), 0f, repeatRate);
        });

        unityAdsManager.Initialize();
    }

    public void ShowInterstitialAd()
    {
        if (googleAdMobManager.CanShowInterstitialAd)
        {
            googleAdMobManager.ShowInterstitialAd();
        }
        else
        {
            unityAdsManager.LoadInterstitialAd(unityAdsManager.ShowInterstitialAd);
        }
    }

    public void ShowRewardedAd(Action<string, double> rewardedCallback)
    {
        if (googleAdMobManager.CanShowRewardedAd)
        {
            googleAdMobManager.ShowRewardAd(rewardedCallback);
        }
        else
        {
            unityAdsManager.LoadRewardedAd(() =>
            {
                unityAdsManager.ShowRewardedAd(rewardedCallback);
            });
        }
    }

    private void RepeatLoadAd()
    {
        if (isUseInterstitialAd)
        {
            googleAdMobManager.LoadInterstitialAd();
        }

        if (isUseRewardedAd)
        {
            googleAdMobManager.LoadRewardedAd();
        }
    }

    void OnDestroy()
    {
        CancelInvoke();

        if (googleAdMobManager != null)
        {
            googleAdMobManager.DestroyBanner();
            googleAdMobManager.DestroyInterstitialAd();
            googleAdMobManager.DestroyRewardedAd();
        }
    }
}
