using Player.States;
using UnityEngine;

public class JumpState: AirState
{
    public JumpState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
