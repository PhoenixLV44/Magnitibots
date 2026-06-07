using UnityEngine;

namespace Player
{
    public class StateMachine
    {
        private Player.State  _currentState;
        public Player.State CurrentState { get { return _currentState; }  private set { _currentState = value; } }

        public void InitializeStateMachine(Player.State initialState)
        {
            _currentState = initialState;

            _currentState.EnterState();
        }

        public void ChangeState(Player.State newState)
        {
            _currentState.ExitState();
            
            _currentState = newState;

            _currentState.EnterState();
        }
    }
}
