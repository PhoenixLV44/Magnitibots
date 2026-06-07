using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ability.Object;

namespace Ability
{
    public class Smash : Parent
    {
        private GameObject _smashObjectPrefab;
        private GameObject _smashBall;
        private SmashBall _smashObjectScript;
        private void Start()
        {
            InitializeAbility();
            
        }

        public override void Activate()
        {
            //base.Activate();
        }

        public override IEnumerator Charge()
        {
            rangeIndicator.ChangeRangeSize(baseRange * maxPowerLevel * 2 );
            float chargeTimer = 1f;
            while (true)
            {
                if (!_smashBall.activeSelf)
                {
                    _smashBall.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    currentPowerLevel++;
                }

            }
        }
        
        private void Update()
        {
            if (InputSystem.actions.FindAction("Charge").IsPressed())
            {
                targetCursor.MoveCursor();
                targetCursor.MoveObjectToCursor(_smashBall);
            }
        }

        public override void StartCharging()
        {
            base.StartCharging();
            targetCursor.ActivateCursor(transform.position);
        }

        public override void StopCharging()
        {
            base.StopCharging();
            targetCursor.DeactivateCursor();
        }

        public override void Fire()
        {
            //base.Fire();
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            _smashObjectPrefab = Resources.Load<GameObject>("Prefabs/SmashBallPrefab");
            _smashBall = Instantiate(_smashObjectPrefab, transform.position, transform.rotation, transform);
            _smashBall.SetActive(false);
            _smashObjectScript = _smashBall.GetComponent<SmashBall>();
        }
    }   
}