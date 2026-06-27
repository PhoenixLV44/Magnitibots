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
            
        private bool _lassoHooked = false;
        public bool  LassoHooked { get => _lassoHooked; set => _lassoHooked = value; }
        
        private Player.PCamera _playerCamera;
        public PCamera PlayerCamera { get =>  _playerCamera; set => _playerCamera = value; }
        
        private RangeIndicator _rangeIndicator;
        public RangeIndicator RangeIndicator { get { return _rangeIndicator; } }
        private bool _canUseSmash = false;
        public bool CanUseSmash { get => _canUseSmash; set => _canUseSmash = value; }
        private bool _canUseSuperJump = false;
        public bool CanUseSuperJump { get => _canUseSuperJump; set => _canUseSuperJump = value; }
        
        [SerializeField] private GameObject chargingParticles;
        public GameObject ChargingParticles => chargingParticles;

        [SerializeField] private SuperJumpPoint superJumpPoint;

        private IEnumerator _chargeJump;
        private IEnumerator _spinMerbles;
        bool _jumping = false;
        public bool Jumping => _jumping;

        [SerializeField] private TextMeshProUGUI currentAbilityText;
        
        Animator _animator;
        public Animator Animator => _animator;

        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            _animator = GetComponentInChildren<Animator>();

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
            if (currentAbilityText)
            {
                currentAbilityText.text = _abilityStateManager.StateMachine.CurrentState.ToString();
            }
        }

        public bool jumpLock;
        public void StartJumpChannel()
        {
            if (_canUseSuperJump && _movement.Grounded)
            {
                if (!jumpLock)
                {
                    jumpLock = true;
                    StartCoroutine(_chargeJump);
                    StartCoroutine(CheckForJumpRelease());
                    StartCoroutine(Spin());
                    //StartCoroutine(_spinMerbles);
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
            chargingParticles.SetActive(true);
            _merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

            while (_merbleBoss.ChargedMerbleList.Count <= 10)
            {
                if (!_merbleBoss.ChargedMerbleList.Contains(_merbleBoss.merbleList[0]) && _merbleBoss.ChargedMerbleList.Count < 10)
                {
                    _merbleBoss.merbleList[0].StartCharge(transform.position);
                }
                yield return new WaitForSeconds(0.5f);
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
            StopCoroutine(Spin());
            superJumpPoint.IsCharging = false;
            _movement.Gliding = false;
        }
        IEnumerator Spin()
        {
            yield return new WaitUntil(() => _merbleBoss.ChargedMerbleList.Count > 0);
            while(true)
            {
                for(int i = 0; i < _merbleBoss.ChargedMerbleList.Count; i++)
                {
                    _merbleBoss.ChargedMerbleList[i].transform.position = superJumpPoint.MerblePoints[i].transform.position;
                }
                yield return null;
            }
        }
        public void StopJumpCoroutines()
        {
            StopCoroutine(Spin());
            StopCoroutine(CheckForJumpRelease());
            StopCoroutine(JumpCharging());
            _merbleBoss.FireMerbles();
        }
    }
}
