using System;
using System.Collections;
using UnityEngine;
using Ability;
using UnityEngine.InputSystem;

namespace Player
{
    public class AnimController : MonoBehaviour
    {
        private Controller _playerController;
        private Movement _playerMovement;
        
        private Player.StateManager _playerStateManager;
        private Ability.StateManager _abilityStateManager;
        
        private Lasso _lasso;
        private Smash _smash;
        private SuperJump _superJump;
        
        private Animator _animator;

        private float _walkBlendTree;
        
        InputAction _move;

        private float _jumpAnimLength;
        public float JumpAnimLength => _jumpAnimLength;
        [SerializeField] private AnimationClip jumpAnimation; //Player_Jump2
        public AnimationClip JumpAnimation => jumpAnimation;

        private float _pullAnimLength;
        public float PullAnimLength => _pullAnimLength;
        [Tooltip("Animation called 'Pull_Full'")]
        [SerializeField] private AnimationClip pullAnimation;
        public AnimationClip PullAnimation => pullAnimation;
        
        private float _pullLeverAnimLength;
        [Tooltip("Animation called 'Pull_Mid'")]
        [SerializeField] private AnimationClip pullLeverAnimation; //Player_Pull_Mid
        public AnimationClip PullLeverAnimation => pullLeverAnimation;

        private bool _charging;
        public bool Charging {get => _charging; set => _charging = value;}
        

        public void SetUpController(Controller playerController, Movement playerMovement, Player.StateManager playerStateManager, Ability.StateManager abilityStateManager, Lasso lasso, Smash smash,
            SuperJump superJump,  Animator animator)
        {
            _playerController = playerController;
            _playerMovement = playerMovement;
            _playerStateManager = playerStateManager;
            _lasso = lasso;
            _smash = smash;
            _superJump = superJump;
            _abilityStateManager = abilityStateManager;
            _animator = animator;
            
            _move = InputSystem.actions.FindAction("Move");
        }
        
        private void Update()
        {
            if (_playerMovement.Grounded)
            {
                ChangeWalkBlendTree();
            }
            else
            {
                //
            }
            
            _animator.SetBool("Hovering", _playerMovement.Hovering);

            switch (_abilityStateManager.CurrentAbility)
            {
                case Lasso:
                    _animator.SetBool("Lasso", true);
                    _animator.SetBool("Smash", false);
                    _animator.SetBool("SuperJump", false);
                    break;
                case Smash:
                    _animator.SetBool("Smash", true);
                    _animator.SetBool("Lasso", false);
                    _animator.SetBool("SuperJump", false);
                    break;
                case SuperJump:
                    _animator.SetBool("SuperJump", true);
                    _animator.SetBool("Lasso", false);
                    _animator.SetBool("Smash", false);
                    if(_playerController.MerbleBoss.ChargedMerbleList.Count >= 5)
                        _playerMovement.Hovering = true;
                    break;
                default:
                    _animator.SetBool("Lasso", false);
                    _animator.SetBool("Smash", false);
                    _animator.SetBool("SuperJump", false);
                    break;
            }

            if (_playerController.LassoHooked)
            {
                _animator.SetBool("LassoHooked", true);
            }
            else
            {
                _animator.SetBool("LassoHooked", false);
            }
            _animator.SetBool("Charging",  _charging);
        }

        private void ChangeWalkBlendTree()
        {
            if (_move.IsPressed())
            {
                if (_walkBlendTree < 1)
                {
                    _walkBlendTree += Time.deltaTime;
                    _walkBlendTree = Mathf.Clamp(_walkBlendTree, 0, 1);
                }
            }
            else
            {
                if (_walkBlendTree > 0)
                {
                    _walkBlendTree -= Time.deltaTime;
                    _walkBlendTree = Mathf.Clamp(_walkBlendTree, 0, 1);
                }
            }
            _animator.SetFloat("IdleToWalk" , _walkBlendTree);
        }

        private void Start()
        {
            _jumpAnimLength = jumpAnimation.length / 4;
            _pullLeverAnimLength = pullLeverAnimation.length;
            _pullAnimLength = pullAnimation.length;
        }

        public IEnumerator PullingLeverAnim()
        {
            Debug.Log("Pulling lever");
            _animator.SetBool("PullingLever", _playerController.Interacting);
            yield return new WaitForSeconds(1);
            _playerController.Interacting = false;
            _animator.SetBool("PullingLever", _playerController.Interacting);
        }
    }
}
