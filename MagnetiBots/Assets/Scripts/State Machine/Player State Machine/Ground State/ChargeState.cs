using Ability;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeState : GroundedState
{
    public ChargeState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    

    public override void EnterState()
    {
        //Debug.Log("Entering Charge State");
        base.EnterState();

        if (player.Movement.Grounded)
        {
            currentAbility.StartCharging();
        }
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

            /*switch (abilityManager.CurrentAbility)
            {
                case Ability.SuperJump:
                    stateMachine.ChangeState(stateManager.JumpState);
                    break;
                default:
                    stateMachine.ChangeState(stateManager.IdleState);
                    break;
            }*/
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
    }
}
