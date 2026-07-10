using System;
using UnityEngine;

namespace Cat
{
    public class CatTrigger : MonoBehaviour
    {
        private Cat _cat;

        private void Start()
        {
            _cat = GetComponentInParent<Cat>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || (_cat.ReactToCube && other.CompareTag("LassoTarget")))
            {
                Debug.Log("MEow");
                StartCoroutine(_cat.Disappear());
            }
        }
    }
}
