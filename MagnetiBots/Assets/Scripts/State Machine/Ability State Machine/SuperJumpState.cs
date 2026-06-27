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
            Debug.Log("Entered Super Jump State");
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

