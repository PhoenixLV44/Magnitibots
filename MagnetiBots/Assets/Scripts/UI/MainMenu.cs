using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

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
    private ListView _sources;
    private Button _creditsReturnButton;

    private void Start()
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
        _sources = _creditsContainer.Q("SourcesList") as ListView;
        VisualElement content = _sources.Q("unity-content-container");
        _sources.Q<ScrollView>().RegisterCallback<WheelEvent>(evt => {
            evt.StopPropagation();
        }, TrickleDown.TrickleDown);

        // Intercept touch / pointer drag scrolling
        _sources.Q<ScrollView>().RegisterCallback<PointerMoveEvent>(evt => {
            // Check if the user is attempting to drag-scroll
            if (evt.pressedButtons == 1)
            {
                evt.StopPropagation();
            }
        }, TrickleDown.TrickleDown);
        AnimateSources();
        _creditsReturnButton.RegisterCallback<ClickEvent>(OnClickReturnCredits);
        #endregion


        if (Globals.Managers.Settings.waitMenu)
        {
            _mainContainer.visible = false;
        }

        _controlsContainer.visible = false;
        Globals.Managers.Settings.DisableHUD();
        Globals.Managers.Settings.DisablePause();
        Globals.Managers.Settings.SettingsMenuSetup();
        Globals.Managers.Audio.UpdateBGM("BambooMarimba");
        Cursor.lockState = CursorLockMode.None;
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
        Globals.Managers.Settings.SettingsMenuUnregister();
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
        GoToCredits();
    }
    public void GoToCredits()
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
    private void AnimateSources(int index = 0)
    {
        //Debug.Log(index);
        if (_sources.itemsSource is System.Collections.IList listData)
        {
            int itemCount = listData.Count;
            //Debug.Log(itemCount);
            if (index >= itemCount) { index = 0; Debug.Log("what?"); }
        }
        
        _sources.schedule.Execute(() =>
        {
            if (_sources.resolvedStyle.height == 0) return;

            //scroll offset
            float itemHeight = _sources.fixedItemHeight;
            float targetOffset = (index * itemHeight) - (_sources.resolvedStyle.height / 2f) + (itemHeight / 2f);
            ScrollView scrollView = _sources.Q<ScrollView>();
            targetOffset = Mathf.Clamp(targetOffset, 0, scrollView.contentViewport.resolvedStyle.height);
            scrollView.scrollOffset = new Vector2(0, targetOffset);

            _sources.ScrollToItem(index);
            AnimateSources(index+1);
        }).ExecuteLater(3000);
    }
}
