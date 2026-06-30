using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class SmashState : State
    {
        public SmashState(Player.Controller playerController, StateMachine stateMachine, StateManager stateManager, Smash ability) : base(playerController, stateMachine, stateManager, ability) { }
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entered Smash State");
        }

        public override void ExitState()
        {
            base.ExitState();
            animator.SetBool("Smash", false);
        }
        public override void TransitionChecks()
        {
            base.TransitionChecks();
            if (InputSystem.actions.FindAction("Activate Lasso").IsPressed() && player.PlayerStateManager.StateMachine.CurrentState != player.PlayerStateManager.ChargeState)
            {
                stateMachine.ChangeState(stateManager.LassoState);
            }
            if (InputSystem.actions.FindAction("Activate Super Jump").IsPressed() && player.CanUseSuperJump && player.PlayerStateManager.StateMachine.CurrentState != player.PlayerStateManager.ChargeState )
            {
                stateMachine.ChangeState(stateManager.SuperJumpState);
            }
        }
    }  
}

