using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class SuperJumpState : State
    {
        public SuperJumpState(Player.Controller playerController, StateMachine stateMachine, StateManager stateManager, SuperJump ability) : base(playerController, stateMachine, stateManager,  ability) { }
        public override void EnterState()
        {
            base.EnterState();
            /*player.TargetCursor.CurrentAbility = ability;*/
            //Debug.Log("Entered Super Jump State");
            //animator.SetBool("SuperJump", true);
            //Globals.Managers.Settings.UpdateHUD();
        }

        public override void ExitState()
        {
            base.ExitState();
            //animator.SetBool("SuperJump", false);
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();
            if (InputSystem.actions.FindAction("Activate Lasso").IsPressed())
            {
                stateMachine.ChangeState(stateManager.LassoState);
            }

            if (InputSystem.actions.FindAction("Activate Smash").IsPressed())
            {
                stateMachine.ChangeState(stateManager.SmashState);
            }
        }
    }
}

