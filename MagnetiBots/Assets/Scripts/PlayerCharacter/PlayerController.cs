using UnityEngine;
using Player.States;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement _movement;

        #region Abilities
        [SerializeField] Ability.Lasso _lassoAbility;
        public Ability.Lasso LassoAbility { get { return _lassoAbility; } }
        
        [SerializeField] Ability.Smash _smashAbility;
        public Ability.Smash SmashAbility { get { return _smashAbility; } }
        
        [SerializeField] Ability.Propeller _propellerAbility;
        public Ability.Propeller PropellerAbility { get { return _propellerAbility; } }
            #endregion

        #region States
        Player.States.PlayerStateManager _playerStateManager;
        Ability.StateManager  _abilityStateManager;
            #endregion
            
        private bool _lassoHooked = false;
        public bool  LassoHooked { get => _lassoHooked; set => _lassoHooked = value; }
            
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            Quaternion _cameraAdjust = Quaternion.Euler(0,FindFirstObjectByType<Player.Camera>().gameObject.transform.rotation.eulerAngles.y,0);
            _movement.adjustedMovement = _cameraAdjust;
            /*
            _lassoAbility = gameObject.AddComponent<Ability.Lasso>();
            _smashAbility = gameObject.AddComponent<Ability.Smash>();
            _propellerAbility = gameObject.AddComponent<Ability.Propeller>();
            */
            _playerStateManager = gameObject.AddComponent<PlayerStateManager>();
            
            _playerStateManager.PlayerController = this;
            _playerStateManager.PlayerMovement = _movement;
            
            _abilityStateManager = gameObject.AddComponent<Ability.StateManager>();
            _abilityStateManager.PlayerController = this;
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
