using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ability.Object;

namespace Ability
{
    public class Parent : MonoBehaviour
    {
        protected InputAction activateInput;
        protected InputAction chargeInput;
        protected InputAction fireInput;
        
        protected bool isCharging;
        public bool IsCharging {get => isCharging; set => isCharging = value;}
        
        protected float currentPowerLevel = 1;
        protected float basePowerLevel = 1;
        protected float baseRange;
        protected int maxPowerLevel;
        public float CurrentPowerLevel => currentPowerLevel;
        protected float heightOffset;
        
        protected Player.Controller controller;
        protected IEnumerator chargeCoroutine;
        protected TargetingCursor targetCursor;
        public TargetingCursor TargetCursor => targetCursor;
        protected GameObject targetCursorObject;
        protected RangeIndicator rangeIndicator;
        
        [SerializeField]protected GameObject aimingGuide;

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
        public virtual void StartCharging()
        {
            //Debug.Log("Starting charging");
            if (chargeCoroutine != null)
            {
                StartCoroutine(chargeCoroutine);
            }
            else
            {
                chargeCoroutine = Charge();
                StartCoroutine(chargeCoroutine);
            }
        }

        public virtual void StopCharging()
        {
            if (chargeCoroutine != null)
            {
                Debug.Log("Stopping charging");
                aimingGuide.SetActive(false);
                currentPowerLevel = basePowerLevel;
                rangeIndicator.DisableRangeIndicator();
                StopCoroutine(chargeCoroutine);
            }
        }

        protected virtual void InitializeAbility()
        {
            targetCursor = GetComponent<TargetingCursor>();
            targetCursorObject = targetCursor.gameObject;
            
            controller = GetComponent<Player.Controller>();
            
            rangeIndicator = GetComponent<RangeIndicator>();
            
            chargeCoroutine = Charge();
            
            aimingGuide = transform.GetChild(0).transform.Find("Aiming Guide").gameObject;
            aimingGuide.SetActive(false);
        }
    }
}