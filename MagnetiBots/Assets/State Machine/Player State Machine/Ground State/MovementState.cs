using Player.States;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : GroundedState
{
    public MovementState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }

    public override void EnterState()
    {
        Debug.Log("Entering Movement State");
    }
    public override void ExitState()
    {
        Debug.Log("Exiting Movement State");
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        
        chargeInput = InputSystem.actions.FindAction("Charge").IsPressed();
        
        if(chargeInput)
            stateMachine.ChangeState(stateManager.ChargeState);

        if(moveInput == Vector2.zero)
            stateMachine.ChangeState(stateManager.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        stateManager.PlayerMovement.Move(stateManager.PlayerMovement.Submitted);
    }
}
