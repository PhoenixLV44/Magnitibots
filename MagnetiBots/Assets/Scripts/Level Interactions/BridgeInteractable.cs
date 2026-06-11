using System;
using UnityEngine;

namespace Interactable
{
    public class Bridge : Interactable.InteractableObject
    {
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            canBeDeactivated = false;
        }

        public override void ActivateObject()
        {
            animator.SetTrigger("Activate");
        }
    }

}