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
        
        [SerializeField] private GameObject chargingParticles;
        public GameObject ChargingParticles => chargingParticles;

        [SerializeField] private SuperJumpPoint superJumpPoint;

        private IEnumerator _chargeJump;
        private IEnumerator _spinMerbles;
        bool _jumping = false;
        public bool Jumping => _jumping;

        [SerializeField] private TextMeshProUGUI currentAbilityText;

        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();

            _groundChecker = gameObject.AddComponent<Player.GroundChecker>();
            _groundChecker.checkerMask = groundLayers;
            _groundChecker.movement = _movement;

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
            
            chargingParticles.SetActive(false);

            _chargeJump = JumpCharging();
            _spinMerbles = superJumpPoint.MoveMerblesCoroutine;

            superJumpPoint.PlayerController = this;
            superJumpPoint.MerbleBoss = _merbleBoss;
            
            Respawner respawner = GetComponent<Respawner>();
            respawner.Movement = _movement;
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
            currentAbilityText.text = _abilityStateManager.StateMachine.CurrentState.ToString();
        }

        public bool jumpLock;
        public void StartJumpChannel()
        {
            if (_canUsePropeller && _movement.Grounded)
            {
                if (!jumpLock)
                {
                    jumpLock = true;
                    StartCoroutine(_chargeJump);
                    StartCoroutine(CheckForJumpRelease());
                    StartCoroutine(_spinMerbles);
                }
            }
            else
            {
                if(_movement.Grounded)
               {
                    if (!jumpLock)
                    {
                        jumpLock = true;
                        StartCoroutine(BaseJump());
                    }
               }
            }
        }
        IEnumerator BaseJump()
        {
            _merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            yield return new WaitUntil(() => (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame()));
            _movement.Jump(0);
        }
        IEnumerator JumpCharging()
        {
            superJumpPoint.IsCharging = true;
            chargingParticles.SetActive(true);
            //StartCoroutine(_spinMerbles);
            //Debug.Log("jump charging");
            _merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

            while (_merbleBoss.ChargedMerbleList.Count <= 10)
            {
                //_merbleBoss.CheckForDuplicates();
                /*if (!InputSystem.actions.FindAction("Jump").IsPressed())
                {
                    _movement.Jump(_merbleBoss.ChargedMerbleList.Count);
                    if(_merbleBoss.ChargedMerbleList.Count>0)
                    {
                        _movement.Gliding = true;
                        yield return new WaitUntil(() => !_movement.Grounded);
                        Debug.Log("gliding");
                        yield return new WaitUntil(() => _movement.Grounded);
                        _movement.Gliding = false;
                    }
                    _merbleBoss.FireMerbles();
                    break;
                }*/

                if (!_merbleBoss.ChargedMerbleList.Contains(_merbleBoss.merbleList[0]) && _merbleBoss.ChargedMerbleList.Count < 10)
                {
                    _merbleBoss.merbleList[0].StartCharge(transform.position);
                }
                yield return new WaitForSeconds(0.5f);
                //_merbleBoss.ChargeMerble(transform.position);
            }
        }
        IEnumerator CheckForJumpRelease()
        {
            yield return new WaitUntil(() =>  (InputSystem.actions.FindAction("Jump").WasReleasedThisFrame()));
            int jumpPowerMult = _merbleBoss.ChargedMerbleList.Count;
            
            StopCoroutine(_chargeJump);
            chargingParticles.SetActive(false);
            
            _movement.Jump(jumpPowerMult);
        
            yield return new WaitUntil(() => !_movement.Grounded);
            if (jumpPowerMult > 3)
            {
                _movement.Gliding = true;
                Debug.Log("GLIDING");
            }
            else
            {
                _movement.Gliding = false;
            }

                yield return new WaitUntil(() => _movement.Grounded);
            Debug.Log("BEE BOOP");
            _merbleBoss.FireMerbles();
            foreach (var merble in _merbleBoss.MasterList)
            {
                merble.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            }
            StopCoroutine(_spinMerbles);
            superJumpPoint.IsCharging = false;
            _movement.Gliding = false;
        }
    }
}
