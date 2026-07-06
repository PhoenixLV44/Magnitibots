using System;
using UnityEngine;

namespace Interactable
{
    public class Button : TriggerObject
    {
        private void Awake()
        {
            canBeDeactivated = true;
        }

        public override void ActivateObject()
        {
            base.ActivateObject();
            Globals.Managers.Audio.PlaySFXHere("UI_WoodClick", transform);
        }

        public override void DeactivateObject()
        {
            base.DeactivateObject();           
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LassoTarget"))
            {
                PuzzleCube cube = other.GetComponent<PuzzleCube>();
                cube.FreezeConstraints();
            }
            ActivateObject();
        }

        private void OnTriggerExit(Collider other)
        {
            DeactivateObject();
        }
    }
}
