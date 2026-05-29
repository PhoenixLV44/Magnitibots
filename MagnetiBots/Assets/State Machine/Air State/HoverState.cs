using UnityEngine;

public class HoverState : AirState
{
    public HoverState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
