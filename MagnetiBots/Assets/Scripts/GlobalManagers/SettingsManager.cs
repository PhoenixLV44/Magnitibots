using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static AudioManager.AudioSettings;

public class SettingsManager : MonoBehaviour
{
    #region AudioSettings
    private float _sfxVolume;
    public float SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }

    private float _bgmVolume;
    public float BGMVolume { get { return _bgmVolume; } set { _bgmVolume = value; } }

    private float _masterVolume;
    public float MasterVolume { get { return _masterVolume; } set { _masterVolume = value; }  }

    private float _uiVolume;
    public float UIVolume { get { return _uiVolume; } set { _uiVolume = value; } }
    #endregion

    #region UI References
    VisualElement root;
    Slider BGMVolumeSlider;
    Slider SFXVolumeSlider;
    Slider MasterVolumeSlider;
    Slider UIVolumeSlider;
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
        UIVolumeSlider = root.Q<Slider>("UIVolumeSlider");
        MouseSensitivitySlider = root.Q<Slider>("MouseSensitivitySlider");

        //set current values


        //register callbacks
        BGMVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.BGM);
        SFXVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.SFX);
        MasterVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.Master);
        UIVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.UI);

        BGMVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.BGM);
        SFXVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.SFX);
        MasterVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.Master);
        UIVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.UI);


    }
    public void LateAwake()
    {
        float volumeHolder;

        //BGMVolume
        if(Globals.Managers.Saves.GetData<float>("BGMVolume", out volumeHolder))
        {
            BGMVolume = volumeHolder;
            BGMVolumeSlider.value = BGMVolume;
        }
        else
        {
            BGMVolume = 1;
            Globals.Managers.Saves.AddData<float>("BGMVolume", BGMVolume);
            BGMVolumeSlider.value = BGMVolume;
        }

        //SFXVolume
        if(Globals.Managers.Saves.GetData<float>("SFXVolume", out volumeHolder))
        {
            SFXVolume = volumeHolder;
            SFXVolumeSlider.value = SFXVolume;
        }
        else
        {
            SFXVolume = 1;
            Globals.Managers.Saves.AddData<float>("SFXVolume", SFXVolume);
            SFXVolumeSlider.value = SFXVolume;
        }

        //MasterVolume
        if(Globals.Managers.Saves.GetData<float>("MasterVolume", out volumeHolder))
        {
            MasterVolume = volumeHolder;
            MasterVolumeSlider.value = MasterVolume;
            Debug.Log("success!");
        }
        else
        {
            MasterVolume = 1;
            Globals.Managers.Saves.AddData<float>("MasterVolume", MasterVolume);
            MasterVolumeSlider.value = MasterVolume;
            Debug.Log("failure");
        }

        //UIVolume
        if (Globals.Managers.Saves.GetData<float>("UIVolume", out volumeHolder))
        {
            UIVolume = volumeHolder;
            UIVolumeSlider.value = UIVolume;
        }
        else
        {
            UIVolume = 1;
            Globals.Managers.Saves.AddData<float>("UIVolume", UIVolume);
            UIVolumeSlider.value = UIVolume;
        }
        Globals.Managers.Audio.FullVolumeUpdate();
    }

    public void ChangeVolumeCallback(ChangeEvent<float> evt, Destination destination)
    {
        Globals.Managers.Audio.UpdateVolumes(AudioManager.AudioSettings.Destination.BGM, evt.newValue);
        Debug.Log("bgm");
    }
    public void SensitivityCallback(ChangeEvent<float> evt)
    {
        MouseSensitivity = evt.newValue;
    }
    public void SaveVolumeCallback(FocusOutEvent evt, AudioManager.AudioSettings.Destination dinger)
    {
        switch (dinger)
        {
            case Destination.Master:
                Globals.Managers.Saves.AddData<float>("MasterVolume", MasterVolume);
                break;
            case Destination.SFX:
                Globals.Managers.Saves.AddData<float>("SFXVolume", SFXVolume);
                break;
            case Destination.BGM:
                Globals.Managers.Saves.AddData<float>("BGMVolume", BGMVolume);
                break;
            case Destination.UI:
                Globals.Managers.Saves.AddData<float>("UIVolume", UIVolume);
                break;
            default: break;
        }
    }
}
