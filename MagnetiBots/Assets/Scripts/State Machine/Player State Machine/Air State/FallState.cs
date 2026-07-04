using UnityEngine;

public class FallState : AirState
{
    public FallState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    public override void EnterState()
    {
        base.EnterState();
        //animator.Play("Fall");
        //Debug.Log("Entered FallState");
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        if (player.Movement.Grounded)
        {
            stateMachine.ChangeState(stateManager.IdleState);
        }

        if (player.Movement.Hovering)
        {
            stateMachine.ChangeState(stateManager.HoverState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.Movement.Move(player.Movement.Submitted[0]);
    }
}
