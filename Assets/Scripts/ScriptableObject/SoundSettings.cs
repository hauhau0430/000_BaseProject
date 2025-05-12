using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SoundData
{
    [SerializeField][Header("ID=0は設定不可")]
    private int soundId;
    public int SoundId => soundId;
    [SerializeField]
    private AudioClip clip;
    public AudioClip Clip => clip;
}

[CreateAssetMenu(menuName = "ScriptableObject/Sound Data", fileName = "SoundSettings")]
public class SoundSettings : SingletonScriptableObject<SoundSettings>
{
    [SerializeField]
    private List<SoundData> bgmDataList;
    [SerializeField]
    private List<SoundData> seDataList;

    public AudioClip GetBgmClip(int id)
    {
        var data = bgmDataList.Where(data => data.SoundId == id).FirstOrDefault();

        if (data == null)
        {
            Debug.Log($"id {id} is invalid");
        }

        return data?.Clip;
    }

    public AudioClip GetSeClip(int id)
    {
        var data = seDataList.Where(data => data.SoundId == id).FirstOrDefault();

        if (data == null)
        {
            Debug.Log($"id {id} is invalid");
        }

        return data?.Clip;
    }
}
