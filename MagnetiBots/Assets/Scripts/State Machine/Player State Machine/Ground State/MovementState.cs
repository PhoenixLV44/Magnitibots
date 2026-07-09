using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : GroundedState
{
    public MovementState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    

    public override void EnterState()
    {
        //Debug.Log("Entering Movement State");
        base.EnterState();
        currentAbility = abilityManager.StateMachine.CurrentState.Ability;
        //animator.Play("Walk");
        //Cursor.lockState = CursorLockMode.None;
    }
    public override void ExitState()
    {
        //Debug.Log("Exiting Movement State");
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (InputSystem.actions.FindAction("Charge").IsPressed())
        {
            if (player.CanCharge)
            {
                stateMachine.ChangeState(stateManager.ChargeState);
            }
            else
            {
                Debug.Log("Move State: Cant charge rn");
            }
        }

        if (InputSystem.actions.FindAction("Jump").IsPressed() && !player.Movement.JumpLock)
        {
            stateMachine.ChangeState(stateManager.JumpState);
        }

        if(moveInput == Vector2.zero)
            stateMachine.ChangeState(stateManager.IdleState);


        if (!player.Movement.Grounded)
        {
            stateMachine.ChangeState(stateManager.FallState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!player.Interacting)
        {
            stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
        }

        if (!player.Movement.Hovering && player.MerbleBoss.ChargedMerbleList.Count > 0)
        {
            //player.MerbleBoss.FireMerbles();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        stateManager.PlayerMovement.Move(stateManager.PlayerMovement.Submitted[0]);
    }
}
