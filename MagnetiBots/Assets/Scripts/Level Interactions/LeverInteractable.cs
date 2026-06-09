using UnityEngine;

namespace Interactable
{
    public class Lever : InteractableObject
    {
        [SerializeField] InteractableObject triggerObject;

        private void PlayAnimation() //for when animations are in the game
        {

        }

        public override void ActivateObject()
        {
            if (triggerObject != null)
            {
                triggerObject.ActivateObject();
            }
            else
            {
                Debug.LogWarning("Trigger object is null!");
            }
        }
        public override void DeactivateObject()
        {
            if (triggerObject != null)
            {
                triggerObject.DeactivateObject();
            }
            else
            {
                Debug.LogWarning("Trigger object is null!");
            }
        }
    }
}
