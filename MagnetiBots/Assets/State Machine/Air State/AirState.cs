using Player.States;
using UnityEngine;

public class AirState : PlayerState
{
    public AirState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
