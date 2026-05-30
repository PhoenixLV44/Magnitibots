using UnityEngine;
using Player.Abilities;
using Player.States;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement movement;
        Player.States.PlayerStateManager _playerStateMachine;
        LassoAbility lassoAbility;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            movement = gameObject.AddComponent<Player.Movement>();
            _playerStateMachine = new PlayerStateManager(this, movement);
            //lassoAbility = gameObject.AddComponent<LassoAbility>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
