using Player.States;
using UnityEngine;

namespace Ability
{
    public class LassoState : State
    {
        public LassoState(Player.Controller playerController, StateMachine stateMachine, StateManager stateManager) : base(playerController, stateMachine, stateManager) { }
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

