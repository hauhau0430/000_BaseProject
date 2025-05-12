using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Analytics;
using UniRx;

public class FirebaseManager
{
    // Document : https://firebase.google.com/docs/guides?authuser=0&_gl=1*89zquy*_ga*MjA4NzMyNDA2LjE3NDQ4MTMyMTE.*_ga_CW55HF8NVT*MTc0NDgxMzIxMC4xLjEuMTc0NDgxNDk5MC42MC4wLjA.
    
    private const string PLAYING_DATA_KEY = "FBPlayingData";

    private class PlayingData
    {
        public List<string> Days;

        public PlayingData()
        {
            Days = new List<string>();
        }
    }
    
    private FirebaseApp app = null;
    private PlayingData playingData = null;
    private DateTime startTime = new();

    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase Initialize Success");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void OnPlayStart()
    {
        startTime = DateTime.Now;
        SavePlayingData();
    }

    public void OnPlayEnd()
    {
        SavePlayingData();
        OnEventPlayingData();
    }


    public void OnEventLogin()
    {
        if (app == null) return;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
    }

    public void OnEventStageClear(int stageNum)
    {
        if (app == null) return;

        FirebaseAnalytics.LogEvent("stage_clear", new Parameter("stageNum", stageNum));
    }

    private void OnEventPlayingData()
    {
        if (app == null) return;

        string timeSpan = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");

        FirebaseAnalytics.LogEvent(
            "playing_data",
            new Parameter[]
            {
                new("Duration", playingData.Days.Count),
                new("TimeSpan", timeSpan),
            }
        );
    }

    private void GetPlayingData()
    {
        string json = PlayerPrefs.GetString(PLAYING_DATA_KEY, string.Empty);
        playingData = JsonUtility.FromJson<PlayingData>(json);
        playingData ??= new();
    }

    private void SavePlayingData()
    {
        GetPlayingData();
        string toDay = DateTime.Now.ToString("yyyy-MM-dd");

        if (!playingData.Days.Contains(toDay))
        {
            playingData.Days.Add(toDay);
            string json = JsonUtility.ToJson(playingData);
            PlayerPrefs.SetString(PLAYING_DATA_KEY, json);
            PlayerPrefs.Save();
        }
    }
}
