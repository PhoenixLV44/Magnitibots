using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float moveSpeed;
        public Rigidbody rb;
        Vector3[] submitted;
        InputAction move;
        InputAction look;
        InputAction jump;
        bool movetoggle;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            move = InputSystem.actions.FindAction("Move");
            look = InputSystem.actions.FindAction("Look");
            jump = InputSystem.actions.FindAction("Jump");
            movetoggle = true;
        }
        private void Update()
        {
            submitted = GetInput();
            if (look.IsPressed())
            {
                Look(submitted);
            }
        }
        private void LateUpdate()
        {
            if (movetoggle)
            {

                if (move.IsPressed())
                {
                    Move(submitted);
                }
            }
        }
        public Vector3[] GetInput()
        {
            Vector3 movedir = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
            Vector3 lookdir = new Vector3(look.ReadValue<Vector2>().x/Screen.currentResolution.width-0.5f, 0, look.ReadValue<Vector2>().y/Screen.currentResolution.height-0.5f);

            Vector3[] returnable = { movedir, lookdir };

            return returnable;
        }
        public void Move(Vector3[] input)
        {
            rb.MovePosition(rb.transform.position+input[0]*moveSpeed);
        }
        public void Look(Vector3[] input)
        {
            Debug.Log(input[1]);
            rb.rotation = Quaternion.LookRotation(input[1], Vector3.up);
        }
        public void Jump()
        {
            throw new System.NotImplementedException();
        }
    }
}
