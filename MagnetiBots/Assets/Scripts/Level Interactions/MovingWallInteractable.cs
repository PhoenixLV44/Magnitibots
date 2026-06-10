using UnityEngine;

namespace Interactable
{
    public class MovingWall: InteractableObject
    {
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            canBeDeactivated = false;
        }

        public override void ActivateObject()
        {
            base.ActivateObject();
            animator.SetTrigger("Activate");
        }
    }
}
