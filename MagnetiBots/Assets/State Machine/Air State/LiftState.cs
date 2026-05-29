using Player.States;
using UnityEngine;

public class LiftState : AirState
{
    public LiftState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
