using UnityEngine;
using Player.States;
public class HoverState : AirState
{
    public HoverState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
