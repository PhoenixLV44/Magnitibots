using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactable
{
    public class MoveMultiplePlatforms : InteractableObject
    {
        [SerializeField] private MovingPlatform[] platforms;
        [SerializeField] private GameObject cutsceneCamera;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private float delay = 0.5f;

        void Start()
        {

            if (!cutsceneCamera)
            {
                cutsceneCamera = transform.GetChild(transform.childCount - 1).gameObject;
            }

            if (!mainCamera)
            {
                mainCamera = GameObject.Find("CameraPivotPoint").transform.GetChild(0).gameObject;
            }
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
            Player.Controller player = FindFirstObjectByType<Player.Controller>();
            player.Interacting = true;
            yield return new WaitForSeconds(delay);
            if (cutsceneCamera)
            {
                cutsceneCamera.SetActive(true);
                mainCamera.SetActive(false);
            }
            yield return new  WaitForSecondsRealtime(delay);
            foreach (var platform in platforms)
            {
                platform.ActivateObject();
                yield return new WaitForSecondsRealtime(delay);
            }
            yield return new WaitForSecondsRealtime(0.5f);
            if (triggerObject.Cat)
            {
                triggerObject.Cat.IncreaseTriggers();
            }
            yield return new WaitForSecondsRealtime(0.5f);
            mainCamera.SetActive(true);
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
