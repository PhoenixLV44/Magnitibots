using UnityEngine;

namespace Player.States
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState { get { return CurrentState; } private set { CurrentState = value; } }

        public void InitializeStateMachine(PlayerState initialState)
        {
            CurrentState = initialState;

            CurrentState.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState.ExitState();
            
            CurrentState = newState;

            CurrentState.EnterState();
        }
    }
}
