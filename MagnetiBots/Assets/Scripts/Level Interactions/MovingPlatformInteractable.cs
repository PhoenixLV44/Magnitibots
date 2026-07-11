using System;
using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class MovingPlatform : InteractableObject
    {
        protected GameObject platform;
        
        [SerializeField] protected Vector3 startPosition;
        public Vector3 StartPosition => startPosition;
        [SerializeField]protected Vector3 endPosition;
        public Vector3 EndPosition => endPosition;
        
        [SerializeField] protected float moveSpeed = 5;
        GameObject _cutsceneCamera;
        GameObject _mainCamera;

        private void Start()
        {
            _mainCamera = GameObject.Find("CameraPivotPoint").transform.GetChild(0).gameObject;
            platform = transform.GetChild(0).gameObject;
            _cutsceneCamera = transform.GetChild(transform.childCount - 1).gameObject;
            if (!_cutsceneCamera)
            {
                Debug.LogError(transform.name + " Cutscene camera not found");
            }
            _cutsceneCamera.SetActive(false);
            if (startPosition == Vector3.zero)
                startPosition = platform.transform.localPosition;
            
            else
                platform.transform.localPosition = startPosition;
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
            Player.Controller player = FindFirstObjectByType<Player.Controller>();
            player.Interacting = true;
            yield return new WaitForSeconds(player.AnimController.PullLeverAnimation.length);
            if (_cutsceneCamera)
            {
               // _cutsceneCamera.transform.LookAt(_platform.transform);
                _cutsceneCamera.SetActive(true);
                _mainCamera.SetActive(false);
            }
            yield return new WaitForSeconds(1f);
            Globals.Managers.Audio.PlaySFXHere("movingPlatformsSfx", transform);
            float time = 0;
            while (time < 1)
            {
                platform.transform.localPosition = Vector3.Slerp(firstPos, secondPos, time);
                time += (Time.deltaTime *  moveSpeed);
                time = Mathf.Clamp(time, 0, 1);
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            if (triggerObject.Cat)
            {
                triggerObject.Cat.IncreaseTriggers();
            }
            yield return new WaitForSeconds(0.5f);
            platform.transform.localPosition = secondPos;
            _mainCamera.SetActive(true);
            _cutsceneCamera.SetActive(false);
            player.Interacting = false;
            //Debug.Log("Done Moving");
        }
    }
}