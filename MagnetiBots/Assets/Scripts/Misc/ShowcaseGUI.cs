using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShowcaseGUI : MonoBehaviour
{
    [SerializeField] private Player.Controller _playerController;
    [SerializeField] private Player.StateManager _playerStateManager;
    [SerializeField] private Ability.StateManager _abilityStateManager;
    

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
        if (InputSystem.actions.FindAction("MainMenu").WasReleasedThisFrame())
        {
            Debug.Log("MainMenu Pressed");
            SceneManager.LoadScene("MenuScene");
        }
    }
    
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 20;
        
        /*GUILayout.BeginArea(new Rect(20, 20, 1000, 500));
        {
            GUILayout.Label("Player State: "  + _playerStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Current Ability: " + _abilityStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Charge Level: " + _abilityStateManager.StateMachine.CurrentState.Ability.CurrentPowerLevel);
        }
        GUILayout.EndArea();*/
        
        GUILayout.BeginArea(new Rect(20, 800, 1000, 500));
        {
            GUILayout.Label("WASD to Move, Space to Jump");
            GUILayout.Label("Q and E to Rotate Camera");
            if (_playerController.CanUsePropeller)
            {
                GUILayout.Label("Hold Space to Charge Propeller");
            }
            GUILayout.Label("Hold Left Click to Charge Ability, Release to Use Ability");
            GUILayout.Label("Move Mouse to Aim Ability");
            
            if (_playerController.CanUseSmash)
            {
                GUILayout.Label("Press 1 to use Lasso, 2 to use Smash");
            }
            GUILayout.Label("Press R to Respawn");
            GUILayout.Label("Press Esc to Return to Main Menu");
        }
        GUILayout.EndArea();
        
    }
}
