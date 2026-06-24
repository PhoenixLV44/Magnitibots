using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeState : GroundedState
{
    public ChargeState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
    
    private Ability.StateManager _abilityManager;
    private Ability.Parent _currentAbility;
    

    public override void EnterState()
    {
        //Debug.Log("Entering Charge State");
        if (!_abilityManager)
        {
            _abilityManager = stateManager.gameObject.GetComponent<Ability.StateManager>();
        }

        _currentAbility = _abilityManager.StateMachine.CurrentState.Ability;

        if (_currentAbility == player.PropellerAbility && !player.CanUsePropeller)
        {
            //_currentAbility.Fire();
            //stateMachine.ChangeState(stateManager.IdleState);
        }
        else
        {
        }
        _currentAbility.StartCharging();
        Cursor.lockState = CursorLockMode.None;
        /*player.Movement.CharacterController*/
    }

    public override void ExitState()
    {
        _currentAbility.StopCharging();
        _currentAbility.IsCharging = false;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (player.LassoHooked)
        {
            stateMachine.ChangeState(stateManager.LassoHookedState);
        }

        if (_abilityManager.StateMachine.CurrentState == _abilityManager.LassoState || _abilityManager.StateMachine.CurrentState == _abilityManager.SmashState)
        {
            if (InputSystem.actions.FindAction("Charge").WasReleasedThisFrame())
            {
                Debug.Log("AFHUFADSHJF");
                _currentAbility.Fire();
                stateMachine.ChangeState(stateManager.IdleState);
            }
        }
        else if(_abilityManager.StateMachine.CurrentState == _abilityManager.PropellerState)
        {
            if (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame())
            {
                _currentAbility.Fire();
                stateMachine.ChangeState(stateManager.IdleState);
            }
        }
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
    }
}
