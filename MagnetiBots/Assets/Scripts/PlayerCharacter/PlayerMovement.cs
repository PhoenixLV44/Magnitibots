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
        float _glidingSpeed = 0.25f;
        bool _gravityOn;
        private CharacterController _characterController;
        public CharacterController CharacterController => _characterController;
        private Vector3 _currentVelocity;
        private float _playerMass = 5f;
        public float maxMoveSpeed = 10f;

        private float _defaultMoveSpeed = 10f;
        public float DefaultMoveSpeed => _defaultMoveSpeed;
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
            _characterController = GetComponent<CharacterController>();
            model = gameObject.transform.Find("PlayerModel");
            _move = InputSystem.actions.FindAction("Move");
            _look = InputSystem.actions.FindAction("Look");
            _jump = InputSystem.actions.FindAction("Jump");
            _controller = GetComponent<Player.Controller>();
            _isGrounded = true;
            _gravityOn = true;
        }
        private void Update()
        {
            _submitted = GetInput();
        }

        public Vector3[] GetInput()
        {
            Vector3 movedir = new Vector3(_move.ReadValue<Vector2>().x, 0, _move.ReadValue<Vector2>().y);

            movedir = adjustedMovement * movedir;

            Vector3 lookdir = new Vector3(_look.ReadValue<Vector2>().x / Screen.width - 0.5f, 0, _look.ReadValue<Vector2>().y / Screen.height - 0.5f);

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
            Vector3 targetVelocity = input * moveSpeed;
            /*
            currentVelocity = cc.velocity;

            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, 40 * Time.deltaTime);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, targetVelocity.z, 40 * Time.deltaTime);
            cc.Move(currentVelocity * Time.deltaTime);
            */
            _submittedMovement = targetVelocity;
        }
        /// <summary>
        /// Called in every player state currently implemented
        /// Called with Submitted[1]
        /// </summary>
        public void Look(Vector3 input)
        {
            //Debug.Log(input[1]);

            if (_controller.TargetCursorObject.activeSelf)
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
        
        float _submittedJump = 0;
        public void Jump(int jumpModifier = 0)
        {
            float jumpPower = jumpModifier == 0? jumpForce: jumpForce + (jumpForce * Mathf.Log(jumpModifier));
            //jumpPower = jumpForce + (1 * jumpModifier);
            //Debug.Log("jumping with power " + jumpPower);
            _submittedJump = jumpPower;
        }
        
        /// <summary>
        /// Calculate the vertical motion of the character at once, rather than applying two moves seperately. Combined in HandleMovement().
        /// </summary>
        /// <returns>A Vector3 representing the intended vertical velocity.</returns>
        public Vector3 VerticalMotion()
        {
            //calculate forces
            float intendedVerticalForce = 0;
            //the force of gravity
            if (_gravityOn && !Grounded)
            {
                intendedVerticalForce -= Physics.gravity.magnitude;
            }
            //the force of glide
            else if (_isGliding && !Grounded)
            {
                intendedVerticalForce -= _glidingSpeed;
            }
            else if(Grounded)
            {
                _gravityOn = true;
            }
            //the force of jump
            if (_submittedJump != 0)
            {
                intendedVerticalForce += _submittedJump;
                _submittedJump = 0;
                _controller.jumpLock = false;
            }

            //calculate acceleration from force
            Vector3 intendedVerticalAcceleration = Vector3.zero;
            intendedVerticalAcceleration.y += intendedVerticalForce/_playerMass;

            //calculate speed from acceleration
            Vector3 intendedVerticalSpeed = new Vector3(0,_characterController.velocity.y,0);
            intendedVerticalSpeed += intendedVerticalAcceleration;
/*
            Debug.Log(intendedVerticalSpeed);
            Debug.Log(_gravityOn);
            Debug.Log(Grounded);*/
            //move a distance based on the speed
            return intendedVerticalSpeed;
        }
        
        Vector3 _submittedMovement;
        /// <summary>
        /// Calculate the horizontal movement of the character at once, instead of doing Move and Friction seperately. Combined in HandleMovement()
        /// </summary>
        /// <returns>A Vector3 representing the total horizontal movement of the character.</returns>
        public Vector3 HorizontalMotion()
        {
            //calculate forces
            Vector3 intendedHorizontalForce = Vector3.zero;
            Vector3 ccHorizontal = new Vector3(_characterController.velocity.x,0,_characterController.velocity.z);

            //the force of movement
            if (_submittedMovement != Vector3.zero) 
            {
                intendedHorizontalForce += _submittedMovement;
                _submittedMovement = Vector3.zero;
            }

            //the force of friction
            if (ccHorizontal.x != 0 || ccHorizontal.z != 0)
            {
                //direction
                Vector3 frictionalForce = -Vector3.Normalize(ccHorizontal);
                //magnitude
                frictionalForce *= (0.45f * Physics.gravity.magnitude);
                if (frictionalForce.magnitude > ccHorizontal.magnitude)
                {
                    frictionalForce = Vector3.ClampMagnitude(frictionalForce, ccHorizontal.magnitude);
                }
                intendedHorizontalForce += frictionalForce;
            }

            //calculate acceleration from force
            Vector3 intendedHorizontalAcceleration = Vector3.zero;
            intendedHorizontalAcceleration += intendedHorizontalForce / _playerMass;

            //calculate speed from acceleration
            Vector3 intendedHorizontalSpeed = ccHorizontal;
            intendedHorizontalSpeed += intendedHorizontalAcceleration;

            //clamp horizontal speed
            intendedHorizontalSpeed = Vector3.ClampMagnitude(intendedHorizontalSpeed, maxMoveSpeed);

            //move a distance based on the speed
            return intendedHorizontalSpeed;
        }
        /// <summary>
        /// The function that combines movement behaviors into a single move. Called in FixedUpdate of PlayerController.
        /// </summary>
        public void HandleMovement()
        {
            Vector3 intendedTotalMovement = HorizontalMotion() + VerticalMotion();
            //Debug.Log(intendedTotalMovement);
            Vector3 intendedTotalDistance = intendedTotalMovement * Time.deltaTime;
            _characterController.Move(intendedTotalDistance);
            if (intendedTotalDistance.y < 0 && _isGliding)
            {
                _gravityOn = false;
            }
        }
    }
}
