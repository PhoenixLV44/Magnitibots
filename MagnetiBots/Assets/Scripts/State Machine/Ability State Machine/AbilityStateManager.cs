using System;
using UnityEngine;

namespace Ability
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField] private Player.Controller _playerController;
        public Player.Controller PlayerController {get {return _playerController;} set {_playerController = value;} }
        
        private StateMachine _stateMachine;
        
        [SerializeField] private LassoState _lassoState;
        public LassoState LassoState {get {return _lassoState;} }
        [SerializeField] private SmashState _smashState;
        public SmashState SmashState {get {return _smashState;} }
        [SerializeField] private PropellerState _propellerState;
        public PropellerState PropellerState {get {return _propellerState;} }
        
        [SerializeField] private NoAbilityState _noAbilityState;
        public NoAbilityState NoAbilityState {get {return _noAbilityState;} }

        private void Awake()
        {
            if(_playerController == null) _playerController = gameObject.GetComponent<Player.Controller>();
            _stateMachine = new StateMachine();
            _lassoState = new LassoState(_playerController, _stateMachine, this, gameObject.AddComponent<Lasso>());
            _smashState = new SmashState(_playerController, _stateMachine, this, gameObject.AddComponent<Smash>());
            _propellerState = new PropellerState(_playerController, _stateMachine, this,  gameObject.AddComponent<Propeller>());
            _noAbilityState = new NoAbilityState(_playerController, _stateMachine, this, gameObject.AddComponent<Parent>());
        }

        private void Start()
        {
            _stateMachine.InitializeStateMachine(_noAbilityState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.LogicUpdate();
            _stateMachine.CurrentState.TransitionChecks();
        }
    }  
}

