using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
    Slider MouseSensitivitySlider;
    #endregion

    float _mouseSens;
    public float MouseSensitivity { get { return _mouseSens; } set { _mouseSens = value; } }

    private void Awake()
    {
        //find UI References
        root = GameObject.Find("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        
        BGMVolumeSlider = root.Q<Slider>("BGMVolumeSlider");
        SFXVolumeSlider = root.Q<Slider>("SFXVolumeSlider");
        MasterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");
        MouseSensitivitySlider = root.Q<Slider>("MouseSensitivitySlider");

        //set current values


        //register callbacks
        BGMVolumeSlider.RegisterValueChangedCallback(BGMCallback);
        SFXVolumeSlider.RegisterValueChangedCallback(SFXCallback);
        MasterVolumeSlider.RegisterValueChangedCallback(MasterCallback);

    }
    public void LateAwake()
    {
        float volumeHolder;

        if(Globals.Managers.Saves.GetData<float>("BGMVolume", out volumeHolder))
        {
            BGMVolume = volumeHolder;
            BGMVolumeSlider.value = BGMVolume;
        }
        else
        {
            BGMVolume = 100;
            Globals.Managers.Saves.AddData("BGMVolume", BGMVolume);
        }

        if(Globals.Managers.Saves.GetData<float>("SFXVolume", out volumeHolder))
        {
            SFXVolume = volumeHolder;
            SFXVolumeSlider.value = SFXVolume;
        }
        else
        {
            SFXVolume = 100;
            Globals.Managers.Saves.AddData("SFXVolume", SFXVolume);
        }

        if(Globals.Managers.Saves.GetData<float>("MasterVolume", out volumeHolder))
        {
            MasterVolume = volumeHolder;
            MasterVolumeSlider.value = MasterVolume;
        }
        else
        {
            SFXVolume = 100;
            Globals.Managers.Saves.AddData("MasterVolume", MasterVolume);
        }

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
    public void SensitivityCallback(ChangeEvent<float> evt)
    {
        MouseSensitivity = evt.newValue;
    }
}
