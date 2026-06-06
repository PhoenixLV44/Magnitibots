using UnityEngine;

namespace Interactable
{
    public class InteractableObject : MonoBehaviour
    {
        protected bool activated = false;
        public bool canBeDeactivated;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public virtual void ActivateObject()
        {

        }

        public virtual void DeactivateObject()
        {
            
        }
    }
}
