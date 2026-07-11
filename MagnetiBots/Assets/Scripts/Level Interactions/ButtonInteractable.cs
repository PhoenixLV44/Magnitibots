using System;
using UnityEngine;

namespace Interactable
{
    public class Button : TriggerObject
    {
        private void Awake()
        {
            canBeDeactivated = false;
        }

        void Start()
        {
            if (cat)
            {
                cat.IncreaseTriggersNeeded();
            }
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

                cube.GetComponent<ItemRespawner>().CanRespawn = false;
                
                cube.FreezeConstraints();
            }
            ActivateObject();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LassoTarget"))
            {
                other.GetComponent<ItemRespawner>().CanRespawn = true;
            }
            DeactivateObject();
        }
    }
}
