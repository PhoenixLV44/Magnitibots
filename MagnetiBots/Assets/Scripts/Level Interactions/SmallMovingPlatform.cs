using System;
using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class SmallMovingPlatform : MovingPlatform
    {
        private GameObject _platform;
        
        [SerializeField] private Vector3 startPosition;
        public Vector3 StartPosition => startPosition;
        [SerializeField]private Vector3 endPosition;
        public Vector3 EndPosition => endPosition;
        
        [SerializeField] private float moveSpeed = 5;
        

        private void Start()
        {
            _platform = transform.GetChild(0).gameObject;
            if (startPosition == Vector3.zero)
                startPosition = _platform.transform.localPosition;
            
            else
                _platform.transform.localPosition = startPosition;
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
            //Player.Controller player = FindObjectOfType<Player.Controller>();
            //player.Interacting = true;
            yield return new WaitForSeconds(0.5f);
            float time = 0;
            while (time < 1)
            {
                _platform.transform.localPosition = Vector3.Slerp(firstPos, secondPos, time);
                time += (Time.deltaTime *  moveSpeed);
                time = Mathf.Clamp(time, 0, 1);
                yield return null;
            }
            _platform.transform.localPosition = secondPos;
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("Done Moving");
        }
    }
}
