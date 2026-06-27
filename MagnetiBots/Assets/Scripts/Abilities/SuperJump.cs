using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Merbles;

namespace Ability
{
    public class SuperJump : Parent
    {
        private Player.Movement _playerMovement;
        private GameObject _superJumpPoint;
        private Transform[]  _merblePoints;
        IEnumerator _rotateCoroutine;
        
        private void Start()
        {
            activateInput = InputSystem.actions.FindAction("Activate Super Jump");
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
            controller.ChargingParticles.SetActive(true);
            merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

            while (merbleBoss.ChargedMerbleList.Count <= 10)
            {
                if (!merbleBoss.ChargedMerbleList.Contains(merbleBoss.merbleList[0]) && merbleBoss.ChargedMerbleList.Count < 10)
                {
                    merbleBoss.merbleList[0].StartCharge(transform.position);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        public override void Fire()
        {
            int jumpPowerMult = merbleBoss.ChargedMerbleList.Count;
            
            controller.ChargingParticles.SetActive(false);
            
            _playerMovement.Jump(jumpPowerMult);
            StopCharging();
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
            base.InitializeAbility();
            _playerMovement = GetComponent<Player.Movement>();
            _superJumpPoint = transform.GetChild(6).gameObject;
            maxPowerLevel = 10;
            /*
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
                        merbleArray[i].FloatTowardsObject(merblePoints[i].position, i,Merble.AbilityEnum.SuperJump, 5);
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
