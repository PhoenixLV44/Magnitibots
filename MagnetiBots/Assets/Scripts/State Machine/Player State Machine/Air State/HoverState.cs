using UnityEngine;
using UnityEngine.InputSystem;

public class HoverState : AirState
{
    public HoverState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered HoverState"); 
    }

    public override void ExitState()
    {
        animator.SetBool("Hovering", false);
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        if (player.Movement.Grounded)
        {
            stateMachine.ChangeState(stateManager.IdleState);
        }

        if (InputSystem.actions.FindAction("Jump").IsPressed() || InputSystem.actions.FindAction("Charge").IsPressed())
        {
            stateMachine.ChangeState(stateManager.FallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        stateManager.PlayerMovement.Move(stateManager.PlayerMovement.Submitted[0]);
    }
}
