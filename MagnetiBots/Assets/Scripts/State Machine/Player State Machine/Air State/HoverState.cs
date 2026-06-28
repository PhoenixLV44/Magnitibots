using UnityEngine;
public class HoverState : AirState
{
    public HoverState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    public override void EnterState()
    {
        base.EnterState();
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
    }
}
