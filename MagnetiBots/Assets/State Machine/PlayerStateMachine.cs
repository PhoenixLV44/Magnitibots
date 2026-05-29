using UnityEngine;

namespace Player
{
    public class PlayerStateMachine
    {
        public PlayerState currentState { get { return currentState; } private set { currentState = value; } }

        public void InitializeStateMachine(Player.PlayerState initialState)
        {
            currentState = initialState;

            currentState.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.ExitState();
            
            currentState = newState;

            currentState.EnterState();
        }
    }
}
