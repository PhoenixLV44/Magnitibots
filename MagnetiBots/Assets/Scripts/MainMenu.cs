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

    private VisualElement _settingsContainer;

    private Button _returnButton;

    private void Awake()
    {
        
        _mainDocument = GetComponent<UIDocument>();

        #region MainMenu Container and Buttons

        _mainContainer = _mainDocument.rootVisualElement.Q("MainMenu");

        _startButton = _mainDocument.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnClickStart);

        _settingsButton = _mainDocument.rootVisualElement.Q("SettingsButton") as Button;
        _settingsButton.RegisterCallback<ClickEvent>(OnClickSettings);

        _quitButton = _mainDocument.rootVisualElement.Q("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnClickQuit);
        #endregion

        _settingsContainer = _mainDocument.rootVisualElement.Q("SettingsMenu");

        _returnButton = _mainDocument.rootVisualElement.Q("ReturnButton") as Button;
        _returnButton.RegisterCallback<ClickEvent>(OnClickReturn);

        _settingsContainer.visible = false;
        _mainContainer.BringToFront();

    }
    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnClickStart);
        _quitButton.UnregisterCallback<ClickEvent>(OnClickQuit);
        _settingsButton.UnregisterCallback<ClickEvent>(OnClickSettings);
        _returnButton.UnregisterCallback<ClickEvent>(OnClickReturn);
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

    private void OnClickQuit(ClickEvent click)
    {
        Application.Quit();
    }

    private void OnClickReturn(ClickEvent click)
    {
        _mainContainer.visible = true;
        _mainContainer.BringToFront();
        _settingsContainer.visible=false;
    }
}
