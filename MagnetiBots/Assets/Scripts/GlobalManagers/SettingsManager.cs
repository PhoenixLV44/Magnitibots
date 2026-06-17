using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    #region AudioSettings
    private float _sfxVolume;
    public float SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }

    private float _bgmVolume;
    public float BGMVolume { get { return _bgmVolume; } set { _bgmVolume = value; } }

    private float _masterVolume;
    public float MasterVolume { get { return _masterVolume; } set { _masterVolume = value; }  }
    #endregion

    #region UI References
    VisualElement root;
    Slider BGMVolumeSlider;
    Slider SFXVolumeSlider;
    Slider MasterVolumeSlider;
    #endregion

    private void Awake()
    {
        //find UI References
        root = GameObject.Find("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        
        BGMVolumeSlider = root.Q<Slider>("BGMVolumeSlider");
        SFXVolumeSlider = root.Q<Slider>("SFXVolumeSlider");
        MasterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");

        //register callbacks
        BGMVolumeSlider.RegisterValueChangedCallback(BGMCallback);
    }

    public void BGMCallback(ChangeEvent<float> evt)
    {
        Globals.Managers.Audio.UpdateVolumes(AudioManager.AudioSettings.Destination.BGM, evt.newValue);
    }
    public void SFXCallback(ChangeEvent<float> evt)
    {
        Globals.Managers.Audio.UpdateVolumes(AudioManager.AudioSettings.Destination.SFX, evt.newValue);
    }
    public void MasterCallback(ChangeEvent<float> evt)
    {
        Globals.Managers.Audio.UpdateVolumes(AudioManager.AudioSettings.Destination.Master, evt.newValue);
    }
}
