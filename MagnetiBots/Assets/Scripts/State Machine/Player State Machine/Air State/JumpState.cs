using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState: AirState
{
    public JumpState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    
    

    public override void EnterState()
    {
        base.EnterState();
        
        animator.Play("Jump");
        Cursor.lockState = CursorLockMode.None;
        /*player.Movement.CharacterController*/
    }

    public override void ExitState()
    {
        currentAbility.StopCharging();
        currentAbility.IsCharging = false;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (player.LassoHooked)
        {
            stateMachine.ChangeState(stateManager.LassoHookedState);
        }

        if (InputSystem.actions.FindAction("Charge").WasReleasedThisFrame())
        {
            //Debug.Log("AFHUFADSHJF");
            currentAbility.Fire();
            stateMachine.ChangeState(stateManager.IdleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
    }
}
