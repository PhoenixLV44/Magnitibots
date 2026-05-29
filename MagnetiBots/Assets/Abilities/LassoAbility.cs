using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class LassoAbility : PlayerAbility
    {
        private void Start()
        {
            activateInput = InputSystem.actions.FindAction("ActivateLasso");
            chargeInput = InputSystem.actions.FindAction("Charge");
            fireInput = InputSystem.actions.FindAction("Fire");
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Charge()
        {
            base.Charge();
        }

        public override void Fire()
        {
            base.Fire();
        }

        public override void GetInputs()
        {
            if (activateInput.IsPressed())
            {
                Activate();
            }
        }
    }
}
