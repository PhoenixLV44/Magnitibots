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
        float _glidingSpeed = 1f;
        bool _gravityOn;
        private CharacterController cc;
        private Vector3 currentVelocity;
        private float playerMass = 5f;
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
            cc = GetComponent<CharacterController>();
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
            submittedMovement = targetVelocity;
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
        public void Jump(int jumpModifier)
        {
            float jumpPower = jumpForce + (jumpForce * Mathf.Log(jumpModifier + 1));
            Debug.Log("jumping with power " + jumpPower);
            submittedJump = jumpPower;
        }
        float submittedJump = 0;
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
            if (_isGliding && !Grounded)
            {
                intendedVerticalForce -= _glidingSpeed;
            }
            //the force of jump
            if (submittedJump != 0)
            {
                intendedVerticalForce += submittedJump;
                submittedJump = 0;
                _controller.jumpLock = false;
            }

            //calculate acceleration from force
            Vector3 intendedVerticalAcceleration = Vector3.zero;
            intendedVerticalAcceleration.y += intendedVerticalForce/playerMass;

            //calculate speed from acceleration
            Vector3 intendedVerticalSpeed = new Vector3(0,cc.velocity.y,0);
            intendedVerticalSpeed += intendedVerticalAcceleration;

            Debug.Log(intendedVerticalSpeed);
            Debug.Log(_gravityOn);
            Debug.Log(Grounded);
            //move a distance based on the speed
            return intendedVerticalSpeed;
        }
        Vector3 submittedMovement;
        /// <summary>
        /// Calculate the horizontal movement of the character at once, instead of doing Move and Friction seperately. Combined in HandleMovement()
        /// </summary>
        /// <returns>A Vector3 representing the total horizontal movement of the character.</returns>
        public Vector3 HorizontalMotion()
        {
            //calculate forces
            Vector3 intendedHorizontalForce = Vector3.zero;
            Vector3 ccHorizontal = new Vector3(cc.velocity.x,0,cc.velocity.z);

            //the force of movement
            if (submittedMovement != Vector3.zero) 
            {
                intendedHorizontalForce += submittedMovement;
                submittedMovement = Vector3.zero;
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
            intendedHorizontalAcceleration += intendedHorizontalForce / playerMass;

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
            cc.Move(intendedTotalMovement * Time.deltaTime);
        }
    }
}
