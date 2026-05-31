using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        chargeInput = InputSystem.actions.FindAction("Charge").IsPressed();
        
        fireInput = InputSystem.actions.FindAction("Fire").IsPressed();
        
        if(!chargeInput || fireInput)
            stateMachine.ChangeState(stateManager.IdleState);

    }
}
