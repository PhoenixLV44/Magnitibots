using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    
    private UIDocument ui;

    private VisualElement _pauseContainer;
    private VisualElement _settingsContainer;
    private VisualElement _blur;

    private VisualElement _controlsContainer;
    private ControlsCarousel _controlsCarousel;

    private Button _return;
    private Button _menu;
    private Button _settings;
    private Button _controls;

    private Button _settingsReturn;
    private Button _controlsReturn;

    private void Start()
    {
        Startup();
    }
    public void Startup()
    {
        ui = GetComponent<UIDocument>();

        _pauseContainer = ui.rootVisualElement.Q("PauseMenu");
        _settingsContainer = ui.rootVisualElement.Q("SettingsMenu");
        _controlsContainer = ui.rootVisualElement.Q("ControlsMenu");

        _blur = ui.rootVisualElement.Q("Blur");

        _controls = ui.rootVisualElement.Q("ControlsButton") as Button;
        _controls.RegisterCallback<ClickEvent>(OnClickControls);

        _controlsCarousel = gameObject.AddComponent<ControlsCarousel>();
        _controlsCarousel.container = _controlsContainer;
        _controlsCarousel.Startup();

        _controlsReturn = ui.rootVisualElement.Q("ControlsReturnButton") as Button;
        _controlsReturn.RegisterCallback<ClickEvent>(OnClickControlsReturn);

        _return = ui.rootVisualElement.Q("ReturnButton") as Button;
        _return.RegisterCallback<ClickEvent>(OnClickReturn);

        _menu = ui.rootVisualElement.Q("MainMenu") as Button;
        _menu.RegisterCallback<ClickEvent>(OnClickMain);

        _settings = ui.rootVisualElement.Q("SettingsButton") as Button;
        _settings.RegisterCallback<ClickEvent>(OnClickSettings);

        _settingsReturn = ui.rootVisualElement.Q("SettingsReturn") as Button;
        _settingsReturn.RegisterCallback<ClickEvent>(OnClickSettingsReturn);

        _pauseContainer.visible = false;

        Globals.Managers.Settings.PauseMenuSetup();
    }
    private void OnDisable()
    {
        _menu.UnregisterCallback<ClickEvent>(OnClickMain);
        _settings.UnregisterCallback<ClickEvent>(OnClickSettings);
        _return.UnregisterCallback<ClickEvent>(OnClickReturn);
        _settingsReturn.UnregisterCallback<ClickEvent>(OnClickSettingsReturn);

    }
    IEnumerator PausedMenu()
    {
        InputSystem.actions.actionMaps[0].Disable();
        InputSystem.actions.actionMaps[2].Disable();
        Debug.Log("pause");
        Globals.Managers.paused = true;
        _blur.visible = true;
        _pauseContainer.visible = true;
        Globals.Managers.Settings.UpdatePauseSliders();
        //Time.timeScale = 0.01f;
        InputSystem.actions.FindAction("MainMenu").Reset();
        while (Globals.Managers.paused)
        {
            if (InputSystem.actions.FindAction("MainMenu").IsPressed() && _pauseContainer.visible)
            {
                Debug.Log("return");
                _blur.visible = false;
                _pauseContainer.visible = false;
                _pauseContainer.SendToBack();
                Globals.Managers.Settings.EnableHUD();
                Time.timeScale = 1;
                Globals.Managers.paused = false;
                InputSystem.actions.actionMaps[0].Enable();
                InputSystem.actions.actionMaps[2].Enable();
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void OnClickReturn(ClickEvent click)
    {
        Debug.Log("return");
        _blur.visible = false;
        _pauseContainer.visible = false;
        _pauseContainer.SendToBack();
        Globals.Managers.Settings.EnableHUD();
        Time.timeScale = 1;
        Globals.Managers.paused = false;
        InputSystem.actions.actionMaps[0].Enable();
        InputSystem.actions.actionMaps[2].Enable();
    }
    private void OnClickSettings(ClickEvent click)
    {
        
        _pauseContainer.visible = false;
        _settingsContainer.visible = true;
    }
    private void OnClickControls(ClickEvent click)
    {
        _pauseContainer.visible = false;
        _controlsContainer.visible = true;
        _controlsCarousel.Ready();
    }
    private void OnClickMain(ClickEvent click)
    {

        _pauseContainer.visible = false;
        _blur.visible = false;
        Time.timeScale = 1;
        Globals.Managers.paused = false;
        InputSystem.actions.actionMaps[0].Enable();
        InputSystem.actions.actionMaps[2].Enable();
        SceneManager.LoadScene(0);
    }
    private void OnClickSettingsReturn(ClickEvent click)
    {
        _pauseContainer.visible = true;
        _settingsContainer.visible = false;
    }
    private void OnClickControlsReturn(ClickEvent click)
    {
        _pauseContainer.visible = true;
        _controlsContainer.visible = false;
        _controlsCarousel.UnReady();
    }
    public void PauseMe()
    {
        StartCoroutine(PausedMenu());
    }
}