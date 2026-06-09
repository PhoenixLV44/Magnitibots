using UnityEngine;

public class JumpState: AirState
{
    public JumpState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
