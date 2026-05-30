using UnityEngine;

namespace Ability
{
    public class State
    {
        protected Player.Controller player;
        
        protected Parent ability;
        
        protected StateMachine stateMachine;
        protected StateManager stateManager;
        
        protected Animator animationController;
        protected string animationName;

        protected bool isExitingState;
        protected bool isAnimationFinished;
        protected float startTime;

        public State(Player.Controller _player, StateMachine _stateMachine, StateManager _stateManager)
        {
            player = _player;
            stateMachine = _stateMachine;
            stateManager = _stateManager;
        }
        public virtual void EnterState()
        {
            //isAnimationFinished = false;
            isExitingState = false;
            startTime = Time.time;
            //animationController.SetBool(animationName, true);
        }
        public virtual void ExitState()
        {
            isExitingState = true;
            //if (!isAnimationFinished) isAnimationFinished = true;
            //animationController.SetBool(animationName, false);
        }
        public virtual void LogicUpdate()
        {
            TransitionChecks();
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
