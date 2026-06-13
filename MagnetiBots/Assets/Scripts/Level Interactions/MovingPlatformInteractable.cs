using System;
using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class MovingPlatform : InteractableObject
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float moveSpeed = 5;

        private void Start()
        {
            if (startPosition == Vector3.zero)
                startPosition = transform.position;
            
            else
                transform.position = startPosition;
        }

        public override void ActivateObject()
        {
            StartCoroutine(MovePlatform(startPosition, endPosition));
        }

        public override void DeactivateObject()
        {
            StartCoroutine(MovePlatform(endPosition, startPosition));
        }

        private IEnumerator MovePlatform(Vector3 firstPos, Vector3 secondPos)
        {
            float time = 0;
            while (time < 1)
            {
                transform.position = Vector3.Slerp(firstPos, secondPos, time);
                time += (Time.deltaTime *  moveSpeed);
                yield return null;
            }

            Debug.Log("Done Moving");
        }
    }
}