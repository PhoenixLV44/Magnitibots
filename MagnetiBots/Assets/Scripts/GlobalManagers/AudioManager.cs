using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{

    #region Audio Data
    [System.Serializable]
    public class AudioDataObject
    {
        public Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> bgm = new Dictionary<string, AudioClip>();
    }

    AudioDataObject data;
    #endregion

    AudioSource sfxSource;
    AudioSource bgmSource;

    AudioMixer audioMixer;

    public static class AudioSettings 
    {
        public enum Destination
        {
            BGM,
            SFX,
            Master
        }
    }

    private void Awake()
    {
        #region Updating Missing Audio
        //load audio resources
        AudioClip[] SFXLoad = Resources.LoadAll<AudioClip>("Audio/SFX");
        AudioClip[] BGMLoad = Resources.LoadAll<AudioClip>("Audio/BGM");

        //parse audio resources
        for (int i = 0; i < SFXLoad.Length; i++) 
        { 
            if (SFXLoad[i] != null)
            {
                if (!data.sfx.ContainsValue(SFXLoad[i]))
                {
                    data.sfx.Add(SFXLoad[i].name, SFXLoad[i]);
                }
            }
        }
        for (int i = 0; i < BGMLoad.Length; i++)
        {
            if (BGMLoad[i] != null)
            {
                if (!data.sfx.ContainsValue(BGMLoad[i]))
                {
                    data.sfx.Add(BGMLoad[i].name, BGMLoad[i]);
                }
            }
        }
        #endregion

        #region CreateAudioPlayers
        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();

        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        #endregion
    }
    /// <summary>
    /// Event Handler for changing the volume settings.
    /// </summary>
    public void UpdateVolumes(AudioSettings.Destination destination, float value)
    {
        switch (destination)
        {
            case AudioSettings.Destination.Master:
                audioMixer.SetFloat("Master_Volume", value);
                break;
            case AudioSettings.Destination.SFX:
                audioMixer.SetFloat("SFX_Volume", value);
                break;
            case AudioSettings.Destination.BGM:
                audioMixer.SetFloat("BGM_Volume", value);
                break;
            default: break;
        }
    }
}
