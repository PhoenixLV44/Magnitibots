using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float moveSpeed = 10f;
        public float jumpForce = 10f;
        public Quaternion adjustedMovement;
        public Rigidbody rb;
        Vector3[] _submitted;
        public Vector3[] Submitted { get { return _submitted; } }
        InputAction _move;
        InputAction _look;
        InputAction _jump;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            _move = InputSystem.actions.FindAction("Move");
            _look = InputSystem.actions.FindAction("Look");
            _jump = InputSystem.actions.FindAction("Jump");
        }
        private void Update()
        {
            _submitted = GetInput();
            if (_look.IsPressed())
            {
                Look(_submitted);
            }
        }

        public Vector3[] GetInput()
        {
            Vector3 movedir = new Vector3(_move.ReadValue<Vector2>().x, 0, _move.ReadValue<Vector2>().y);

            movedir = adjustedMovement * movedir;

            Vector3 lookdir = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(_look.ReadValue<Vector2>().x, _look.ReadValue<Vector2>().y,50)), out hit, 50))
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.collider)
                {
                    lookdir = new Vector3(hit.point.x,transform.position.y,hit.point.z) - transform.position;
                }
                else
                {
                    lookdir = new Vector3(_look.ReadValue<Vector2>().x / Screen.currentResolution.width - 0.5f, 0, _look.ReadValue<Vector2>().y / Screen.currentResolution.height - 0.5f);
                }
            }
            

            Vector3[] returnable = { movedir, lookdir };

            return returnable;
        }
        public void Move(Vector3[] input) //Called in MovementState
        {
            rb.MovePosition(rb.transform.position + input[0] * (moveSpeed * Time.deltaTime));
        }
        public void Look(Vector3[] input)
        {
            //Debug.Log(input[1]);
            rb.rotation = Quaternion.LookRotation(input[1], Vector3.up);
        }
        public void Jump()
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }
}
