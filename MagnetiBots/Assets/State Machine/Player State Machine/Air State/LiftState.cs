using Player.States;
using UnityEngine;

public class LiftState : AirState
{
    public LiftState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
