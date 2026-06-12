using UnityEngine;

namespace Player {
    public class GroundChecker : MonoBehaviour
    {
        public Player.Movement movement;
        public LayerMask checkerMask;

        private void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.SphereCast((transform.position), 0.5f, -Vector3.up, out hit,0.75f,checkerMask))
            {
                Debug.Log("cast did find ground");
                movement.Grounded = true;
            }
            else
            {
                Debug.Log("cast did not find ground");
                movement.Grounded = false;
            }
        }
    }
}
