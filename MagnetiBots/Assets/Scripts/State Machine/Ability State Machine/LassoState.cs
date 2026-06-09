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
            Debug.Log("Entered Lasso State");
        }

        public override void ExitState()
        {
            ability.StopAllCoroutines();
            ability.enabled = false;
            Debug.Log("Exited Lasso State");
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();
            if (InputSystem.actions.FindAction("Activate Smash").IsPressed())
            {
                stateMachine.ChangeState(stateManager.SmashState);
            }
            if (InputSystem.actions.FindAction("Activate Propeller").IsPressed())
            {
                stateMachine.ChangeState(stateManager.SmashState);
            }
        }
    }
}

