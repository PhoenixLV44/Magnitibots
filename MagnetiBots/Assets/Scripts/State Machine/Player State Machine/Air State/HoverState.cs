using UnityEngine;
public class HoverState : AirState
{
    public HoverState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
}
