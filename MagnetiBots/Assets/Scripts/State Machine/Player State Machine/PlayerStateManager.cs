using UnityEngine;

namespace Player
{
    public class StateManager : MonoBehaviour
    {
        private  Player.Controller _playerController;
        public Player.Controller PlayerController {get {return _playerController;} set {_playerController = value;} }
        [SerializeField]private Player.Movement _playerMovement;
        public Player.Movement PlayerMovement {get {return _playerMovement;} set {_playerMovement = value;} }
        private StateMachine _stateMachine;
        public StateMachine StateMachine {get {return _stateMachine;} }
    
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
            _stateMachine = new StateMachine();
            _idleState = new IdleState(_playerController, _stateMachine, this);
            _movementState = new MovementState(_playerController, _stateMachine, this);
            _chargeState = new ChargeState(_playerController, _stateMachine, this);
            _lassoHookedState = new LassoHooked(_playerController, _stateMachine, this);
            _stateMachine.InitializeStateMachine(_idleState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.LogicUpdate();
        }

        private void LateUpdate()
        {
            _stateMachine.CurrentState.PhysicsUpdate();
        }
    }
}

