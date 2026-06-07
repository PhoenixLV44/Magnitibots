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
        
        protected int currentPowerLevel = 1;
        protected int basePowerLevel = 1;
        protected float baseRange;
        protected int maxPowerLevel;
        public int CurrentPowerLevel => currentPowerLevel;
        
        protected Player.Controller controller;
        protected IEnumerator chargeCoroutine;
        protected TargetingCursor targetCursor;
        protected GameObject targetCursorObject;
        protected RangeIndicator rangeIndicator;
        
        protected GameObject aimingGuide;

        private void Start()
        {
            InitializeAbility();
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
        public void StartCharging()
        {
            Debug.Log("Starting charging");
            if (chargeCoroutine != null)
            {
                aimingGuide.SetActive(true);
                StartCoroutine(chargeCoroutine);
            }
            else
            {
                chargeCoroutine = Charge();
                aimingGuide.SetActive(true);
                StartCoroutine(chargeCoroutine);
            }
        }

        public void StopCharging()
        {
            if (chargeCoroutine != null)
            {
                Debug.Log("Stopping charging");
                aimingGuide.SetActive(false);
                currentPowerLevel = basePowerLevel;
                StopCoroutine(chargeCoroutine);
            }
        }

        public virtual void InitializeAbility()
        {
            targetCursor = GetComponent<TargetingCursor>();
            targetCursorObject = targetCursor.gameObject;
            
            controller = GetComponent<Player.Controller>();
            
            rangeIndicator = GetComponent<RangeIndicator>();
            
            chargeCoroutine = Charge();
            
            aimingGuide = transform.GetChild(0).transform.GetChild(1).gameObject;
            aimingGuide.SetActive(false);
        }
    }
}