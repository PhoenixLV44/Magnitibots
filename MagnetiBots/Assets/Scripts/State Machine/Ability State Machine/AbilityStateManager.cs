using System;
using UnityEngine;

namespace Ability
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField] private Player.Controller _playerController;
        public Player.Controller PlayerController {get => _playerController;
            set => _playerController = value;
        }
        
        private StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine;

        [SerializeField] private LassoState _lassoState;
        public LassoState LassoState => _lassoState;
        [SerializeField] private SmashState _smashState;
        public SmashState SmashState => _smashState;
        [SerializeField] private SuperJumpState _superJumpState;
        public SuperJumpState SuperJumpState => _superJumpState;

        [SerializeField] private NoAbilityState _noAbilityState;
        public NoAbilityState NoAbilityState => _noAbilityState;

        private Parent _currentAbility;
        public Parent CurrentAbility {get => _currentAbility;
            set => _currentAbility = value;
        }

        private void Awake()
        {
            if(_playerController == null) _playerController = gameObject.GetComponent<Player.Controller>();
            _stateMachine = new StateMachine();

        }
        private void Start()
        {
            _lassoState = new LassoState(_playerController, _stateMachine, this, gameObject.AddComponent<Lasso>());
            _smashState = new SmashState(_playerController, _stateMachine, this, gameObject.AddComponent<Smash>());
            _superJumpState = new SuperJumpState(_playerController, _stateMachine, this,  gameObject.AddComponent<SuperJump>());
            _noAbilityState = new NoAbilityState(_playerController, _stateMachine, this, gameObject.AddComponent<Parent>());
            
            _lassoState.Ability.enabled = false;
            _smashState.Ability.enabled = false;
            _superJumpState.Ability.enabled = false;
            _noAbilityState.Ability.enabled = false;
            _stateMachine.InitializeStateMachine(_lassoState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.LogicUpdate();
            _stateMachine.CurrentState.TransitionChecks();
        }
    }  
}

