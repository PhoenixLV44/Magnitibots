using System;
using UnityEngine;

namespace Interactable
{
    public class Button : InteractableObject
    {
        [SerializeField] InteractableObject[] triggerObjects;

        private void Awake()
        {
            canBeDeactivated = true;
        }
        private void PlayAnimation() //for when animations are in the game
        {

        }

        public override void ActivateObject()
        {
            if (triggerObjects != null)
            {
                foreach (var triggerObject in triggerObjects)
                {
                    triggerObject.ActivateObject();
                }
            }
            else
            {
                Debug.LogWarning("Trigger object is null!");
            }
        }

        public override void DeactivateObject()
        {
            if (triggerObjects != null)
            {
                foreach (var triggerObject in triggerObjects)
                {
                    triggerObject.DeactivateObject();
                }            
            }
            else
            {
                Debug.LogWarning("Trigger object is null!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ActivateObject();
        }

        private void OnTriggerExit(Collider other)
        {
            DeactivateObject();
        }
    }
}
