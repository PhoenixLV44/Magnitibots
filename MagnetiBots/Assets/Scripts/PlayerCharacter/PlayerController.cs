using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Ability.Object;
using static UnityEngine.GraphicsBuffer;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Movement Variables
        Player.Movement _movement;
        [SerializeField] float movementSpeed;
        [SerializeField] float jumpForce;
        #endregion

        #region Merbles
        Merbles.Boss _merbleBoss;
        public Merbles.Boss MerbleBoss { get { return _merbleBoss; } }
        [SerializeField] GameObject merblePrefab;
        [SerializeField] string merbleFollowType;
        public Movement Movement { get { return _movement; } }
        #endregion

        #region Scripts
        private GroundChecker _groundChecker;

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
        
        private Player.PCamera _playerCamera;
        public PCamera PlayerCamera { get =>  _playerCamera; set => _playerCamera = value; }
        
        private RangeIndicator _rangeIndicator;
        public RangeIndicator RangeIndicator { get { return _rangeIndicator; } }
        private bool _canUseSmash = false;
        public bool CanUseSmash { get => _canUseSmash; set => _canUseSmash = value; }
        private bool _canUsePropeller = false;
        public bool CanUsePropeller { get => _canUsePropeller; set => _canUsePropeller = value; }
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            GetComponentInChildren<GroundChecker>().movement = _movement;

            _movement.moveSpeed = movementSpeed;
            _movement.jumpForce = jumpForce;

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
                StartCoroutine(ChannelingMerbles(transform.position));
            }
            _movement.adjustedMovement = Quaternion.Euler(0,_playerCamera.PivotPoint.transform.localEulerAngles.y,0);;
        }
        IEnumerator ChannelingMerbles(Vector3 target)
        {
            _merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, target).CompareTo(Vector3.Distance(b.transform.position, target)));
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
        private bool jumpLock;
        public void StartJumpChannel()
        {
            if (_canUsePropeller && _movement.Grounded)
            {
                if (!jumpLock)
                {
                    jumpLock = true;
                    StartCoroutine(JumpChanneling());
                    
                }
            }
            else
            {
               if(_movement.grounded)
               {
                _movement.Jump(1);
               }
            }
        }
        IEnumerator JumpChanneling()
        {
            {
                Debug.Log("jump charging");
                _merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                while (_merbleBoss.chargedMerbles < _merbleBoss.currentMerbles)
                {
                    if (!InputSystem.actions.FindAction("Jump").IsPressed())
                    {
                        _movement.Jump(_merbleBoss.chargedMerbles);
                        if(_merbleBoss.chargedMerbles>0)
                        {
                            _movement.Gliding = true;
                            yield return new WaitUntil(() => !_movement.Grounded);
                            Debug.Log("gliding");
                            yield return new WaitUntil(() => _movement.Grounded);
                            _movement.Gliding = false;
                        }
                        _merbleBoss.FireMerbles();
                        jumpLock = false;
                        break;
                    }
                    _merbleBoss.ChargeMerble(transform.position);
                    yield return new WaitForSeconds(1);
                }
                yield return new WaitUntil(() => (!InputSystem.actions.FindAction("Jump").IsPressed()));
                _movement.Jump(_merbleBoss.chargedMerbles);
                jumpLock = false;
                if (_merbleBoss.chargedMerbles > 0)
                {
                    _movement.Gliding = true;
                    yield return new WaitUntil(() => !_movement.Grounded);
                    Debug.Log("gliding");
                    yield return new WaitUntil(() => _movement.Grounded);
                    _movement.Gliding = false;
                }
                _merbleBoss.FireMerbles();
            }
        }
    }
}
