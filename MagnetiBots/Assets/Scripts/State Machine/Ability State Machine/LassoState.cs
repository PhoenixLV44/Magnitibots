using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class LassoState : State
    {
        public LassoState(Player.Controller player, StateMachine stateMachine, StateManager stateManager, Lasso ability) : base(player, stateMachine, stateManager,  ability) { }
        public override void EnterState()
        {
            base.EnterState();
            //Debug.Log("Entered Lasso State");
            Globals.Managers.Settings.UpdateHUD("Lasso");
            /*player.TargetCursor.CurrentAbility = ability;
            */
        }

        public override void ExitState()
        {
            base.ExitState();
            ability.StopAllCoroutines();
            ability.MerbleBoss.FireMerbles();
            ability.enabled = false;
            //Debug.Log("Exited Lasso State");
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();
            if (player.PlayerStateManager.StateMachine.CurrentState != player.PlayerStateManager.ChargeState || player.MerbleBoss.ChargedMerbleList.Count > 0)
            {
                if (!player.LassoHooked )
                {
                    if (InputSystem.actions.FindAction("Activate Smash").IsPressed() && player.CanUseSmash)
                    {
                        /*
                         Lasso lasso = player.GetComponent<Lasso>();
                        lasso.StartCoroutine(lasso.UnhookLasso());*/
                        stateMachine.ChangeState(stateManager.SmashState);
                        /*
                        Globals.Managers.Settings.UpdateHUD();
                    */
                    }
                    if (InputSystem.actions.FindAction("Activate Super Jump").IsPressed() && player.CanUseSuperJump)
                    {
                        /*Lasso lasso = player.GetComponent<Lasso>();
                        lasso.StartCoroutine(lasso.UnhookLasso());*/
                        stateMachine.ChangeState(stateManager.SuperJumpState);
                        /*
                        Globals.Managers.Settings.UpdateHUD();
                    */
                    }
                }
            }
        }
    }
}

