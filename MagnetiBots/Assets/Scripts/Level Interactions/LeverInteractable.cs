using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class Lever : TriggerObject
    {

        private void Start()
        {
            delayBetweenObjects = Mathf.Clamp(delayBetweenObjects, 0, Mathf.Infinity);
        }
        public override void ActivateObject()
        {
            base.ActivateObject();
        }
        public override void DeactivateObject()
        {
            base.DeactivateObject();
        }
        
    }
}
