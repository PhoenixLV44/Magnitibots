using UnityEngine;
using Player.States;
using UnityEngine.InputSystem;

public abstract class GroundedState : PlayerState
{
    public GroundedState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }

    protected Vector2 moveInput;
    protected bool isGrounded;
    protected bool chargeInput;
    protected bool fireInput;

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>() ;
        TransitionChecks();
    }
}
