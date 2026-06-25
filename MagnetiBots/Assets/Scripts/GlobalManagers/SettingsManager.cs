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

    #region regular settings
    VisualElement root;
    Slider BGMVolumeSlider;
    Slider SFXVolumeSlider;
    Slider MasterVolumeSlider;
    Slider UIVolumeSlider;
    Slider MouseSensitivitySlider;
    #endregion

    #region pause settings
    VisualElement pause_root;
    Slider pause_BGMVolumeSlider;
    Slider pause_SFXVolumeSlider;
    Slider pause_MasterVolumeSlider;
    Slider pause_UIVolumeSlider;
    Slider pause_MouseSensitivitySlider;
    #endregion

    #endregion

    float _mouseSens;
    public float MouseSensitivity { get { return _mouseSens; } set { _mouseSens = value; } }

    private void Awake()
    {
        #region regular settings menu
        //find UI References
        root = GameObject.Find("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        
        BGMVolumeSlider = root.Q<Slider>("BGMVolumeSlider");
        SFXVolumeSlider = root.Q<Slider>("SFXVolumeSlider");
        MasterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");
        UIVolumeSlider = root.Q<Slider>("UIVolumeSlider");
        MouseSensitivitySlider = root.Q<Slider>("MouseSensitivitySlider");

        //register callbacks
        BGMVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.BGM);
        SFXVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.SFX);
        MasterVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.Master);
        UIVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.UI);

        BGMVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.BGM);
        SFXVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.SFX);
        MasterVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.Master);
        UIVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.UI);
        #endregion

        #region pause settings menu
        //find UI References
        pause_root = GameObject.Find("PauseMenu").GetComponent<UIDocument>().rootVisualElement;

        pause_BGMVolumeSlider = pause_root.Q<Slider>("BGMVolumeSlider");
        pause_SFXVolumeSlider = pause_root.Q<Slider>("SFXVolumeSlider");
        pause_MasterVolumeSlider = pause_root.Q<Slider>("MasterVolumeSlider");
        pause_UIVolumeSlider = pause_root.Q<Slider>("UIVolumeSlider");
        pause_MouseSensitivitySlider = pause_root.Q<Slider>("MouseSensitivitySlider");

        //register callbacks
        pause_BGMVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.BGM);
        pause_SFXVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.SFX);
        pause_MasterVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.Master);
        pause_UIVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.UI);

        pause_BGMVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.BGM);
        pause_SFXVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.SFX);
        pause_MasterVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.Master);
        pause_UIVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.UI);
        #endregion


    }
    public void LateAwake()
    {
        UpdateSettingsSliders();
        UpdatePauseSliders();
    }

    public void ChangeVolumeCallback(ChangeEvent<float> evt, Destination destination)
    {
        Globals.Managers.Audio.UpdateVolumes(destination, evt.newValue);
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
    private void UpdatePauseSliders()
    {
        float volumeHolder;

        //BGMVolume
        if (Globals.Managers.Saves.GetData<float>("BGMVolume", out volumeHolder))
        {
            BGMVolume = volumeHolder;
            pause_BGMVolumeSlider.value = BGMVolume;
        }
        else
        {
            BGMVolume = 1;
            Globals.Managers.Saves.AddData<float>("BGMVolume", BGMVolume);
            pause_BGMVolumeSlider.value = BGMVolume;
        }

        //SFXVolume
        if (Globals.Managers.Saves.GetData<float>("SFXVolume", out volumeHolder))
        {
            SFXVolume = volumeHolder;
            pause_SFXVolumeSlider.value = SFXVolume;
        }
        else
        {
            SFXVolume = 1;
            Globals.Managers.Saves.AddData<float>("SFXVolume", SFXVolume);
            pause_SFXVolumeSlider.value = SFXVolume;
        }

        //MasterVolume
        if (Globals.Managers.Saves.GetData<float>("MasterVolume", out volumeHolder))
        {
            MasterVolume = volumeHolder;
            pause_MasterVolumeSlider.value = MasterVolume;
            Debug.Log("success!");
        }
        else
        {
            MasterVolume = 1;
            Globals.Managers.Saves.AddData<float>("MasterVolume", MasterVolume);
            pause_MasterVolumeSlider.value = MasterVolume;
            Debug.Log("failure");
        }

        //UIVolume
        if (Globals.Managers.Saves.GetData<float>("UIVolume", out volumeHolder))
        {
            UIVolume = volumeHolder;
            pause_UIVolumeSlider.value = UIVolume;
        }
        else
        {
            UIVolume = 1;
            Globals.Managers.Saves.AddData<float>("UIVolume", UIVolume);
            pause_UIVolumeSlider.value = UIVolume;
        }
        Globals.Managers.Audio.FullVolumeUpdate();
    }
    private void UpdateSettingsSliders()
    {
        float volumeHolder;

        //BGMVolume
        if (Globals.Managers.Saves.GetData<float>("BGMVolume", out volumeHolder))
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
        if (Globals.Managers.Saves.GetData<float>("SFXVolume", out volumeHolder))
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
        if (Globals.Managers.Saves.GetData<float>("MasterVolume", out volumeHolder))
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
}
