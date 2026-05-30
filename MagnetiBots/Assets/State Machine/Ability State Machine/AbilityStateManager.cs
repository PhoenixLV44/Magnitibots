using System;
using UnityEngine;

namespace Ability
{
    public class StateManager : MonoBehaviour
    {
        private Player.Controller _playerController;
        public Player.Controller PlayerController {get {return _playerController;} set {_playerController = value;} }
        
        private StateMachine _stateMachine;
        
        private LassoState _lassoState;
        public LassoState LassoState {get {return _lassoState;} }

        private void Awake()
        {
            StateMachine stateMachine = new StateMachine();
            _lassoState = new LassoState(_playerController, stateMachine, this);
        }
    }   
}

