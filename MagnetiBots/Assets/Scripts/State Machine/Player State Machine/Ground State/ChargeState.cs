using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeState : GroundedState
{
    public ChargeState(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }
    
    private Ability.StateManager abilityManager;
    private Ability.Parent currentAbility;
    

    public override void EnterState()
    {
        Debug.Log("Entering Charge State");
        if (!abilityManager)
        {
            abilityManager = stateManager.gameObject.GetComponent<Ability.StateManager>();
        }

        currentAbility = abilityManager.StateMachine.CurrentState.Ability;
        currentAbility.IsCharging = true;
    }

    public override void ExitState()
    {
        currentAbility.IsCharging = false;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (!InputSystem.actions.FindAction("Charge").IsPressed() || InputSystem.actions.FindAction("Fire").IsPressed())
        {
            stateMachine.ChangeState(stateManager.IdleState);
        }
        

    }
}
