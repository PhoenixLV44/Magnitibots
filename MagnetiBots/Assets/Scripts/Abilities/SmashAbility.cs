using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class Smash : Parent
    {
        private void Start()
        {
            activateInput = InputSystem.actions.FindAction("ActivateSmash");
            chargeInput = InputSystem.actions.FindAction("Charge");
            fireInput = InputSystem.actions.FindAction("Fire");
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override IEnumerator Charge()
        {
            while (isCharging)
            {
                yield return null;
            }
        }

        public override void Fire()
        {
            base.Fire();
        }
    }   
}