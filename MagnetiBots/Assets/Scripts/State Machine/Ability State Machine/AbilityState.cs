using UnityEngine;

namespace Ability
{
    public class State
    {
        protected Player.Controller player;
        
        protected Parent ability;
        public Parent Ability{get => ability;}

        protected StateMachine stateMachine;
        protected StateManager stateManager;
        
        protected Animator animationController;
        protected string animationName;

        protected bool isExitingState;
        protected bool isAnimationFinished;
        protected float startTime;

        public State(Player.Controller _player, StateMachine _stateMachine, StateManager _stateManager, Parent _ability)
        {
            player = _player;
            stateMachine = _stateMachine;
            stateManager = _stateManager;
            ability = _ability;
        }
        public virtual void EnterState()
        {
            //isAnimationFinished = false;
            isExitingState = false;
            startTime = Time.time;
            ability.enabled = true;
            ability.IsCharging = false;
            //animationController.SetBool(animationName, true);
        }
        public virtual void ExitState()
        {
            isExitingState = true;
            ability.IsCharging = false;
            ability.enabled = false;
            //if (!isAnimationFinished) isAnimationFinished = true;
            //animationController.SetBool(animationName, false);
        }
        public virtual void LogicUpdate()
        {

        }
        public virtual void PhysicsUpdate()
        {

        }
        public virtual void TransitionChecks()
        {

        }
        public virtual void AnimationTrigger()
        {
            isAnimationFinished = true;
        }
    }
}
