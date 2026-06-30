using UnityEngine;
namespace Player
{
    public class GroundChecker : MonoBehaviour
    {
        public Player.Movement movement;
        public LayerMask groundMask;
        [SerializeField] private GameObject shadow;
        private void FixedUpdate()
        {
            if (!movement)
            {
                movement = transform.parent.GetComponent<Movement>();
            }

            if (!shadow)
            {
                shadow = transform.parent.GetChild(7).gameObject;
            }
            CheckForGround();
            MoveShadow();
        }

        private void CheckForGround()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.5f, -Vector3.up, out hit, 0.75f, groundMask))
            {
                ///Debug.Log("cast did find ground");
                movement.Grounded = true;
            }
            else
            {
                //Debug.Log("cast did not find ground");
                if(movement)
                {
                    movement.Grounded = false;
                }
            }
        }
        private void MoveShadow()
        {
            RaycastHit hit;
            if (movement && shadow)
            {
                if (movement.Grounded)
                {
                    shadow.SetActive(false);
                }
                else
                {
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundMask))
                    {
                        Debug.Log("activate shadow");
                        Vector3 point = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                        shadow.SetActive(true);
                        shadow.transform.position = point;
                    }
                }
            }
            else
            {
                if (!movement)
                {
                    Debug.Log("no movement");
                }

                if (!shadow)
                {
                    Debug.Log("no shadow");
                }
            }
        }
    }
}
