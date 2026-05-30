using Player.States;
using UnityEngine;

public class ChargeState : GroundedState
{
    public ChargeState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }

    public override void EnterState()
    {
        Debug.Log("Entering Charge State");
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        
    }
}
