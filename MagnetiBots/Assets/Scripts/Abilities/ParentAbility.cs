using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class Parent : MonoBehaviour
    {
        protected InputAction activateInput;
        protected InputAction chargeInput;
        protected InputAction fireInput;
        
        protected bool isCharging;
        public bool IsCharging {get => isCharging; set => isCharging = value;}

        private void Start()
        {
            StartCoroutine(Charge());
        }

        public virtual void Activate()
        {
            throw new System.NotImplementedException();
        }
        public virtual IEnumerator Charge()
        {
            while (isCharging)
            {
                yield return null;
            }
        }
        public virtual void Fire()
        {
            throw new System.NotImplementedException();
        }
        public virtual void GetInputs()
        {
            if (activateInput.IsPressed())
            {
                Activate();
            }

            if (chargeInput.IsPressed())
            {
                Charge();
            }
            else if (fireInput.IsPressed())
            {
                Fire();
            }
        }
    }
}