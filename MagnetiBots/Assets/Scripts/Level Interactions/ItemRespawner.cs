using System;
using UnityEngine;

namespace Interactable
{
    public class ItemRespawner : MonoBehaviour
    {
        Vector3 initialPosition;
        Rigidbody rb;

        private void Start()
        {
            initialPosition = transform.position;
            rb = GetComponent<Rigidbody>();
        }
        private void Respawn()
        {
            transform.position = initialPosition;
            rb.linearVelocity = Vector3.zero;
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