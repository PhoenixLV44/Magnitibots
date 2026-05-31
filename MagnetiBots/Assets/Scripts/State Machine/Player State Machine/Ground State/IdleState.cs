using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : GroundedState
{
    public IdleState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        
        if(InputSystem.actions.FindAction("Charge").IsPressed())
            stateMachine.ChangeState(stateManager.ChargeState);
        
        if(moveInput != Vector2.zero)
            stateMachine.ChangeState(stateManager.MovementState);
    }
}
