using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : GroundedState
{
    public IdleState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }

    public override void EnterState()
    {
        //Debug.Log("Entering Idle State");
        base.EnterState();
        animator.Play("Idle");
        Cursor.lockState = CursorLockMode.None;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (player.LassoHooked)
        {
            stateMachine.ChangeState(stateManager.LassoHookedState);
        }

        if (InputSystem.actions.FindAction("Charge").IsPressed())
        {
            stateMachine.ChangeState(stateManager.ChargeState);
        }
        
        if (moveInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateManager.MovementState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (stateManager.PlayerMovement != null)
        {
            stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
            //Debug.LogError("NOT NULL");
        }
        else
        {
            Debug.LogError("No State Manager found!");
        }

        if (InputSystem.actions.FindAction("Fire").IsPressed())
        {
            //_currentAbility.Fire();
        }
    }
}
