using Player.States;
using UnityEngine;

public class AirState : PlayerState
{
    public AirState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
