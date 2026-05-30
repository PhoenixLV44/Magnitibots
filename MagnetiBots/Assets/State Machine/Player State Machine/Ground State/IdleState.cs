using Player.States;
using UnityEngine;

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
        
        if(moveInput != Vector2.zero)
            stateMachine.ChangeState(stateManager.MovementState);
    }
}
