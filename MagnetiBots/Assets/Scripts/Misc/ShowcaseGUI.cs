using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowcaseGUI : MonoBehaviour
{
    [SerializeField] private Player.Controller _playerController;
    [SerializeField] private Player.StateManager _playerStateManager;
    [SerializeField] private Ability.StateManager _abilityStateManager;

    [SerializeField] private Camera[] cameras;
    private int _cameraInt = 0;

    private void Start()
    {
        StartCoroutine(GetVariables());
    }

    IEnumerator GetVariables()
    {
        while (true)
        {
            if (_playerStateManager == null || _playerController == null || _abilityStateManager == null)
            {
                if (_playerController == null)
                {
                    _playerController = GameObject.Find("Player Stand-in").GetComponent<Player.Controller>();
                }

                if (_playerStateManager == null)
                {
                    _playerStateManager = _playerController.gameObject.GetComponent<Player.StateManager>();
                }

                if (_abilityStateManager == null)
                {
                    _abilityStateManager = _playerController.gameObject.GetComponent<Ability.StateManager>();
                    
                }

                yield return null;
            }
            else
            {
                yield break;
            }
        }
    }

    private void Update()
    {
        if (InputSystem.actions.FindAction("Sprint").WasReleasedThisFrame())
        {
            SwitchCamera();
        }
        if(InputSystem.actions.FindAction("Crouch").WasReleasedThisFrame())
            Application.Quit();
    }

    private void SwitchCamera()
    {
        switch (_cameraInt)
        {
            case 3:
                _cameraInt = 0;
                break;
            default:
                _cameraInt++;
                break;     
        }

        for(int i = 0; i < cameras.Length; i++)
        {
            if (i != _cameraInt)
            {
                cameras[i].gameObject.SetActive(false);
            }
            else
            {
                cameras[i].gameObject.SetActive(true);
            }
        }
        Debug.Log("Camera Int: " + _cameraInt);
    }
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 20;
        
        GUILayout.BeginArea(new Rect(20, 20, 1000, 500));
        {
            GUILayout.Label("Player State: "  + _playerStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Current Ability: " + _abilityStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Charge Level: " + _abilityStateManager.StateMachine.CurrentState.Ability.CurrentPowerLevel);
        }
        GUILayout.EndArea();
        
        GUILayout.BeginArea(new Rect(20, 920, 1000, 500));
        {
            GUILayout.Label("WASD to Move, Space to Jump");
            GUILayout.Label("Hold Left Mouse to charge ability/Spawn more marbles, Release to fire. Press again to release object");
            GUILayout.Label("Move mouse to move Lasso'd object");
            GUILayout.Label("Press Shift to switch cameras");
            GUILayout.Label("Press C to quit");
        }
        GUILayout.EndArea();
        
        GUILayout.BeginArea(new Rect(1920 - 500, 20, 1000, 500));
        {
            GUILayout.Label("Level Whitebox'd by Gwen");
            GUILayout.Label("Cat Lantern modeled by Vincent, textured by Kaiden");
            GUILayout.Label("Rocks modeled by Nick");
        }
        GUILayout.EndArea();
    }
}
