using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{

    #region Audio Data
    [System.Serializable]
    public class AudioDataObject
    {
        public SerializedDictionary<string, AudioClip> sfx = new SerializedDictionary<string, AudioClip>();
        public SerializedDictionary<string, AudioClip> bgm = new SerializedDictionary<string, AudioClip>();
    }

    [SerializeField] AudioDataObject data;
    #endregion

    AudioSource sfxSource;
    AudioSource bgmSource;

    AudioMixer audioMixer;

    float maxVolume = -20;
    float minVolume = -80;

    public static class AudioSettings 
    {
        public enum Destination
        {
            BGM,
            SFX,
            Master,
            UI
        }
    }

    private void Awake()
    {
        #region Updating Missing Audio
        //load audio resources
        AudioClip[] SFXLoad = Resources.LoadAll<AudioClip>("Audio/SFX");
        AudioClip[] BGMLoad = Resources.LoadAll<AudioClip>("Audio/BGM");

        Debug.Log(BGMLoad[0].name);

        data = new AudioDataObject();

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
                if (!data.bgm.ContainsValue(BGMLoad[i]))
                {
                    data.bgm.Add(BGMLoad[i].name, BGMLoad[i]);
                }
            }
        }
        #endregion

        #region CreateAudioPlayers
        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        bgmSource.loop = true;




        #endregion
    }
    public void LateAwake()
    {

    }
    /// <summary>
    /// Event Handler for changing the volume settings.
    /// </summary>
    public void UpdateVolumes(AudioSettings.Destination destination, float value)
    {
        float volume = Mathf.Lerp(minVolume, maxVolume, value);
        switch (destination)
        {
            case AudioSettings.Destination.Master:
                audioMixer.SetFloat("Master_Volume", volume);
                break;
            case AudioSettings.Destination.SFX:
                audioMixer.SetFloat("SFX_Volume", volume);
                break;
            case AudioSettings.Destination.BGM:
                audioMixer.SetFloat("BGM_Volume", volume);
                break;
            case AudioSettings.Destination.UI:
                audioMixer.SetFloat("UI_Volume", volume);
                break;
            default: break;
        }
    }
    public void FullVolumeUpdate()
    {
        Debug.Log(Globals.Managers.Settings.MasterVolume);
        audioMixer.SetFloat("Master_Volume", Globals.Managers.Settings.MasterVolume);
        audioMixer.SetFloat("BGM_Volume", Globals.Managers.Settings.BGMVolume);
        audioMixer.SetFloat("SFX_Volume", Globals.Managers.Settings.SFXVolume);
        audioMixer.SetFloat("UI_Volume", Globals.Managers.Settings.UIVolume);
    }
    public void UpdateBGM(string clipName)
    {
        bgmSource.clip = data.bgm[clipName];
        bgmSource.Play();
    }
}
