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
            //base.Activate();
            Debug.Log("Activating Lasso Ability");
        }

        public override void Charge()
        {
            //base.Charge();
            Debug.Log("Charging Lasso Ability");
        }

        public override void Fire()
        {
            //base.Fire();
            Debug.Log("Firing Lasso Ability");
        }

        private void Update()
        {
            GetInputs();
        }
    }
}
