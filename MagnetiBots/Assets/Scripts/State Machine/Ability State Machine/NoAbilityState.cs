using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class NoAbilityState : State
    {
        public NoAbilityState(Player.Controller playerController, StateMachine stateMachine, StateManager stateManager, Parent ability) : base(playerController, stateMachine, stateManager, ability) { }
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("No Ability State");
            player.TargetCursor.CurrentAbility = ability;
        }
        public override void TransitionChecks()
        {
            base.TransitionChecks();
            if (player.CanUseLasso)
            {
                stateMachine.ChangeState(stateManager.LassoState);
            }
        }
    }
}