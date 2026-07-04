using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ability.Object;
using Merbles;

namespace Ability
{
    public class Smash : Parent
    {
        private GameObject _smashBall;
        //public GameObject SmashBall => _smashBall;
        private Rigidbody _smashBallRb;

        private IEnumerator moveCursorRoutine;
        private Transform _returnPoint;
        private bool _returnMerbles;
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
            //controller.ChargingParticles.SetActive(true);
            currentPowerLevel = 0;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();
            
            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            Debug.Log("MAX POWER: " + maxPower);
            for (int i = 0; i < 5; i++)
            {
                if (!merbleBoss.ChargedMerbleList.Contains(merbleBoss.merbleList[i]) && !merbleBoss.merbleList[i].Charging && merbleBoss.merbleList.Count > 0)
                {
                    merbleBoss.merbleList[i].StartCharge(transform.position);
                    Globals.Managers.Audio.PlaySFX("ChargeMerble");
                }
            }

            int j = 0;
            while (true)
            {
                
                currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                //Debug.Log("Current PowerLevel: " + currentPowerLevel);
                merbleBoss.merbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                Merble[] merbleArray = merbleBoss.merbleList.ToArray();

                if (merbleArray.Length > 0)
                {
                    merbleArray[j].StartCharge(transform.position);
                    if (j < maxPower)
                    {
                        //j++;
                    }
                }

                yield return new WaitForSecondsRealtime(chargeTimer);
            }
        }

        public override void StartCharging()
        {
            base.StartCharging();
            ActivateBall();
            StartCoroutine(moveCursorRoutine);
        }

        public override void Fire()
        {
            Cursor.lockState = CursorLockMode.None;
            DropBall();
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            
            _smashBall = Instantiate(Resources.Load<GameObject>("Prefabs/SmashBallPrefab"), transform.position, transform.rotation, transform);
            _smashBall.GetComponent<SmashBall>().SmashAbility = this;
            _smashBall.name = "SmashBall";
            _smashBallRb = _smashBall.GetComponent<Rigidbody>();
            
            _returnPoint = GameObject.Find("ReturnPoint").transform;
            DeactivateBall();
        }
        
        private void ActivateBall()
        {
            //Debug.Log("Activating Ball");
            SmashBall smashBallScript = _smashBall.GetComponent<SmashBall>();
            
            rangeIndicator.ChangeRangeSize(baseRange * maxPowerLevel * 2 );

            _smashBallRb.useGravity = false;
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            newPosition = controller.Movement.adjustedMovement * newPosition;
            newPosition.y = transform.position.y + 5;
            _smashBall.transform.position = newPosition;
            _smashBall.transform.localScale = smashBallScript.BaseScale;

            Vector3 cursorPos = transform.position + GameObject.Find("PlayerModel").transform.forward;
            targetCursor.ChangeCursorPosition(cursorPos);
            targetCursor.ObjectToMove = _smashBall;
            smashBallScript.TriggerCollider.enabled = false;
            _smashBall.SetActive(true);

            currentPowerLevel = basePowerLevel;

            StartCoroutine(MoveMerbles());

            _returnMerbles = false;

            //StartCoroutine(MoveCursor());
        }

        public void DeactivateBall()
        {
            _smashBallRb.linearVelocity = Vector3.zero;
            Merble[] merbleArray = MerbleBoss.ChargedMerbleList.ToArray();
            _smashBall.SetActive(false);
            foreach (var merble in merbleArray)
            {
                if (merble.GroundCheck())
                {
                    //merble.StopCharging();
                    StopCoroutine(MoveMerbles());
                }
                else
                {
                    
                }
            }
            _returnMerbles = true;
            
        }
        private void DropBall()
        {
            //Debug.Log("DropBall");
            Globals.Managers.Audio.PlaySFXHere("ThrowRock", _smashBall.transform);
            _smashBallRb.useGravity = true;
            _smashBall.GetComponent<SmashBall>().TriggerCollider.enabled = true;
            StopCoroutine(moveCursorRoutine);
            StopCoroutine(chargeCoroutine);
            targetCursor.DeactivateCursor();
            Cursor.lockState = CursorLockMode.None;
            //targetCursor.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            rangeIndicator.DisableRangeIndicator();
            foreach (var b in MerbleBoss.ChargedMerbleList)
            {
                //StartCoroutine(b.UseGravity());
            }
        }
        private IEnumerator MoveMerbles()
        {
            //_merbleList =  new List<Merbles.Merble>();
            _returnMerbles = false;
            yield return new WaitUntil(() => merbleBoss.ChargedMerbleList.Count > 0);
            while(merbleBoss.ChargedMerbleList.Count > 0)
            {
                if (!_returnMerbles)
                {
                    List<Merble> merbleList = merbleBoss.ChargedMerbleList;
                    for (int i = 0; i < merbleList.Count; i++)
                    {
                        merbleList[i].FloatTowardsObject(_smashBall.transform.position, i, Merble.AbilityEnum.Smash, 1.5f);
                    }
                }
                else
                {
                    for(int i = 0 ; i < merbleBoss.ChargedMerbleList.Count; i++)
                    {
                        if (!merbleBoss.ChargedMerbleList[i].GroundCheck())
                        {
                            merbleBoss.ChargedMerbleList[i].FloatTowardsObject(_returnPoint.position, i, Merble.AbilityEnum.Smash, 1.5f);
                        }
                        else
                        {
                            merbleBoss.ChargedMerbleList[i].StopCharging();
                        }
                    }
                }
                yield return null;
            }
            merbleBoss.FireMerbles();
        }
    }   
}