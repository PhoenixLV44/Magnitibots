using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    
    private UIDocument ui;

    private VisualElement _pauseContainer;

    private Button _return;
    private Button _menu;
    private Button _settings;
    private Button _quit;

    private void Start()
    {
        ui = GetComponent<UIDocument>();

        _pauseContainer = ui.rootVisualElement.Q("PauseMenu");

        _return = ui.rootVisualElement.Q("Return") as Button;
        _return.RegisterCallback<ClickEvent>(OnClickReturn);

        _menu = ui.rootVisualElement.Q("MainMenu") as Button;
        _menu.RegisterCallback<ClickEvent>(OnClickMain);

        _settings = ui.rootVisualElement.Q("Settings") as Button;
        _settings.RegisterCallback<ClickEvent>(OnClickSettings);

        _quit = ui.rootVisualElement.Q("Quit") as Button;
        _quit.RegisterCallback<ClickEvent>(OnClickQuit);

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
    IEnumerator PausedMenu()
    {
        Debug.Log("pause");
        Globals.Managers.paused = true;
        ui.enabled = true;
        Time.timeScale = 0;
        InputSystem.actions.FindAction("MainMenu").Reset();
        while (Globals.Managers.paused)
        {
            if (InputSystem.actions.FindAction("MainMenu").triggered)
            {
                ui.enabled = false;
                Time.timeScale = 1;
                Globals.Managers.paused = false;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void OnClickReturn(ClickEvent click)
    {
        ui.enabled = false;
        Time.timeScale = 1;
        Globals.Managers.paused = false;
    }
    private void OnClickQuit(ClickEvent click)
    {
        Application.Quit();
    }
    private void OnClickSettings(ClickEvent click)
    {

    }
    private void OnClickMain(ClickEvent click)
    {

    }
}