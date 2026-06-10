using System;
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

        private void Start()
        {
            _respawnInput = InputSystem.actions.FindAction("Respawn");
            _respawnPosition = transform.position;
        }
    }

}