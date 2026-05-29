using UnityEngine;
using Player;

namespace Player
{
    public class GroundedState : PlayerState
    {
        public GroundedState(Player.Controller pc, PlayerStateMachine stateMachine, Animator animationController, string animationname) : base(pc,stateMachine, animationController, animationname) { }
    }
}
