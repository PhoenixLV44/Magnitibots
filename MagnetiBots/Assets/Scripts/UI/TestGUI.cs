using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TestGUI : MonoBehaviour
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
        if (InputSystem.actions.FindAction("Crouch").WasReleasedThisFrame())
            Application.Quit();
    }
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 20;

        GUILayout.BeginArea(new Rect(20, 20, 1000, 500));
        {
            GUILayout.Label("Player State: " + _playerStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Current Ability: " + _abilityStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Charge Level: " + _abilityStateManager.StateMachine.CurrentState.Ability.CurrentPowerLevel);
        }
        GUILayout.EndArea();
    }


}
