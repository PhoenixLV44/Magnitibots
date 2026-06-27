using UnityEngine;
using UnityEngine.InputSystem;

public abstract class GroundedState : Player.State
{
    public GroundedState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }

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
