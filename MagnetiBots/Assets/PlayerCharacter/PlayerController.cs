using UnityEngine;
using Player.States;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement _movement;
        
        Player.States.PlayerStateManager _playerStateManager;
        Ability.StateManager  _abilityStateManager;
        
        Ability.Lasso _lassoAbility;
        public Ability.Lasso LassoAbility { get { return _lassoAbility; } }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            
            _playerStateManager = gameObject.AddComponent<PlayerStateManager>();
            
            _playerStateManager.PlayerController = this;
            _playerStateManager.PlayerMovement = _movement;
            
            _abilityStateManager = gameObject.AddComponent<Ability.StateManager>();
            _abilityStateManager.PlayerController = this;
            //_lassoAbility = gameObject.AddComponent<LassoAbility>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
