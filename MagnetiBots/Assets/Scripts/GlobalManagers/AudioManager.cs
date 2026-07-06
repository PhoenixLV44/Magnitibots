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

    float maxVolume = 0;
    float minVolume = -40;

    public static class AudioSettings 
    {
        public enum Destination
        {
            BGM,
            SFX,
            Master,
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
                Debug.Log(volume);
                Globals.Managers.Settings.MasterVolume = value;
                if (audioMixer.SetFloat("Master_Volume", volume))
                {
                    Debug.Log("yippee!");
                }
                ;
                break;
            case AudioSettings.Destination.SFX:
                Globals.Managers.Settings.SFXVolume = value;
                audioMixer.SetFloat("SFX_Volume", volume);
                break;
            case AudioSettings.Destination.BGM:
                Globals.Managers.Settings.BGMVolume = value;
                audioMixer.SetFloat("BGM_Volume", volume);
                break;
            default: break;
        }
    }
    public void FullVolumeUpdate()
    {
        Debug.Log(Globals.Managers.Settings.MasterVolume);
        UpdateVolumes(AudioSettings.Destination.BGM, Globals.Managers.Settings.BGMVolume);
        UpdateVolumes(AudioSettings.Destination.Master, Globals.Managers.Settings.MasterVolume);
        UpdateVolumes(AudioSettings.Destination.SFX, Globals.Managers.Settings.SFXVolume);
    }
    public void UpdateBGM(string clipName)
    {
        bgmSource.clip = data.bgm[clipName];
        bgmSource.Play();
    }
    public void PlaySFX(string clipName)
    {
        sfxSource.clip = data.sfx[clipName];
        sfxSource.Play();
    }
    public void PlaySFXHere(string clipName, Transform transform)
    {
        AudioSource.PlayClipAtPoint(data.sfx[clipName], transform.position, Globals.Managers.Settings.SFXVolume);
    }
    public void PlaySFXRandom(string clipName, Transform transform, int max, float volumeModifier)
    {
        string newName = String.Concat(clipName, UnityEngine.Random.Range(1, max+1));
        AudioSource.PlayClipAtPoint(data.sfx[newName], transform.position, volumeModifier*Globals.Managers.Settings.SFXVolume);
    }
}

/* Sounds Taken from Pixabay:
 * freesound_community
 * floraphonic
 * Mori_sound
 * 
 * BGM By;
 * Jean-Paul-V
 */
