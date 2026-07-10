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
        //Cursor.lockState = CursorLockMode.None;
        player.AnimController.Charging = true;
        animator.Play("Arm_Up");
        Debug.Log("NOW CHARGING");
        player.ChargingParticles.SetActive(true);
        /*player.Movement.CharacterController*/
    }

    public override void ExitState()
    {
        currentAbility.StopCharging();
        currentAbility.IsCharging = false;
        player.StartChargeLockout();
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
            if (player.MerbleBoss.ChargedMerbleList.Count > 0)
            {
                currentAbility.Fire();
            }
            else
            {
                /*if (currentAbility == player.GetComponent<Lasso>() && player.MerbleBoss.ChargedMerbleList.Count < 1)
                {
                    Debug.Log("NO CHARGE");
                    Lasso lasso = player.GetComponent<Lasso>();
                    lasso.IsCharging = false;
                    player.TargetCursor.CanMoveCursor = true;
                    player.RangeIndicator.DisableRangeIndicator();
                    lasso.StopAllCoroutines();
                    lasso.LoopScript.StopAllCoroutines();
                    player.MerbleBoss.FireMerbles();
                }*/
                currentAbility.StopCharging();
                player.MerbleBoss.FireMerbles();
            }
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
