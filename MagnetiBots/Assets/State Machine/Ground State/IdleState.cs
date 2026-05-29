using Player.States;
using UnityEngine;

public class IdleState : GroundedState
{
    public IdleState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
