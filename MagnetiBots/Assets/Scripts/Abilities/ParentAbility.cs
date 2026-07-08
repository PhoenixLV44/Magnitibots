using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ability.Object;
using Merbles;
using System.Collections.Generic;
using Player;

namespace Ability
{
    public class Parent : MonoBehaviour
    {
        protected InputAction activateInput;
        protected InputAction chargeInput;
        protected InputAction fireInput;
        
        protected bool isCharging;
        public bool IsCharging {get => isCharging; set => isCharging = value;}
        
        [SerializeField] protected float currentPowerLevel;
        protected float basePowerLevel;
        public float BasePowerLevel => basePowerLevel;
        protected float baseRange;
        public float BaseRange => baseRange;
        protected int maxPowerLevel;
        public int MaxPowerLevel => maxPowerLevel;
        public float CurrentPowerLevel => currentPowerLevel;
        protected float heightOffset;
        
        protected Player.Controller controller;
        public Player.Controller Controller => controller;
        protected IEnumerator chargeCoroutine;
        public IEnumerator ChargeCoroutine => chargeCoroutine;
        protected TargetingCursor targetCursor;
        public TargetingCursor TargetCursor => targetCursor;
        protected GameObject targetCursorObject;
        protected RangeIndicator rangeIndicator;
        
        protected Merbles.Boss merbleBoss;

        public Boss MerbleBoss { get => merbleBoss; set => merbleBoss = value; }
        /*protected List<Merble> chargedMerbleList;
        public List<Merble> ChargedMerbleList => chargedMerbleList;*/

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
            //controller.StopJumpCoroutines();
            if (chargeCoroutine != null)
            {
                if (merbleBoss.MasterList.Count >= 1)
                {
                    StartCoroutine(chargeCoroutine);
                    controller.ChargingParticles.SetActive(true);
                }
            }
            else
            {
                chargeCoroutine = Charge();
                StartCoroutine(chargeCoroutine);
                controller.ChargingParticles.SetActive(true);
            }
        }

        public virtual void StopCharging()
        {
            if (chargeCoroutine != null)
            {
                //Debug.Log("Stopping charging");
                //aimingGuide.SetActive(false);
                currentPowerLevel = basePowerLevel;
                rangeIndicator.DisableRangeIndicator();
                controller.ChargingParticles.SetActive(false);
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
            
            //aimingGuide = transform.GetChild(0).transform.Find("Aiming Guide").gameObject;
            //aimingGuide.SetActive(false);

            merbleBoss = GetComponent<Merbles.Boss>();
        }

        public virtual void Respawn()
        {
            merbleBoss.FireMerbles();
            isCharging = false;
            StopAllCoroutines();
            StopCharging();
        }
    }
}