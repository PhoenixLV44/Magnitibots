using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement movement;
        [SerializeField] float moveSpeed;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            movement = gameObject.AddComponent<Player.Movement>();
            movement.moveSpeed = moveSpeed;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
