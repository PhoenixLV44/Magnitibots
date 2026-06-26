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

    private Button _return;
    private Button _menu;
    private Button _settings;
    private Button _quit;

    private Button _settingsReturn;

    private void Start()
    {
        ui = GetComponent<UIDocument>();

        _pauseContainer = ui.rootVisualElement.Q("PauseMenu");
        _settingsContainer = ui.rootVisualElement.Q("SettingsMenu");

        _return = ui.rootVisualElement.Q("ReturnButton") as Button;
        _return.RegisterCallback<ClickEvent>(OnClickReturn);

        _menu = ui.rootVisualElement.Q("MainMenu") as Button;
        _menu.RegisterCallback<ClickEvent>(OnClickMain);

        _settings = ui.rootVisualElement.Q("SettingsButton") as Button;
        _settings.RegisterCallback<ClickEvent>(OnClickSettings);

        _quit = ui.rootVisualElement.Q("QuitButton") as Button;
        _quit.RegisterCallback<ClickEvent>(OnClickQuit);

        _settingsReturn = ui.rootVisualElement.Q("SettingsReturn") as Button;
        _settingsReturn.RegisterCallback<ClickEvent>(OnClickSettingsReturn);

        _pauseContainer.visible = false;
    }
    private void Update()
    {
        if (ui != null)
        {
            if (InputSystem.actions.FindAction("MainMenu").triggered)
            {
                if (!Globals.Managers.paused)
                {
                    StartCoroutine(PausedMenu());
                }
            }
        }
    }
    private void OnDisable()
    {
        _menu.UnregisterCallback<ClickEvent>(OnClickMain);
        _quit.UnregisterCallback<ClickEvent>(OnClickQuit);
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
        _pauseContainer.visible = true;
        //Time.timeScale = 0.01f;
        InputSystem.actions.FindAction("MainMenu").Reset();
        while (Globals.Managers.paused)
        {
            if (InputSystem.actions.FindAction("MainMenu").IsPressed())
            {
                _pauseContainer.visible = false;
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
        _pauseContainer.visible = false;
        Time.timeScale = 1;
        Globals.Managers.paused = false;
        InputSystem.actions.actionMaps[0].Enable();
        InputSystem.actions.actionMaps[2].Enable();
    }
    private void OnClickQuit(ClickEvent click)
    {
        Application.Quit();
    }
    private void OnClickSettings(ClickEvent click)
    {
        _pauseContainer.visible = false;
        _settingsContainer.visible = true;
    }
    private void OnClickMain(ClickEvent click)
    {
        SceneManager.LoadScene(0);
    }
    private void OnClickSettingsReturn(ClickEvent click)
    {
        _pauseContainer.visible = true;
        _settingsContainer.visible = false;
    }
}