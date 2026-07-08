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
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            transform.position = _initialPosition;
            transform.rotation = Quaternion.Euler(_initialRotation);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RespawnPlane"))
            {
                Globals.Managers.Audio.PlaySFX("waterSplash");
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