using System;
using UnityEngine;

namespace Player
{
    public class StateManager : MonoBehaviour
    {
        #region Non-State Scripts
            private  Player.Controller _playerController;
            public Player.Controller PlayerController {get => _playerController;
                set => _playerController = value; }
            private Player.Movement _playerMovement;
            public Player.Movement PlayerMovement {get => _playerMovement;
                set => _playerMovement = value; }
            private StateMachine _stateMachine;
            public StateMachine StateMachine => _stateMachine;
        #endregion
        
        #region Ground States
            private IdleState _idleState;
            public IdleState IdleState => _idleState;

            private MovementState _movementState;
            public MovementState MovementState => _movementState;

            private ChargeState _chargeState;
            public ChargeState ChargeState => _chargeState;

            private LassoHooked _lassoHookedState;
            public LassoHooked LassoHookedState => _lassoHookedState;

            #endregion

        #region Air States
            private JumpState _jumpState;
            public JumpState JumpState => _jumpState;

            private FallingState _fallState;
            public FallingState FallState => _fallState;
            
            private HoverState _hoverState;
            public HoverState HoverState => _hoverState;

            #endregion
            
        private void Start()
        {
            _stateMachine = new StateMachine();
            
            _idleState = new IdleState(_playerController, _stateMachine, this, _playerController.Animator);
            _movementState = new MovementState(_playerController, _stateMachine, this, _playerController.Animator);
            _chargeState = new ChargeState(_playerController, _stateMachine, this, _playerController.Animator);
            _lassoHookedState = new LassoHooked(_playerController, _stateMachine, this, _playerController.Animator);
            _jumpState = new JumpState(_playerController, _stateMachine, this, _playerController.Animator);
            _fallState = new FallingState(_playerController, _stateMachine, this, _playerController.Animator);
            _hoverState = new HoverState(_playerController, _stateMachine, this, _playerController.Animator);
            
            _stateMachine.InitializeStateMachine(_idleState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.CurrentState.PhysicsUpdate();
        }
    }
}

