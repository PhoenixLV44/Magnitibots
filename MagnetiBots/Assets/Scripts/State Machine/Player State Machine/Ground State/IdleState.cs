using Ability;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : GroundedState
{
    public IdleState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }

    public override void EnterState()
    {
        //Debug.Log("Entering Idle State");
        base.EnterState();
        animator.Play("IdleWalk");
        if (currentAbility == player.GetComponent<Lasso>())
        {
            player.MerbleBoss.FireMerbles();
            currentAbility.StopCharging();
        }
        //Cursor.lockState = CursorLockMode.None;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (player.LassoHooked)
        {
            stateMachine.ChangeState(stateManager.LassoHookedState);
        }

        if (InputSystem.actions.FindAction("Charge").IsPressed() && player.AbilityStateManager.StateMachine.CurrentState != player.AbilityStateManager.NoAbilityState)
        {
            if (player.CanCharge)
            {
                stateMachine.ChangeState(stateManager.ChargeState);
            }
            else
            {
                Debug.Log("Idle State: Cant charge rn");
            }
        }

        if (InputSystem.actions.FindAction("Jump").WasPerformedThisFrame() && !player.Movement.JumpLock)
        {
            stateMachine.ChangeState(stateManager.JumpState);
        }

        
        if (moveInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateManager.MovementState);
        }

        if (!player.Movement.Grounded)
        {
            stateMachine.ChangeState(stateManager.FallState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.Movement != null && !player.Interacting)
        {
            //player.Movement.Look(player.Movement.Submitted[1]);
            //Debug.LogError("NOT NULL");
        }
        else
        {
            //stateManager = player.PlayerStateManager;
            //Debug.LogError("No State Manager found!");
        }
    }
}
