using UnityEngine;
using Player.Abilities;
using Player.States;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement _movement;
        Player.States.PlayerStateManager _playerStateMachine;
        LassoAbility _lassoAbility;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            _playerStateMachine = gameObject.AddComponent<PlayerStateManager>();
            _playerStateMachine.PlayerController = this;
            _playerStateMachine.PlayerMovement = _movement;
            //_lassoAbility = gameObject.AddComponent<LassoAbility>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
