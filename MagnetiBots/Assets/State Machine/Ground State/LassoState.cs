using Player;
using UnityEngine;

public class LassoState : GroundedState
{
    public LassoState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationName) : base(pc, stateMachine, animationController, animationName) { }
}
