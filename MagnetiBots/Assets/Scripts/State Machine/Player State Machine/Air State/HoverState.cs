using UnityEngine;
public class HoverState : AirState
{
    public HoverState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    public override void EnterState()
    {
        base.EnterState();
        animator.Play("Hover");
    }
}
