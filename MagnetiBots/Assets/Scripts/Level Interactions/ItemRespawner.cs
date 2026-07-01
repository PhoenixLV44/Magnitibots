using System;
using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class ItemRespawner : MonoBehaviour
    {
        private Player.Respawner _playerRespawner;
        Vector3 _initialPosition;
        Vector3 _initialRotation;
        Rigidbody _rb;

        private void Start()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation.eulerAngles;
            _rb = GetComponent<Rigidbody>();
            StartCoroutine(FindPlayerRespawn());
        }
        public void Respawn()
        {
            transform.position = _initialPosition;
            transform.rotation = Quaternion.Euler(_initialRotation);
            _rb.linearVelocity = Vector3.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "RespawnPlane")
            {
                Respawn();
            }
        }
        IEnumerator FindPlayerRespawn()
        {
            while (!_playerRespawner)
            {
                _playerRespawner = FindFirstObjectByType<Player.Respawner>();
                yield return null;
            }
            //Debug.Log("Found player respawn");
            _playerRespawner.ItemRespawners.Add(this);
        }
    }
}