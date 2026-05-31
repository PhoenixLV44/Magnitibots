using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : GroundedState
{
    public IdleState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
    
    private Ability.StateManager _abilityManager;
    private Ability.Parent _currentAbility;

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
        if (!_abilityManager)
        {
            _abilityManager = stateManager.gameObject.GetComponent<Ability.StateManager>();
        }

        _currentAbility = _abilityManager.StateMachine.CurrentState.Ability;
        _currentAbility.IsCharging = true;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (player.LassoHooked)
        {
            stateMachine.ChangeState(stateManager.LassoHookedState);
        }
        
        if(InputSystem.actions.FindAction("Charge").IsPressed())
            stateMachine.ChangeState(stateManager.ChargeState);
        
        if(moveInput != Vector2.zero)
            stateMachine.ChangeState(stateManager.MovementState);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);

        if (InputSystem.actions.FindAction("Fire").IsPressed())
        {
            _currentAbility.Fire();
        }
    }
}
