using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
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

    GameObject _pauseMenu;

    GameObject _hud;
    VisualElement _hudRoot;
    VisualElement _hudblur;

    #region regular settings
    VisualElement root;
    Slider BGMVolumeSlider;
    Slider SFXVolumeSlider;
    Slider MasterVolumeSlider;
    Slider MouseSensitivitySlider;
    #endregion

    #region pause settings
    VisualElement pause_root;
    VisualElement pause_blur;
    VisualElement pause_settingsMenu;
    Slider pause_BGMVolumeSlider;
    Slider pause_SFXVolumeSlider;
    Slider pause_MasterVolumeSlider;
    Slider pause_MouseSensitivitySlider;
    #endregion

    #region Transitions
    UIDocument sceneTransition;
    public bool waitMenu = false;
    #endregion

    #endregion

    float _mouseSens;
    public float MouseSensitivity { get { return _mouseSens; } set { _mouseSens = value; } }

    public void SettingsMenuSetup()
    {
        #region regular settings menu
        //find UI References
        root = GameObject.Find("MainMenu").GetComponent<UIDocument>().rootVisualElement;

        BGMVolumeSlider = root.Q<Slider>("BGMVolumeSlider");
        SFXVolumeSlider = root.Q<Slider>("SFXVolumeSlider");
        MasterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");
        MouseSensitivitySlider = root.Q<Slider>("MouseSensitivitySlider");

        //register callbacks
        BGMVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.BGM);
        SFXVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.SFX);
        MasterVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.Master);
        MouseSensitivitySlider.RegisterCallback<ChangeEvent<float>>(SensitivityCallback);

        BGMVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.BGM);
        SFXVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.SFX);
        MasterVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.Master);
        MouseSensitivitySlider.RegisterCallback<FocusOutEvent>(SaveSensitivityCallback);
        #endregion
        UpdateSettingsSliders();
    }
    public void SettingsMenuUnregister()
    {
        BGMVolumeSlider.UnregisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback);
        SFXVolumeSlider.UnregisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback);
        MasterVolumeSlider.UnregisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback);
        MouseSensitivitySlider.UnregisterCallback<ChangeEvent<float>>(SensitivityCallback);

        BGMVolumeSlider.UnregisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback);
        SFXVolumeSlider.UnregisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback);
        MasterVolumeSlider.UnregisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback);
        MouseSensitivitySlider.UnregisterCallback<FocusOutEvent>(SaveSensitivityCallback);
    }
    public void PauseMenuSetup()
    {
        pause_root = GameObject.Find("PauseMenu").GetComponent<UIDocument>().rootVisualElement;
        pause_settingsMenu = pause_root.Q("SettingsMenu");
        pause_blur = pause_root.Q<VisualElement>("Blur");

        pause_BGMVolumeSlider = pause_settingsMenu.Q<Slider>("BGMVolumeSlider");
        pause_SFXVolumeSlider = pause_settingsMenu.Q<Slider>("SFXVolumeSlider");
        pause_MasterVolumeSlider = pause_settingsMenu.Q<Slider>("MasterVolumeSlider");
        pause_MouseSensitivitySlider = pause_settingsMenu.Q<Slider>("MouseSensitivitySlider");

        //register callbacks
        pause_BGMVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.BGM);
        pause_SFXVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.SFX);
        pause_MasterVolumeSlider.RegisterCallback<ChangeEvent<float>, Destination>(ChangeVolumeCallback, Destination.Master);
        pause_MouseSensitivitySlider.RegisterCallback<ChangeEvent<float>>(SensitivityCallback);

        pause_BGMVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.BGM);
        pause_SFXVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.SFX);
        pause_MasterVolumeSlider.RegisterCallback<FocusOutEvent, Destination>(SaveVolumeCallback, Destination.Master);
        pause_MouseSensitivitySlider.RegisterCallback<FocusOutEvent>(SaveSensitivityCallback);
        UpdatePauseSliders();
    }
    private void Awake()
    {
        
        #region pause settings menu
        //find UI References

        #endregion

        _hud = GameObject.Find("HUD");

        _hudRoot = _hud.GetComponent<UIDocument>().rootVisualElement;
        _hudblur = _hudRoot.Q<VisualElement>("Unlocks").Q<VisualElement>("Blur");

        _pauseMenu = GameObject.Find("PauseMenu");

        sceneTransition = GameObject.Find("SceneTransition").GetComponent<UIDocument>();
        
    }
    private void Update()
    {
        if (_pauseMenu != null)
        {
            if (InputSystem.actions.FindAction("MainMenu").triggered && SceneManager.GetActiveScene().buildIndex != 0)
            {
                if (!Globals.Managers.paused)
                {
                    Globals.Managers.Settings.DisableHUD();
                    _pauseMenu.GetComponent<PauseMenu>().PauseMe();
                }
            }
        }
    }
    public void LateAwake()
    {
        sceneTransition.rootVisualElement.visible = false;
        SettingsMenuSetup();
        PauseMenuSetup();
    }
    private void Start()
    {
        Globals.Managers.Audio.FullVolumeUpdate();
    }
    public void ChangeVolumeCallback(ChangeEvent<float> evt, Destination destination)
    {
        Globals.Managers.Audio.UpdateVolumes(destination, evt.newValue);
    }
    public void SensitivityCallback(ChangeEvent<float> evt)
    {
        MouseSensitivity = evt.newValue;
    }
    public void SaveSensitivityCallback(FocusOutEvent evt)
    {
        Globals.Managers.Saves.AddData<float>("MouseSensitivity", MouseSensitivity);
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
            default: break;
        }
    }
    public void UpdatePauseSliders()
    {
        float volumeHolder;

        #region Volumes
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
        }
        else
        {
            MasterVolume = 1;
            Globals.Managers.Saves.AddData<float>("MasterVolume", MasterVolume);
            pause_MasterVolumeSlider.value = MasterVolume;
        }

        Globals.Managers.Audio.FullVolumeUpdate();
        #endregion

        //MouseSensitivity
        if (Globals.Managers.Saves.GetData<float>("MouseSensitivity", out volumeHolder))
        {
            MouseSensitivity = volumeHolder;
            pause_MouseSensitivitySlider.value = MouseSensitivity;
        }
        else
        {
            UIVolume = 1;
            Globals.Managers.Saves.AddData<float>("MouseSensitivity", MouseSensitivity);
            pause_MouseSensitivitySlider.value = MouseSensitivity;
        }
    }
    private void UpdateSettingsSliders()
    {
        float volumeHolder;

        #region Volumes
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
        }
        else
        {
            MasterVolume = 1;
            Globals.Managers.Saves.AddData<float>("MasterVolume", MasterVolume);
            MasterVolumeSlider.value = MasterVolume;
        }

        Globals.Managers.Audio.FullVolumeUpdate();
        #endregion

        //MouseSensitivity
        if (Globals.Managers.Saves.GetData<float>("MouseSensitivity", out volumeHolder))
        {
            MouseSensitivity = volumeHolder;
            MouseSensitivitySlider.value = MouseSensitivity;
        }
        else
        {
            MouseSensitivity = 1;
            Globals.Managers.Saves.AddData<float>("MouseSensitivity", MouseSensitivity);
            MouseSensitivitySlider.value = MouseSensitivity;
        }
    }
    public void EnableHUD()
    {
        _hud.SetActive(true);
        _hud.GetComponent<HUDGUI>().Startup();
    }
    public void UpdateHUD(string ability = "nada")
    {
        _hud.GetComponent<HUDGUI>().UpdateGUI(ability);
    }
    public void DisableHUD()
    {
        _hud.SetActive(false);
    }
    public void EnablePause()
    {
        _pauseMenu.SetActive(true);
        _pauseMenu.GetComponent<PauseMenu>().Startup();
    }
    public void DisablePause()
    {
        _pauseMenu.SetActive(false);
    }
    public void UnlockPopup(string ability)
    {
        _hud.GetComponent<HUDGUI>().UnlockPopup(ability);
    }
    public void TransitionScene()
    {
        StartCoroutine(Fader("Load"));
    }
    public void TransitionCredits()
    {
        StartCoroutine(Fader("Credits"));
    }
    public void FadeAway(string action = "", int index = 1)
    {
        StartCoroutine(Fader(action, index));
    }
    private IEnumerator Fader(string action = "", int index = 1)
    {
        sceneTransition.rootVisualElement.visible = true;
        sceneTransition.rootVisualElement.BringToFront();
        sceneTransition.rootVisualElement.Q("Blackout").AddToClassList("transitionOn");
        yield return new WaitForSecondsRealtime(1);
        switch (action)
        {
            case "Load":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
                yield return new WaitForSecondsRealtime(2);
                EnableHUD();
                EnablePause();
                break;
            case "Credits":
                Globals.Managers.Settings.DisableHUD();
                Globals.Managers.Settings.DisablePause();
                waitMenu = true;
                SceneManager.LoadScene(0);
                sceneTransition.rootVisualElement.BringToFront();
                yield return new WaitForSecondsRealtime(2);
                sceneTransition.rootVisualElement.BringToFront();
                SettingsMenuSetup();
                root.Q("MainMenu").visible = true;
                GameObject.Find("MainMenu").GetComponent<MainMenu>().GoToCredits();
                break;
            default:
                break;
        }
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {

        sceneTransition.rootVisualElement.Q("Blackout").RemoveFromClassList("transitionOn");
        yield return new WaitForSecondsRealtime(1);

        sceneTransition.rootVisualElement.SendToBack();
        sceneTransition.rootVisualElement.visible = false;
        Globals.Managers.paused = false;
    }
}
