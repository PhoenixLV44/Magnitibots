using UnityEngine;

public class AirState : Player.State
{
    public AirState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
