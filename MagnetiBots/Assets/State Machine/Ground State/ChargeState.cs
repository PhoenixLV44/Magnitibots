using Player.States;
using UnityEngine;

public class ChargeState : GroundedState
{
    public ChargeState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
