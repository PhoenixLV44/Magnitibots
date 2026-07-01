using UnityEngine;
using System.Collections;

namespace Player
{
    public abstract class State
    {
        protected Player.Controller player;
        protected Player.StateMachine stateMachine;
        protected Player.StateManager stateManager;
        protected Animator animator;
        protected string animationName;

        protected bool isExitingState;
        protected bool isAnimationFinished;
        protected float startTime;
        
        protected Ability.StateManager abilityManager;
        protected Ability.Parent currentAbility;

        public State(Player.Controller _player, StateMachine _stateMachine, Player.StateManager _stateManager, Animator _animator)
        {
            player = _player;
            stateMachine = _stateMachine;
            stateManager = _stateManager;
            animator = _animator;
            //animationName = _animationName;
        }
        public virtual void EnterState()
        {
            //isAnimationFinished = false;
            isExitingState = false;
            startTime = Time.time;
            //animationController.SetBool(animationName, true);
            if (!abilityManager)
            {
                abilityManager = stateManager.gameObject.GetComponent<Ability.StateManager>();
            }

            if (abilityManager)
            {
                currentAbility = abilityManager.StateMachine.CurrentState.Ability;
            }
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