using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SoundManager : DontDestroyObjectBase<SoundManager>
    {
        [SerializeField]
        private AudioSource[] bgmAudioSourceArray = null;
        [SerializeField]
        private AudioSource[] seAudioSourceArray = null;

        private readonly Dictionary<int, int> playBgmDic = new();

        public void PlayBgm(int bgmId , int sourceIndex = 0)
        {
            Debug.Log($"PlayBgmId : {bgmId}");

            if (playBgmDic.TryGetValue(sourceIndex, out var currentBgmId) && currentBgmId != bgmId)
            {
                return;
            }

            bgmAudioSourceArray[sourceIndex].Stop();
            bgmAudioSourceArray[sourceIndex].clip = SoundSettings.Instance.GetBgmClip(bgmId);
            bgmAudioSourceArray[sourceIndex].Play();
            playBgmDic[sourceIndex] = bgmId;
        }

        public void StopBgm(int sourceIndex = 0)
        {
            bgmAudioSourceArray[sourceIndex].Stop();
            playBgmDic[sourceIndex] = 0;
        }

        public void PlaySe(int seId, int sourceIndex = 0)
        {
            seAudioSourceArray[sourceIndex].Stop();
            seAudioSourceArray[sourceIndex].PlayOneShot(SoundSettings.Instance.GetSeClip(seId));
        }
    }
}
