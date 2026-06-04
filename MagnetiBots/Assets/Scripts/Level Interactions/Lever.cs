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
            triggerObject.ActivateObject();
        }
    }
}
