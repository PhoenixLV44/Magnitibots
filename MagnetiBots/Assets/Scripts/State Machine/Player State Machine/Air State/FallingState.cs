using UnityEngine;

public class FallingState : AirState
{
    public FallingState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
