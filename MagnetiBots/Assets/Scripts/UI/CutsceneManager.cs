using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    private int _index;
    [SerializeField] private float cutsceneDuration;
    private bool _skipping = false;
    [SerializeField] private TextMeshProUGUI skipText;
    
    private void Start()
    {
        Globals.Managers.Settings.DisableHUD();
        Globals.Managers.Settings.DisablePause();
        Globals.Managers.Audio.StopBGM();
        Globals.Managers.paused = false;
        skipText.gameObject.SetActive(false);
        _index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadNextScene());
        StartCoroutine(SkipCutscene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(cutsceneDuration);
        if (_index == 1)
        {
            _index ++;
            SceneManager.LoadScene(_index);
        }
        else if(_index == 2)
        {
            _index++;
            Globals.Managers.Settings.FadeAway("Load");
        }
        else
        {
            Globals.Managers.Settings.FadeAway("Credits");
        }
    }

    private void Update()
    {
        /*if (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame() && !_skipping)
        {
            _skipping = true;
            if (_index == 1)
            {
                _index ++;
                if (Globals.Managers)
                {
                    Globals.Managers.Settings.FadeAway("Load", _index);
                }
                else
                {
                    SceneManager.LoadScene(_index);
                }
            }
            else if(_index == 2)
            {
                _index++;
                if (Globals.Managers)
                {
                    Globals.Managers.Settings.FadeAway("Load");
                }
                else
                {
                    SceneManager.LoadScene(_index);
                }
            }
            else
            {
                if (Globals.Managers)
                {
                    Globals.Managers.Settings.FadeAway("Credits");
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
        }*/
    }

    IEnumerator SkipCutscene()
    {
        yield return new WaitForSecondsRealtime(3);
        skipText.gameObject.SetActive(true);
        while (true)
        {
            //Globals.Managers.Settings.DisableHUD();
            Globals.Managers.Settings.DisablePause();
            if (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame() && !_skipping)
            {
                _skipping = true;
                if (_index == 1)
                {
                    _index ++;
                    if (Globals.Managers)
                    {
                        Globals.Managers.Settings.FadeAway("Load", _index);
                    }
                    else
                    {
                        SceneManager.LoadScene(_index);
                    }
                }
                else if(_index == 2)
                {
                    _index++;
                    if (Globals.Managers)
                    {
                        Globals.Managers.Settings.FadeAway("Load");
                    }
                    else
                    {
                        SceneManager.LoadScene(_index);
                    }
                }
                else
                {
                    if (Globals.Managers)
                    {
                        Globals.Managers.Settings.FadeAway("Credits");
                    }
                    else
                    {
                        SceneManager.LoadScene(0);
                    }
                }
            }
            yield return null;
        }
    }
}
