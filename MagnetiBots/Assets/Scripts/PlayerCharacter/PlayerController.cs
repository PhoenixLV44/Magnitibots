using UnityEngine;
using Player.States;
using UnityEngine.InputSystem;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement _movement;
        #region Movement Variables
        [SerializeField] float _movementSpeed;
        [SerializeField] float _jumpForce;
        #endregion

        Merbles.Boss _merbleBoss;
        [SerializeField] GameObject _merblePrefab;
        [SerializeField] string _merbleFollowType;

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
            
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();

            _movement.moveSpeed = _movementSpeed;
            _movement.jumpForce = _jumpForce;

            Quaternion _cameraAdjust = Quaternion.Euler(0,FindFirstObjectByType<Player.PCamera>().gameObject.transform.rotation.eulerAngles.y,0);
            _movement.adjustedMovement = _cameraAdjust;

            _merbleBoss = gameObject.AddComponent<Merbles.Boss>();
            _merbleBoss.MerbleFollowType = _merbleFollowType;
            _merbleBoss.merblePrefab = _merblePrefab;
            _merbleBoss.defaultCapacity = 0;
            _merbleBoss.maxSize = 10;

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
            if (InputSystem.actions.FindAction("Jump").IsPressed())
            {
                _merbleBoss.ChargeMerble();
            }
            if (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame())
            {
                _merbleBoss.FireMerbles();
            }
        }
    }
}
