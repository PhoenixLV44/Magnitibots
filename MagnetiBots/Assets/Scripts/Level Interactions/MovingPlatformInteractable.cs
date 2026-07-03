using System;
using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class MovingPlatform : InteractableObject
    {
        private GameObject _platform;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float moveSpeed = 5;
        GameObject _cutsceneCamera;
        GameObject _mainCamera;

        private void Start()
        {
            _mainCamera = GameObject.Find("CameraPivotPoint");
            _platform = transform.GetChild(0).gameObject;
            _cutsceneCamera = transform.GetChild(1).gameObject;
            _cutsceneCamera.SetActive(false);
            if (startPosition == Vector3.zero)
                startPosition = _platform.transform.position;
            
            else
                _platform.transform.position = startPosition;
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
            Player.Controller player = FindObjectOfType<Player.Controller>();
            player.Interacting = true;
            _cutsceneCamera.SetActive(true);
            _mainCamera.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            float time = 0;
            while (time < 1)
            {
                _platform.transform.position = Vector3.Slerp(firstPos, secondPos, time);
                time += (Time.deltaTime *  moveSpeed);
                time = Mathf.Clamp(time, 0, 1);
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            _mainCamera.SetActive(true);
            _cutsceneCamera.SetActive(false);
            Debug.Log("Done Moving");
        }
    }
}