using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public Transform model;
        public float moveSpeed = 10f;
        public float jumpForce = 10f;
        float _glidingSpeed=-1f;
        bool _gravityOn;
        private CharacterController cc;
        private Vector3 currentVelocity;

        private float _defaultMoveSpeed = 10f;
        public float DefaultMoveSpeed  => _defaultMoveSpeed;
        private float _velocityCap = 20f;
        public Quaternion adjustedMovement;
        public Rigidbody rb;
        Vector3[] _submitted;
        public Vector3[] Submitted { get { return _submitted; } }
        bool _isGliding;
        public bool Gliding { get { return _isGliding; } set { _isGliding = value; } }
        bool _isGrounded;
        public bool Grounded { get { return _isGrounded; } set { _isGrounded = value; } }
        InputAction _move;
        InputAction _look;
        InputAction _jump;
        
        private Player.Controller _controller;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            model = gameObject.transform.Find("PlayerModel");
            _move = InputSystem.actions.FindAction("Move");
            _look = InputSystem.actions.FindAction("Look");
            _jump = InputSystem.actions.FindAction("Jump");
            _controller = GetComponent<Player.Controller>();
            _isGrounded= true;
            _gravityOn = true;
        }
        private void Update()
        {
            _submitted = GetInput();
            if (_isGliding)
            {
                if(rb.linearVelocity.y < _glidingSpeed)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x,_glidingSpeed,rb.linearVelocity.z);
                }
            }
        }

        public Vector3[] GetInput()
        {
            Vector3 movedir = new Vector3(_move.ReadValue<Vector2>().x, 0, _move.ReadValue<Vector2>().y);

            movedir = adjustedMovement * movedir;

            Vector3 lookdir = new Vector3(_look.ReadValue<Vector2>().x/Screen.width-0.5f, 0, _look.ReadValue<Vector2>().y/Screen.height-0.5f);

            Vector3[] returnable = { movedir, lookdir };
            if (InputSystem.actions.FindAction("Jump").IsPressed())
            {
                _controller.StartJumpChannel();
            }

            return returnable;
        }
        /// <summary>
        /// Called in MovementState and LoopedHookState
        /// Call with Submitted[0]
        /// </summary>
        public void Move(Vector3 input)
        {
            if(rb.linearVelocity.magnitude < _velocityCap)
            {
                rb.linearVelocity += input * (moveSpeed * Time.deltaTime);
            }
        }
        public void CheckDecelerate()
        {

        }
        /// <summary>
        /// Called in every player state currently implemented
        /// Called with Submitted[1]
        /// </summary>
        public void Look(Vector3 input)
        {
            //Debug.Log(input[1]);
            
            if(_controller.TargetCursorObject.activeSelf)
            {
                Vector3 lookTarget = _controller.TargetCursorObject.transform.position;
                lookTarget.y = transform.position.y;
                model.LookAt(lookTarget);
            }
            else
            {
                input = adjustedMovement * input;
                model.rotation = Quaternion.LookRotation(input, Vector3.up);
            }
        }
        public void Jump(int jumpModifier)
        {
            float jumpPower = jumpForce + (jumpForce * Mathf.Log(jumpModifier+1));
            Debug.Log("jumping with power "+jumpPower);
            StartCoroutine(JumpAccelerated(jumpPower * Vector3.up * Time.deltaTime));
        }
        IEnumerator JumpAccelerated(Vector3 jumpTarget)
        {
            Vector3 jumpVelocity;
            jumpVelocity = jumpTarget;
            _gravityOn = false;
            do
            {
                Debug.Log(jumpVelocity.y - jumpTarget.y);
                jumpVelocity.y = Mathf.Lerp(jumpTarget.y, 0, 0.5f*9.8f*Time.deltaTime);
                cc.Move(jumpVelocity * Time.deltaTime);
                jumpTarget = cc.velocity;
            } while (jumpVelocity.y > 0.01f);
            _controller.jumpLock = false;
            _gravityOn = true;
            yield return null;
        }
        public void Gravity()
        {
            if (_gravityOn)
            {
                cc.Move(Physics.gravity * Time.deltaTime);
            }
        }
    }
}
