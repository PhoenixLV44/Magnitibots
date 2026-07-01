using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Ability.Object;
using TMPro;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Movement Variables
        Player.Movement _movement;
        Player.GroundChecker _groundChecker;
        public LayerMask groundLayers;
        [SerializeField] float movementSpeed;
        [SerializeField] float jumpForce;
        #endregion

        #region Merbles
        Merbles.Boss _merbleBoss;
        public Merbles.Boss MerbleBoss { get { return _merbleBoss; } }
        [SerializeField] GameObject merblePrefab;
        [SerializeField] Merbles.Merble.FollowTypes merbleFollowType;
        public Movement Movement { get { return _movement; } }
        #endregion

        #region Scripts

        private Ability.Lasso _lassoAbility;
        public Ability.Lasso LassoAbility { get { return _lassoAbility; } }
        
        private Ability.Smash _smashAbility;
        public Ability.Smash SmashAbility { get { return _smashAbility; } }
        
        private Ability.SuperJump _superJumpAbility;
        public Ability.SuperJump SuperJumpAbility { get { return _superJumpAbility; } }
        
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
            
        [SerializeField]private bool _lassoHooked = false;
        public bool  LassoHooked { get => _lassoHooked; set => _lassoHooked = value; }
        
        private Player.PCamera _playerCamera;
        public PCamera PlayerCamera { get =>  _playerCamera; set => _playerCamera = value; }
        
        private RangeIndicator _rangeIndicator;
        public RangeIndicator RangeIndicator { get { return _rangeIndicator; } }
        [SerializeField] private bool canUseLasso;
        public bool CanUseLasso { get => canUseLasso; set => canUseLasso = value; }
        
        [SerializeField] private bool canUseSmash = false;
        public bool CanUseSmash { get => canUseSmash; set => canUseSmash = value; }
        [SerializeField] private bool canUseSuperJump = false;
        public bool CanUseSuperJump { get => canUseSuperJump; set => canUseSuperJump = value; }
        
        [SerializeField] private GameObject chargingParticles;
        public GameObject ChargingParticles => chargingParticles;

        [SerializeField] private SuperJumpPoint superJumpPoint;
        
        Animator _animator;
        public Animator Animator => _animator;
        AnimController _animController;
        public AnimController AnimController => _animController;

        [SerializeField] private GameObject shadow;

        private bool _interacting;
        public bool Interacting { get => _interacting; set => _interacting = value; }

        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            _animator = GetComponent<Animator>();

            /*_groundChecker = gameObject.AddComponent<Player.GroundChecker>();
            _groundChecker.groundMask = groundLayers;
            _groundChecker.movement = _movement;*/

            _movement.DefaultMoveSpeed = movementSpeed;
            _movement.JumpForce = jumpForce;
            //wdDebug.Log("Default Move Speed: " + _movement.DefaultMoveSpeed);

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
            
            chargingParticles.SetActive(false);

            superJumpPoint.PlayerController = this;
            superJumpPoint.Movement = _movement;
            superJumpPoint.MerbleBoss = _merbleBoss;

            _animController = GetComponent<AnimController>();
            _animController.SetUpController(this, _movement, _playerStateManager, _abilityStateManager, GetComponent<Ability.Lasso>(), GetComponent<Ability.Smash>(),  GetComponent<Ability.SuperJump>(), _animator);
            
            Respawner respawner = GetComponent<Respawner>();
            respawner.Movement = _movement;
            
            shadow = transform.GetChild(transform.childCount - 1).gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (InputSystem.actions.FindAction("Charge").triggered)
            {
                //StartCoroutine(ChannelingMerbles(transform.position));
                //_merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            }
            _movement.adjustedMovement = Quaternion.Euler(0,_playerCamera.PivotPoint.transform.localEulerAngles.y,0);
        }
        void FixedUpdate()
        {
            if (_movement.CharacterController)
            {
                _movement.HandleMovement();
            }
            CheckForGround();
            MoveShadow();
        }
        private void CheckForGround()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.5f, -Vector3.up, out hit, 0.75f, groundLayers))
            {
                //Debug.Log("cast did find ground");
                _movement.Grounded = true;
            }
            else
            {
                //Debug.Log("cast did not find ground");
                if(_movement)
                {
                    _movement.Grounded = false;
                }
            }
        }
        private void MoveShadow()
        {
            RaycastHit hit;
            if (_movement && shadow)
            {
                if (_movement.Grounded)
                {
                    shadow.SetActive(false);
                }
                else
                {
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundLayers))
                    {
                        //Debug.Log("activate shadow");
                        Vector3 point = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                        shadow.SetActive(true);
                        shadow.transform.position = point;
                    }
                }
            }
            else
            {
                if (!_movement)
                {
                    Debug.Log("no movement");
                }

                if (!shadow)
                {
                    Debug.Log("no shadow");
                }
            }
        }

        public void UnlockNewAbility()
        {
            if (!canUseLasso)
            {
                Debug.Log("can use lasso");
                canUseLasso = true;
            }
            else if (!canUseSmash)
            {
                Debug.Log("can use smash");
                canUseSmash = true;
            }
            else if (!canUseSuperJump)
            {
                Debug.Log("can use super jump");
                canUseSuperJump = true;
            }
            _animator.Play("Collect");
        }
    }
} 
