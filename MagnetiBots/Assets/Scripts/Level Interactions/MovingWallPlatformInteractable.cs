using UnityEngine;

namespace Interactable
{
    public class MovingWallPlatformInteractable : InteractableObject
    {
        Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public override void ActivateObject()
        {
            base.ActivateObject();
            animator.SetBool("Activate", true);
        }
    }
}
