using UnityEngine;

namespace Player
{
    public class Camera : MonoBehaviour
    {
        GameObject player;
        Vector3 offset;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = GameObject.FindFirstObjectByType<Player.Controller>().gameObject;
            offset = transform.position - player.transform.position;
        }
        private void Update()
        {
            transform.position = player.transform.position + offset;
        }
    }
}
