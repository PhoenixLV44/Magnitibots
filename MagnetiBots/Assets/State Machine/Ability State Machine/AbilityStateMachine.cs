using UnityEngine;

namespace Ability
{
    public class StateMachine
    {
        private State  _currentState;
        public State CurrentState { get { return _currentState; }  private set { _currentState = value; } }

        public void InitializeStateMachine(State initialState)
        {
            _currentState = initialState;

            _currentState.EnterState();
        }

        public void ChangeState(State newState)
        {
            _currentState.ExitState();
            
            _currentState = newState;

            _currentState.EnterState();
        }
    }
}
