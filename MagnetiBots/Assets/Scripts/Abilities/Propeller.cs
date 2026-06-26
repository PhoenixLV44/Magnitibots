using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Merbles;

namespace Ability
{
    public class Propeller : Parent
    {
        private Player.Movement _playerMovement;
        private GameObject _superJumpPoint;
        IEnumerator _rotateCoroutine;
        
        private void Start()
        {
            activateInput = InputSystem.actions.FindAction("ActivatePropeller");
            chargeInput = InputSystem.actions.FindAction("Charge");
            fireInput = InputSystem.actions.FindAction("Fire");
            InitializeAbility();
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override IEnumerator Charge()
        {
            //controller.ChargingParticles.SetActive(true);
            //StartCoroutine(_rotateCoroutine);
            /*currentPowerLevel = 0;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();

            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) =>
                Vector3.Distance(a.transform.position, transform.position)
                    .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            Debug.Log("MAX POWER: " + maxPower);
            for (int i = 0; i < 5; i++)
            {
                if (!merbleBoss.ChargedMerbleList.Contains(merbleBoss.merbleList[i]) &&
                    !merbleBoss.merbleList[i].Charging && merbleBoss.merbleList.Count > 0)
                {
                    merbleBoss.merbleList[i].StartCharge(transform.position);
                }
            }

            int j = 0;
            while (true)
            {

                currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                Debug.Log("Current PowerLevel: " + currentPowerLevel);
                merbleBoss.merbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                Merble[] merbleArray = merbleBoss.merbleList.ToArray();

                if (!merbleBoss.ChargedMerbleList.Contains(merbleArray[j]) && !merbleArray[j].Charging)
                {
                    merbleArray[j].StartCharge(transform.position);
                    if (j < maxPower)
                    {
                        //j++;
                    }
                }

            }
                yield return new WaitForSecondsRealtime(chargeTimer);
            */
            yield return null;
        }

        public override void Fire()
        {
            /*int merbleCount = merbleBoss.merbleList.Count > 0 ? merbleBoss.merbleList.Count : 0;
            if (_playerMovement.Grounded)
            {
                _playerMovement.Jump(merbleCount);
            }
            /*if (merbleBoss.ChargedMerbleList.Count >= 10)
            {
                _playerMovement.Gliding = true;
            }#1#
            StopCharging();*/
        }

        public override void StartCharging()
        {
            /*if (controller.CanUsePropeller)
            {
                if (chargeCoroutine != null)
                {
                    StartCoroutine(chargeCoroutine);
                    controller.ChargingParticles.SetActive(true);
                }
                else
                {
                    chargeCoroutine = Charge();
                    StartCoroutine(chargeCoroutine);
                    controller.ChargingParticles.SetActive(true);
                }            
            }
            else
            {
                Debug.Log("Can't charge propeller");
                _playerMovement.Jump(0);
                StopCharging();
            }*/
        }

        public override void StopCharging()
        {
            base.StopCharging();
        }
        protected override void InitializeAbility()
        {
            /*
            base.InitializeAbility();
            _playerMovement = GetComponent<Player.Movement>();
            _superJumpPoint = transform.GetChild(6).gameObject;
            maxPowerLevel = 10;
            _rotateCoroutine = RotateSuperJumpPoint();
            StartCoroutine(_rotateCoroutine);
            */

        }

        private IEnumerator RotateSuperJumpPoint()
        {
            Debug.Log("RotateSuperJumpPoint");
            Transform[] merblePoints = new Transform[10];
            for (int i = 0; i < merblePoints.Length; i++)
            {
                merblePoints[i] = transform.GetChild(i);
            }
            Vector3 rotationAmount = new Vector3(0, 5, 0);
            while (true)
            {
                _superJumpPoint.transform.Rotate(rotationAmount);
                Merble[] merbleArray = merbleBoss.ChargedMerbleList.ToArray();
                if (merbleArray.Length > 0)
                {
                    for (int i = 0; i < merbleArray.Length; i++)
                    {
                        merbleArray[i].FloatTowardsObject(merblePoints[i].position, i,Merble.AbilityEnum.Propeller, 5);
                    }
                }
            }
        }

        public void ReleaseMerbles()
        {
            merbleBoss.FireMerbles();
        }
    }
}
