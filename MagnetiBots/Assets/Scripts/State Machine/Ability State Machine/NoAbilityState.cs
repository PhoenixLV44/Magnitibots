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
            if(InputSystem.actions.FindAction("Activate Propeller").IsPressed())
            {
                stateMachine.ChangeState(stateManager.PropellerState);
            }
        }
    }
}