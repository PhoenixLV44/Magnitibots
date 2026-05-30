using UnityEngine;

namespace Ability
{
    public class NoAbilityState : State
    {
        public NoAbilityState(Player.Controller playerController, StateMachine stateMachine, StateManager stateManager) : base(playerController, stateMachine, stateManager) { }
        public override void EnterState()
        {
            base.EnterState();
            if (ability == null)
            {
                ability = player.LassoAbility;
            }
        }
    }
}