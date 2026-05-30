using Player.States;
using UnityEngine;

public class LassoState : GroundedState
{
    public LassoState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
