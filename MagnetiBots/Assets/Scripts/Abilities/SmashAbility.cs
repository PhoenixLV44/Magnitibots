using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ability.Object;

namespace Ability
{
    public class Smash : Parent
    {
        private GameObject _smashBall;
        //public GameObject SmashBall => _smashBall;
        private Rigidbody _smashBallRb;
        private void Start()
        {
            InitializeAbility();
            maxPowerLevel = 3;
            basePowerLevel = 5;
        }

        public override void Activate()
        {
            //base.Activate();
        }

        public override IEnumerator Charge()
        {
            //Debug.Log("Charging Smash");
            float chargeTimer = 1f;
            rangeIndicator.ChangeRangeSize(baseRange * maxPowerLevel * 2 );
            yield return new WaitForSeconds(chargeTimer/2);
            while (true)
            {

                if (currentPowerLevel < basePowerLevel)
                {
                    currentPowerLevel++;
                    _smashBall.GetComponent<SmashBall>().PowerLevel = currentPowerLevel;
                }
                yield return new WaitForSeconds(chargeTimer);
            }
        }

        public override void StartCharging()
        {
            base.StartCharging();
            ActivateBall();
            StartCoroutine(MoveCursor());
        }

        public override void Fire()
        {
            DropBall();
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            
            _smashBall = Instantiate(Resources.Load<GameObject>("Prefabs/SmashBallPrefab"), transform.position, transform.rotation, transform);
            _smashBall.GetComponent<SmashBall>().SmashAbility = this;
            _smashBall.name = "SmashBall";
            _smashBallRb = _smashBall.GetComponent<Rigidbody>();
            
            DeactivateBall();
        }
        
        private void ActivateBall()
        {
            _smashBallRb.useGravity = false;
            _smashBall.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            
            targetCursor.ActivateCursor(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            _smashBall.GetComponent<SmashBall>().TriggerCollider.enabled = false;
            _smashBall.SetActive(true);

            currentPowerLevel = basePowerLevel;

            //StartCoroutine(MoveCursor());
        }

        public void DeactivateBall()
        {
            rangeIndicator.DisableRangeIndicator();
            _smashBall.SetActive(false);
        }
        private void DropBall()
        {
            StopAllCoroutines();
            _smashBallRb.useGravity = true;
            _smashBall.GetComponent<SmashBall>().TriggerCollider.enabled = true;
            targetCursor.DeactivateCursor();
        }
        private IEnumerator MoveCursor()
        {
            while (true)
            {
                //Debug.Log("Move Cursor");
                //targetCursor.MoveCursor();
                targetCursor.MoveObjectToCursor(_smashBall);
                yield return new WaitForFixedUpdate();
            }
        }
    }   
}