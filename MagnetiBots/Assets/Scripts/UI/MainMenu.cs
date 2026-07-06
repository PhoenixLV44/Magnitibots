using System;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument _mainDocument;

    private VisualElement _mainContainer;

    private Button _startButton;
    private Button _settingsButton;
    private Button _quitButton;
    private Button _controlsButton;
    private Button _creditsButton;

    private VisualElement _settingsContainer;
    private Button _returnSettingsButton;

    private VisualElement _controlsContainer;
    private ControlsCarousel _controlsCarousel;
    private Button _controlsReturnButton;

    private VisualElement _creditsContainer;
    public List<string> sources;
    private Button _creditsReturnButton;

    private void Awake()
    {
        
        _mainDocument = GetComponent<UIDocument>();

        #region MainMenu Container and Buttons

        _mainContainer = _mainDocument.rootVisualElement.Q("MainMenu");

        _startButton = _mainDocument.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnClickStart);

        _controlsButton = _mainDocument.rootVisualElement.Q("ControlsButton") as Button;
        _controlsButton.RegisterCallback<ClickEvent>(OnClickControls);

        _settingsButton = _mainDocument.rootVisualElement.Q("SettingsButton") as Button;
        _settingsButton.RegisterCallback<ClickEvent>(OnClickSettings);

        _creditsButton = _mainDocument.rootVisualElement.Q("CreditsButton") as Button;
        _creditsButton.RegisterCallback<ClickEvent>(OnClickCredits);

        _quitButton = _mainDocument.rootVisualElement.Q("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnClickQuit);
        #endregion

        #region Settings Container and Buttons
        _settingsContainer = _mainDocument.rootVisualElement.Q("SettingsMenu");

        _returnSettingsButton = _mainDocument.rootVisualElement.Q("ReturnButton") as Button;
        _returnSettingsButton.RegisterCallback<ClickEvent>(OnClickReturnSettings);

        _settingsContainer.visible = false;
        #endregion

        #region Controls Container and Button
        _controlsContainer = _mainDocument.rootVisualElement.Q("ControlsMenu");
        _controlsCarousel = gameObject.AddComponent<ControlsCarousel>();
        _controlsCarousel.container = _controlsContainer;
        _controlsCarousel.Startup();


        _controlsReturnButton = _mainDocument.rootVisualElement.Q("ControlsReturnButton") as Button;
        _controlsReturnButton.RegisterCallback<ClickEvent>(OnClickReturnControls);
        #endregion

        #region Credits Container and Buttons
        _creditsContainer = _mainDocument.rootVisualElement.Q("CreditsMenu");
        _creditsReturnButton = _mainDocument.rootVisualElement.Q("CreditsReturnButton") as Button;
        _creditsReturnButton.RegisterCallback<ClickEvent>(OnClickReturnCredits);
        #endregion

        _mainContainer.visible = true;
        _mainContainer.BringToFront();
        _controlsContainer.visible = false;

    }
    private void Start()
    {
        Globals.Managers.Settings.DisableHUD();
        Globals.Managers.Audio.UpdateBGM("BambooMarimba");
    }
    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnClickStart);
        _quitButton.UnregisterCallback<ClickEvent>(OnClickQuit);
        _controlsButton.UnregisterCallback<ClickEvent>(OnClickControls);
        _settingsButton.UnregisterCallback<ClickEvent>(OnClickSettings);
        _returnSettingsButton.UnregisterCallback<ClickEvent>(OnClickReturnSettings);
        _controlsReturnButton.UnregisterCallback<ClickEvent>(OnClickReturnControls);
    }

    private void OnClickStart(ClickEvent click)
    {
        Globals.Managers.Settings.EnableHUD();
        Globals.Managers.Settings.TransitionScene();
    }

    private void OnClickSettings(ClickEvent click)
    {
        _settingsContainer.visible = true;
        _settingsContainer.BringToFront();
        _mainContainer.visible = false;
    }
    private void OnClickControls(ClickEvent click)
    {
        _controlsContainer.visible = true;
        _controlsContainer.BringToFront();
        _mainContainer.visible = false;
        _controlsCarousel.Ready();
    }
    private void OnClickCredits(ClickEvent click)
    {
        _creditsContainer.visible = true;
        _creditsContainer.BringToFront();
        _mainContainer.visible = false;
    }
    private void OnClickReturnCredits(ClickEvent click)
    {
        _mainContainer.visible = true;
        _mainContainer.BringToFront();
        _creditsContainer.visible = false;
    }
    private void OnClickQuit(ClickEvent click)
    {
        Application.Quit();
    }

    private void OnClickReturnSettings(ClickEvent click)
    {
        _mainContainer.visible = true;
        _mainContainer.BringToFront();
        _settingsContainer.visible=false;
    }
    private void OnClickReturnControls(ClickEvent click)
    {
        _mainContainer.visible = true;
        _mainContainer.BringToFront();
        _controlsContainer.visible=false;
        _controlsCarousel.UnReady();
    }

    private void Update()
    {
        if (InputSystem.actions.FindAction("Unlock Abilities").IsPressed())
        {
            SceneManager.LoadScene(2);
        }
    }
}
