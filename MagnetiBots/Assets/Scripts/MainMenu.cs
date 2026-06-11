using UnityEngine;
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

    private VisualElement _settingsContainer;

    private Button _returnSettingsButton;

    private VisualElement _controlsContainer;

    private Button _controlsReturnButton;

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

        _controlsReturnButton = _mainDocument.rootVisualElement.Q("ControlsReturnButton") as Button;
        _controlsReturnButton.RegisterCallback<ClickEvent>(OnClickReturnControls);
        #endregion

        _mainContainer.BringToFront();

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    }
}
