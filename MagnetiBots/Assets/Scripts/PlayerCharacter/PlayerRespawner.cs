using System;
using Merbles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Respawner : MonoBehaviour
    {
        private InputAction _respawnInput;
        Vector3 _respawnPosition;
        public Vector3 RespawnPosition { get { return _respawnPosition; } set { _respawnPosition = value; } }
        Controller _playerController;
        Movement _movement;
        Merbles.Boss _boss;

        private void Start()
        {
            _respawnInput = InputSystem.actions.FindAction("Respawn");
            _respawnPosition = transform.position;
            _playerController = GetComponent<Controller>();
            _movement = _playerController.Movement;
            _boss = GetComponent<Merbles.Boss>();
        }

        private void Update()
        {
            if (_respawnInput.WasPressedThisFrame())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            _playerController.transform.position = _respawnPosition;
           _movement.rb.linearVelocity = Vector3.zero;
            foreach (var merble in _boss.merbleList)
            {
                merble.transform.position = _respawnPosition;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "RespawnPlane")
            {
                Respawn();
            }
        }
    }

}