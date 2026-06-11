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
            baseRange = 5;
        }

        public override void Activate()
        {
            //base.Activate();
        }

        public override IEnumerator Charge()
        {
            //Debug.Log("Charging Smash");
            float chargeTimer = 0.5f;
            yield return new WaitForSeconds(chargeTimer/2);
            while (true)
            {
                if (currentPowerLevel < maxPowerLevel)
                {
                    //Debug.Log("UPPING POWER LEVEL");
                    currentPowerLevel += 0.25f;
                    _smashBall.GetComponent<SmashBall>().IncreasePowerLevel(currentPowerLevel);
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
            Debug.Log("Activating Ball");
            SmashBall smashBallScript = _smashBall.GetComponent<SmashBall>();
            
            rangeIndicator.ChangeRangeSize(baseRange * maxPowerLevel * 2 );

            _smashBallRb.useGravity = false;
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            newPosition = controller.Movement.adjustedMovement * newPosition;
            newPosition.y = transform.position.y + 5;
            _smashBall.transform.position = newPosition;
            _smashBall.transform.localScale = smashBallScript.BaseScale;
            
            targetCursor.ActivateCursor(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            smashBallScript.TriggerCollider.enabled = false;
            _smashBall.SetActive(true);

            currentPowerLevel = basePowerLevel;

            //StartCoroutine(MoveCursor());
        }

        public void DeactivateBall()
        {
            _smashBallRb.linearVelocity = Vector3.zero;
            _smashBall.SetActive(false);
        }
        private void DropBall()
        {
            StopAllCoroutines();
            _smashBallRb.useGravity = true;
            _smashBall.GetComponent<SmashBall>().TriggerCollider.enabled = true;
            targetCursor.DeactivateCursor();
            rangeIndicator.DisableRangeIndicator();
        }
        private IEnumerator MoveCursor()
        {
            while (true)
            {
                //Debug.Log("Move Cursor");
                //targetCursor.MoveCursor();
                targetCursor.MoveObjectToCursor(_smashBall);
                yield return null;
            }
        }
    }   
}