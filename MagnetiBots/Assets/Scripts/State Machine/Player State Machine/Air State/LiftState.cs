using UnityEngine;

public class LiftState : AirState
{
    public LiftState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
