using Player.States;
using UnityEngine;

public class JumpState: AirState
{
    public JumpState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
