using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class Lasso : Parent
    {
        private float _baseRange = 5f;
        private int _basePowerLevel = 1;
        private int _maxPowerLevel = 5;
        private int _currentPowerLevel = 1;
        private void Start()
        {
            activateInput = InputSystem.actions.FindAction("ActivateLasso");
            chargeInput = InputSystem.actions.FindAction("Charge");
            fireInput = InputSystem.actions.FindAction("Fire");
        }

        private void OnEnable()
        {
            StartCoroutine(Charge());
        }

        private void OnDisable()
        {
            StopCoroutine(Charge());
        }

        public override void Activate()
        {
            //base.Activate();
            Debug.Log("Activating Lasso Ability");
        }

        public override IEnumerator Charge()
        {
            float chargeTimer = 0.5f;
            while (true)
            {
                if (isCharging && _currentPowerLevel < _maxPowerLevel)
                {
                    yield return new WaitForSeconds(chargeTimer);
                    Debug.Log("Current Charge: " + _currentPowerLevel);
                    _currentPowerLevel++;
                }
                else if(!isCharging &&  _currentPowerLevel != _basePowerLevel)
                {
                    _currentPowerLevel = _basePowerLevel;
                }
                yield return null;
            }
        }

        public override void Fire()
        {
            if (isCharging)
            {
                isCharging = false;
            }
            
        }
    }
}
