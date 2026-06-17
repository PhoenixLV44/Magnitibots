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
        }
        #endregion

        _sInstance = new SaveManager();
        _aInstance = new AudioManager();
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
}