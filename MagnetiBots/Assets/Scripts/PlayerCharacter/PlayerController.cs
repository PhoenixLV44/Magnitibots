using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Ability.Object;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement _movement;
        #region Movement Variables
        [SerializeField] float movementSpeed;
        [SerializeField] float jumpForce;
        #endregion

        Merbles.Boss _merbleBoss;
        [SerializeField] GameObject merblePrefab;
        [SerializeField] string merbleFollowType;
        public Movement Movement { get { return _movement; } }

        #region Scripts
        private Ability.Lasso _lassoAbility;
        public Ability.Lasso LassoAbility { get { return _lassoAbility; } }
        
        private Ability.Smash _smashAbility;
        public Ability.Smash SmashAbility { get { return _smashAbility; } }
        
        private Ability.Propeller _propellerAbility;
        public Ability.Propeller PropellerAbility { get { return _propellerAbility; } }
        
        private TargetingCursor _targetCursorScript;
        public TargetingCursor TargetCursorScript { get { return _targetCursorScript; } }
        
        private GameObject _targetCursorObject;
        public GameObject TargetCursorObject => _targetCursorObject;
            #endregion

        #region States
        private Player.StateManager _playerStateManager;
        public Player.StateManager PlayerStateManager => _playerStateManager; 
        Ability.StateManager  _abilityStateManager;
        public Ability.StateManager AbilityStateManager => _abilityStateManager;
            #endregion
            
        private bool _lassoHooked = false;
        public bool  LassoHooked { get => _lassoHooked; set => _lassoHooked = value; }
        
        private RangeIndicator _rangeIndicator;
        public RangeIndicator RangeIndicator { get { return _rangeIndicator; } }
            
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();

            _movement.moveSpeed = movementSpeed;
            _movement.jumpForce = jumpForce;

            Quaternion _cameraAdjust = Quaternion.Euler(0,FindFirstObjectByType<Player.PCamera>().gameObject.transform.rotation.eulerAngles.y,0);
            _movement.adjustedMovement = _cameraAdjust;

            _merbleBoss = gameObject.AddComponent<Merbles.Boss>();
            _merbleBoss.MerbleFollowType = merbleFollowType;
            _merbleBoss.merblePrefab = merblePrefab;
            _merbleBoss.defaultCapacity = 0;
            _merbleBoss.maxSize = 10;
            
            _targetCursorScript = gameObject.AddComponent<TargetingCursor>();
            
            _playerStateManager = gameObject.AddComponent<Player.StateManager>();
            _abilityStateManager = gameObject.AddComponent<Ability.StateManager>();
            
            _playerStateManager.PlayerController = this;
            
            _playerStateManager.PlayerMovement = _movement;
            
            _abilityStateManager.PlayerController = this;
            
            _rangeIndicator = gameObject.AddComponent<RangeIndicator>();
            
            _targetCursorObject = transform.Find("Target Cursor").gameObject;

        }

        // Update is called once per frame
        void Update()
        {
            if (InputSystem.actions.FindAction("Charge").triggered)
            {
                StartCoroutine(ChannelingMerbles(Vector3.zero));
            }
            

        }
        IEnumerator ChannelingMerbles(Vector3 target)
        {
            while (_merbleBoss.chargedMerbles < _merbleBoss.currentMerbles)
            {
                if (!InputSystem.actions.FindAction("Charge").IsPressed())
                {
                    _merbleBoss.FireMerbles();
                    break;
                }
                _merbleBoss.ChargeMerble(target);
                yield return new WaitForSeconds(1);
            }
            yield return new WaitUntil(() => (!InputSystem.actions.FindAction("Charge").IsPressed()));
            _merbleBoss.FireMerbles();
        }
    }
}
