using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Merbles;

namespace Ability
{
    public class SuperJump : Parent
    {
        private Player.Movement _playerMovement;
        private SuperJumpPoint _superJumpPoint;
        private Transform[]  _merblePoints;
        IEnumerator _moveMerblesCoroutine;
        
        GameObject _hoverParticles;
        
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
            Debug.Log("Super Jump Start Charge");
            currentPowerLevel = 0;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(chargeTimer);
            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            //Debug.Log("MAX POWER: " + maxPower);
            yield return wait;
            for (int i = 0; i < 5; i++)
            {
                
                Debug.Log("Super Jump Merble Charge");
                if (!merbleBoss.ChargedMerbleList.Contains(merbleBoss.merbleList[i]) && !merbleBoss.merbleList[i].Charging && merbleBoss.merbleList.Count > 0)
                {
                    merbleBoss.merbleList[i].StartCharge(transform.position);
                    Globals.Managers.Audio.PlaySFX("ChargeMerble");
                }

                if (merbleBoss.ChargedMerbleList.Count == 0 && !chargeInput.IsPressed())
                {
                    merbleBoss.FireMerbles();
                    foreach (var merble in merbleBoss.MasterList)
                    {
                        if (merble.Charging)
                        {
                            merble.StopCharging();
                        }
                    }
                    StopAllCoroutines();
                    yield break;
                }
            }

            int j = 0;
            while (true)
            {
                Debug.Log("Super Jump While Loop");
                currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                //Debug.Log("Current PowerLevel: " + currentPowerLevel);
                merbleBoss.merbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                Merble[] merbleArray = merbleBoss.merbleList.ToArray();

                if (merbleArray.Length > 0)
                {
                    merbleArray[j].StartCharge(transform.position);
                }

                yield return wait;
            }
        }

        public override void Fire()
        {
            int jumpPowerMult = merbleBoss.ChargedMerbleList.Count;
            
            controller.ChargingParticles.SetActive(false);

            if (merbleBoss.ChargedMerbleList.Count > 0)
            {
                StartCoroutine(_playerMovement.Jump(jumpPowerMult));
                if (merbleBoss.ChargedMerbleList.Count > 5)
                {
                    _playerMovement.Hovering = true;
                    _hoverParticles.SetActive(true);
                }
            }
            Globals.Managers.Audio.PlaySFX("SuperJump");
            StopCharging();
            
        }

        public override void StartCharging()
        {
            if (chargeCoroutine != null)
            {
                if (merbleBoss.MasterList.Count >= 1)
                {
                    StartCoroutine(Charge());
                    StartCoroutine(_moveMerblesCoroutine);
                    StartCoroutine(CheckForGround());
                    controller.ChargingParticles.SetActive(true);
                }
            }
            else
            {
                if (merbleBoss.MasterList.Count >= 1)
                {
                    StartCoroutine(Charge());
                    StartCoroutine(_moveMerblesCoroutine);
                    StartCoroutine(CheckForGround());
                    controller.ChargingParticles.SetActive(true);
                }
            }
        }

        public void StopHovering()
        {
            Debug.Log("STOP Hovering");
            _playerMovement.Hovering = false;
            _hoverParticles.SetActive(false);
            _playerMovement.GravityOn = true;
            //_hoverParticles.SetActive(false);
        }

        public override void StopCharging()
        {
            /*base.StopCharging();*/
            currentPowerLevel = basePowerLevel;
            rangeIndicator.DisableRangeIndicator();
            controller.ChargingParticles.SetActive(false);
            StopCoroutine(Charge());
        }
        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            _playerMovement = GetComponent<Player.Movement>();
            _superJumpPoint = transform.GetComponentInChildren<SuperJumpPoint>();
            maxPowerLevel = 10;
            _moveMerblesCoroutine = MoveMerbles();
            _hoverParticles = _superJumpPoint.HoverParticles;
            _hoverParticles.SetActive(false);
        }

        private IEnumerator MoveMerbles()
        {
            yield return new WaitUntil(() => merbleBoss.ChargedMerbleList.Count > 0);
            while(true)
            {
                for(int i = 0; i < merbleBoss.ChargedMerbleList.Count; i++)
                {
                    merbleBoss.ChargedMerbleList[i].FloatTowardsObject(_superJumpPoint.MerblePoints[i].transform.position, i, Merble.AbilityEnum.SuperJump, _superJumpPoint.RotationSpeed);
                }
                yield return null;
            }
        }

        private IEnumerator CheckForGround()
        {
            yield return new WaitUntil((() => !_playerMovement.Grounded));
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => _playerMovement.Grounded);
            if(_playerMovement.Hovering)
                _playerMovement.Hovering = false;
            StopCoroutine(_moveMerblesCoroutine);
            StopAllCoroutines();
            merbleBoss.FireMerbles();
        }
    }
}
