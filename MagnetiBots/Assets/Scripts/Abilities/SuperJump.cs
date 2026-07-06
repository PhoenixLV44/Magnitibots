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
            controller.ChargingParticles.SetActive(true);
            merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

            while (merbleBoss.ChargedMerbleList.Count <= 10)
            {
                if (merbleBoss.ChargedMerbleList.Count < 10 && merbleBoss.merbleList.Count > 0)
                {
                    merbleBoss.merbleList[0].StartCharge(transform.position);
                    Globals.Managers.Audio.PlaySFX("ChargeMerble");
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        public override void Fire()
        {
            int jumpPowerMult = merbleBoss.ChargedMerbleList.Count;
            
            controller.ChargingParticles.SetActive(false);
            
            StartCoroutine(_playerMovement.Jump(jumpPowerMult));
            Globals.Managers.Audio.PlaySFX("SuperJump");
            if (merbleBoss.ChargedMerbleList.Count > 5)
            {
                _playerMovement.Hovering = true;
                _hoverParticles.SetActive(true);
            }
            StopCharging();
            
        }

        public override void StartCharging()
        {
            if (chargeCoroutine != null)
            {
                if (merbleBoss.MasterList.Count >= 1)
                {
                    StartCoroutine(chargeCoroutine);
                    StartCoroutine(_moveMerblesCoroutine);
                    StartCoroutine(CheckForGround());
                    controller.ChargingParticles.SetActive(true);
                }
            }
            else
            {
                chargeCoroutine = Charge();
                StartCoroutine(chargeCoroutine);
                StartCoroutine(_moveMerblesCoroutine);
                StartCoroutine(CheckForGround());
                controller.ChargingParticles.SetActive(true);
            }
        }

        public void StopHovering()
        {
            _playerMovement.Hovering = false;
            //_hoverParticles.SetActive(false);
        }

        public override void StopCharging()
        {
            base.StopCharging();
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
            yield return new WaitUntil(() => _playerMovement.Grounded);
            if(_playerMovement.Hovering)
                _playerMovement.Hovering = false;
            StopCoroutine(_moveMerblesCoroutine);
            merbleBoss.FireMerbles();
        }
    }
}
