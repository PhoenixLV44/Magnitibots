using UnityEngine;

public class Globals : MonoBehaviour
{
    private static Globals _gInstance;
    public static Globals Managers { get { return _gInstance; } }
    private void Awake() {
        #region Singleton
        if (_gInstance != null && _gInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _gInstance = this;
            DontDestroyOnLoad(gameObject);

            _sInstance = gameObject.AddComponent<SaveManager>();
            _aInstance = gameObject.AddComponent<AudioManager>();
            _setInstance = gameObject.AddComponent<SettingsManager>();

            Saves.LateAwake();
            Settings.LateAwake();
            Audio.LateAwake();
            paused = false;
        }
        #endregion
    }

    #region Saving
    private SaveManager _sInstance;
    public SaveManager Saves {  get { return _sInstance; } }
    #endregion

    #region Audio
    private AudioManager _aInstance;
    public AudioManager Audio {  get { return _aInstance; } }
    #endregion

    #region Settings
    private SettingsManager _setInstance;
    public SettingsManager Settings { get { return _setInstance; } }
    #endregion

    public bool paused;
}