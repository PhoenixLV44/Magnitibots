using Player;
using UnityEngine;

public class MovementState : GroundedState
{
    public MovementState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
