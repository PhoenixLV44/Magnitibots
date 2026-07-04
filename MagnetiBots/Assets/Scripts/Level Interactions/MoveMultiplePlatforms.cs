using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactable
{
    public class MoveMultiplePlatforms : InteractableObject
    {
        [SerializeField] private MovingPlatform[] platforms;
        [SerializeField] private GameObject cutsceneCamera;
        private GameObject _mainCamera;
        [SerializeField] private float delay = 0.5f;

        void Start()
        {
            
            cutsceneCamera = GetComponentInChildren<Camera>().gameObject;
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            cutsceneCamera.SetActive(false);
        }

        public override void ActivateObject()
        {
            StartCoroutine(ActivatePlatforms());
        }

        public override void DeactivateObject()
        {
            StartCoroutine(DeactivatePlatforms());
        }

        private IEnumerator ActivatePlatforms()
        {
            StopCoroutine(DeactivatePlatforms());
            Player.Controller player = FindObjectOfType<Player.Controller>();
            player.Interacting = true;
            if (cutsceneCamera)
            {
                cutsceneCamera.SetActive(true);
                _mainCamera.SetActive(false);
            }

            foreach (var platform in platforms)
            {
                platform.ActivateObject();
                yield return new WaitForSecondsRealtime(delay);
            }
            yield return new WaitUntil(() => platforms[platforms.Length - 1].transform.localPosition == platforms[platforms.Length - 1].EndPosition);
            
            yield return new WaitForSecondsRealtime(delay);
            
            _mainCamera.SetActive(true);
            cutsceneCamera.SetActive(false);
            player.Interacting = false;
            //Debug.Log("Done Moving");
        }
        private IEnumerator DeactivatePlatforms()
        {
            StopCoroutine(ActivatePlatforms());
            foreach (var platform in platforms)
            {
                platform.DeactivateObject();
                yield return new WaitForSecondsRealtime(delay);
            }
            //Debug.Log("Done Moving");
        }
    }
}
