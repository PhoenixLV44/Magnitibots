using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class TriggerObject : InteractableObject
    {
        [Tooltip("Objects that this object will activate/deactivate.")]
        [SerializeField] protected InteractableObject[] interactionObjects;
        
        [Tooltip("Delay between objects that will be activated.")]
        [SerializeField] protected float delayBetweenObjects = 0;
        public bool canBeDeactivated;

        [SerializeField] protected Cat.Cat cat; 
        public Cat.Cat Cat => cat;
        [SerializeField] protected bool catMeow;
        [SerializeField] protected bool catDisappear;
        private void Start()
        {
            delayBetweenObjects = Mathf.Clamp(delayBetweenObjects, 0, Mathf.Infinity);
            if (cat)
            {
                cat.IncreaseTriggersNeeded();
            }
        }
        public override void ActivateObject()
        {
            if (interactionObjects != null)
            {
                StartCoroutine(TriggerAction(true));
                activated = true;
            }
            else
            {
                Debug.LogWarning("Trigger objects is null!");
            }
        }
        public override void DeactivateObject()
        {
            if (interactionObjects != null)
            {
                StartCoroutine(TriggerAction(false));
                activated = false;
            }
            else
            {
                Debug.LogWarning("Trigger objects is null!");
            }
        }

        protected virtual IEnumerator TriggerAction(bool activation)
        {
            while (true)
            {
                foreach (var obj in interactionObjects)
                {
                    if (activation)
                        obj.ActivateObject();

                    else
                        obj.DeactivateObject();

                    if (delayBetweenObjects != 0)
                        yield return new WaitForSeconds(delayBetweenObjects);
                }
                yield break;
            }
        }
    }
}