using System;
using Merbles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Respawner : MonoBehaviour
    {
        private InputAction _respawnInput;
        [SerializeField] Vector3 _respawnPosition;
        public Vector3 RespawnPosition { get { return _respawnPosition; } set { _respawnPosition = value; } }
        Controller _playerController;
        Movement _movement;
        Merbles.Boss _boss;
        
        [SerializeField] LayerMask respawnMask;

        private void Start()
        {
            _respawnInput = InputSystem.actions.FindAction("Respawn");
            _respawnPosition = transform.position;
            _playerController = GetComponent<Controller>();
            _movement = GetComponent<Movement>();
            
        }

        private void Update()
        {
            if (!_boss)
            {
                _boss = GetComponent<Merbles.Boss>();
            }
            if (_respawnInput.WasPressedThisFrame())
            {
                Respawn();
            }
        }

        private void FixedUpdate()
        {
            CheckForRespawn();
        }

        private void Respawn()
        {
            Debug.Log("Respawn");
            _playerController.transform.position = _respawnPosition;
            foreach (var merble in _boss.merbleList)
            {
                merble.transform.position = _respawnPosition;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "RespawnPlane")
            {
                //Respawn();
            }
        }

        void CheckForRespawn()
        {
            RaycastHit hit;
            if (Physics.SphereCast((transform.position), 0.5f, -Vector3.up, out hit, 1.5f, respawnMask))
            {
                //Debug.Log("Respawn Raycast");
                Respawn();
            }
        }
    }

}