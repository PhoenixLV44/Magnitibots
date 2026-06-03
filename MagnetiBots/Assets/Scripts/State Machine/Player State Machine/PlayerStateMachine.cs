using UnityEngine;

namespace Player.States
{
    public class PlayerStateMachine
    {
        private PlayerState  _currentState;
        public PlayerState CurrentState { get { return _currentState; }  private set { _currentState = value; } }

        public void InitializeStateMachine(PlayerState initialState)
        {
            _currentState = initialState;

            _currentState.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            _currentState.ExitState();
            
            _currentState = newState;

            _currentState.EnterState();
        }
    }
}
