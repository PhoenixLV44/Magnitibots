using System;
using Player.States;
using Player;
using UnityEngine;

namespace Player.States
{
    public class PlayerStateManager : MonoBehaviour
    {
        private  Player.Controller _playerController;
        private Player.Movement _playerMovement;
        private PlayerStateMachine _playerStateMachine;
    
        //private GroundedState _groundedState;
        private IdleState _idleState;
        public IdleState IdleState {get {return _idleState;} }
        private MovementState _movementState;
        public MovementState MovementState {get {return _movementState;} }
        private ChargeState _chargeState;
        public ChargeState ChargeState {get {return _chargeState;} }

        public PlayerStateManager(Player.Controller playerController, Player.Movement playerMovement) // add any other player scripts when necessary
        {
            _playerController = playerController;
            _playerMovement = playerMovement;
        }

        private void Awake()
        {
            _playerStateMachine = new PlayerStateMachine();
            //_groundedState = new GroundedState(_playerController, _playerStateMachine);
            _idleState = new IdleState(_playerController, _playerStateMachine, this);
            _movementState = new MovementState(_playerController, _playerStateMachine, this);
            _chargeState = new ChargeState(_playerController, _playerStateMachine, this);
        }

        private void Start()
        {
            _playerStateMachine.InitializeStateMachine(_idleState);
        }

        private void Update()
        {
            _playerStateMachine.CurrentState.LogicUpdate();
        }

        private void LateUpdate()
        {
            _playerStateMachine.CurrentState.PhysicsUpdate();
        }
    }
}

