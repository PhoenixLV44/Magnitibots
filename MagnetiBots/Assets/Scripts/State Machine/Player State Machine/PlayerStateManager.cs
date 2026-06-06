using System;
using Player.States;
using Player;
using UnityEngine;

namespace Player.States
{
    public class PlayerStateManager : MonoBehaviour
    {
        private  Player.Controller _playerController;
        public Player.Controller PlayerController {get {return _playerController;} set {_playerController = value;} }
        [SerializeField]private Player.Movement _playerMovement;
        public Player.Movement PlayerMovement {get {return _playerMovement;} set {_playerMovement = value;} }
        private PlayerStateMachine _playerStateMachine;
        public PlayerStateMachine PlayerStateMachine {get {return _playerStateMachine;} }
    
        //States for when the player is on the ground
        private IdleState _idleState;
        public IdleState IdleState {get {return _idleState;} }
        
        private MovementState _movementState;
        public MovementState MovementState {get {return _movementState;} }
        
        private ChargeState _chargeState;
        public ChargeState ChargeState {get {return _chargeState;} }
        
        private LassoHooked _lassoHookedState;
        public LassoHooked LassoHookedState {get {return _lassoHookedState;} }


        private void Awake()
        {
        }

        private void Start()
        {
            _playerStateMachine = new PlayerStateMachine();
            _idleState = new IdleState(_playerController, _playerStateMachine, this);
            _movementState = new MovementState(_playerController, _playerStateMachine, this);
            _chargeState = new ChargeState(_playerController, _playerStateMachine, this);
            _lassoHookedState = new LassoHooked(_playerController, _playerStateMachine, this);
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

