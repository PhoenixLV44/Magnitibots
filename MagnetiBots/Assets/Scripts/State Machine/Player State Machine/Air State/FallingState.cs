using UnityEngine;

public class FallingState : AirState
{
    public FallingState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    public override void EnterState()
    {
        base.EnterState();
        //animator.Play("Fall");
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
