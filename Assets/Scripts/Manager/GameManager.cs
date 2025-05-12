using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DontDestroyObjectBase<GameManager>
{
    private readonly FirebaseManager firebaseManager = new();
    private readonly FacebookManager facebookManager = new();
    private AdvertisementManager AdManager;

    void Start()
    {
        Application.targetFrameRate = 30;
        facebookManager.Initialize();
        // firebaseManager.Initialize();
        // firebaseManager.OnEventLogin();
        AdManager = GetComponent<AdvertisementManager>();
    }

    
    void Update()
    {
        
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // firebaseManager.OnPlayStart();
        }
        else
        {
            // firebaseManager.OnPlayEnd();
        }
    }

    public void StageClear(int stageNum)
    {
        // firebaseManager.OnEventStageClear(stageNum);
    }

    public void InitializeAd()
    {
        AdManager.Initialize();
    }

    public void ShowInterstitialAd()
    {
        AdManager.ShowInterstitialAd();
    }

    public void ShowRewardedAd(Action<string, double> rewardedCallback)
    {
        AdManager.ShowRewardedAd(rewardedCallback);
    }
}
