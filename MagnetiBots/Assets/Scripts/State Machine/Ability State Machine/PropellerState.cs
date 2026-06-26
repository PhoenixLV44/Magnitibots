using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class PropellerState : State
    {
        public PropellerState(Player.Controller playerController, StateMachine stateMachine, StateManager stateManager, Propeller ability) : base(playerController, stateMachine, stateManager,  ability) { }
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entered Propeller State");
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();
            if (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame())
            {
                stateMachine.ChangeState(stateMachine.PreviousState);
            }
        }
    }
}

