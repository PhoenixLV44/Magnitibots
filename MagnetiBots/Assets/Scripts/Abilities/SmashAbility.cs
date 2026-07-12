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
        public GameObject SmashBall => _smashBall;
        private Rigidbody _smashBallRb;

        private IEnumerator moveCursorRoutine;
        private Transform _defaultReturnPoint;
        public Transform DefaultReturnPoint => _defaultReturnPoint;
        private Vector3 _returnPoint;
        public Vector3 ReturnPoint {get => _returnPoint; set => _returnPoint = value; }
        private bool _returnMerbles;
        public bool ReturnMerbles {get => _returnMerbles; set => _returnMerbles = value; }
        private bool _dropMerbles;
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
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(chargeTimer);
            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            //Debug.Log("MAX POWER: " + maxPower);
            yield return wait;
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
                }

                yield return wait;
            }
        }

        public override void StartCharging()
        {
            base.StartCharging();
            ActivateBall();
            //StartCoroutine(moveCursorRoutine);
            _dropMerbles = false;
            controller.Animator.Play("Arm_Up");
        }

        public override void Fire()
        {
            //Cursor.lockState = CursorLockMode.None;
            DropBall();
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            
            _smashBall = Instantiate(Resources.Load<GameObject>("Prefabs/SmashBallPrefab"), transform.position, transform.rotation, transform);
            _smashBall.GetComponent<SmashBall>().SmashAbility = this;
            _smashBall.name = "SmashBall";
            _smashBallRb = _smashBall.GetComponent<Rigidbody>();
            
            _defaultReturnPoint = GameObject.Find("ReturnPoint").transform;
            DeactivateBall();
        }
        
        private void ActivateBall()
        {
            Debug.Log("Activating Ball");
            SmashBall smashBallScript = _smashBall.GetComponent<SmashBall>();
            
            rangeIndicator.ChangeRangeSize(baseRange * maxPowerLevel * 2 );

            _smashBallRb.useGravity = false;
            
            _smashBall.transform.position = controller.ReturnPoint.position;

            Vector3 cursorPos = transform.position + GameObject.Find("PlayerModel").transform.forward;
            targetCursor.ChangeCursorPosition(cursorPos);
            targetCursor.ObjectToMove = _smashBall;
            smashBallScript.TriggerCollider.enabled = false;
            _smashBall.SetActive(true);

            currentPowerLevel = basePowerLevel;

            StartCoroutine(MoveMerbles());

            _returnMerbles = false;
            _returnPoint = Vector3.zero;
            //StartCoroutine(MoveCursor());
        }

        public void DeactivateBall()
        {
            controller.StartChargeLockout();
            _smashBallRb.linearVelocity = Vector3.zero;
            Merble[] merbleArray = MerbleBoss.ChargedMerbleList.ToArray();
            _smashBall.SetActive(false);
            foreach (var merble in merbleArray)
            {
                if (merble.transform.parent == _smashBall.GetComponent<SmashBall>()
                        .MerblePoints[merbleBoss.ChargedMerbleList.IndexOf(merble)])
                {
                    merble.transform.parent = merble.Parent;
                }
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
            //controller.Animator.Play("IdleWalk");
            _dropMerbles = true;
            //_smashBallRb.useGravity = true;
            StartCoroutine(_smashBall.GetComponent<SmashBall>().DropBall());
            targetCursor.ObjectToMove = null;
            _smashBall.GetComponent<SmashBall>().TriggerCollider.enabled = true;
            StopCoroutine(Charge());
            rangeIndicator.DisableRangeIndicator();

        }
        private IEnumerator MoveMerbles()
        {
            //_merbleList =  new List<Merbles.Merble>();
            _returnMerbles = false;
            SmashBall ballScript = _smashBall.GetComponent<SmashBall>();
            yield return new WaitUntil(() => merbleBoss.ChargedMerbleList.Count > 0);
            while(merbleBoss.ChargedMerbleList.Count > 0)
            {
                float speed = 2f;
                if (!_returnMerbles)
                {
                    List<Merble> merbleList = merbleBoss.ChargedMerbleList;
                    for (int i = 0; i < merbleList.Count; i++)
                    {
                        if (merbleList[i].transform.parent != ballScript.MerblePoints[i])
                        {
                            merbleList[i].transform.parent = ballScript.MerblePoints[i];
                        }
                        merbleList[i].FloatTowardsObject(ballScript.MerblePoints[i].position, i, speed);
                    }
                }
                else
                {
                    for(int i = 0 ; i < merbleBoss.ChargedMerbleList.Count; i++)
                    {
                        if (merbleBoss.ChargedMerbleList[i].transform.parent == ballScript.MerblePoints[i])
                        {
                            merbleBoss.ChargedMerbleList[i].transform.parent = merbleBoss.ChargedMerbleList[i].Parent;
                        }
                        var merble = merbleBoss.ChargedMerbleList[i];
                        float distance = Vector3.Distance(merble.transform.position, _returnPoint == Vector3.zero ? _defaultReturnPoint.position : _returnPoint);
                        if (!merble.GroundCheck() || distance > 0.25f)
                        {
                            Debug.Log("Ground check is false;");
                            if (_returnPoint == Vector3.zero)
                            {
                                merble.FloatTowardsObject(_defaultReturnPoint.position, i, speed);
                            }
                            else
                            {
                                merble.FloatTowardsObject(_returnPoint, i, speed);
                            }

                        }
                        else if(merble.GroundCheck() || distance < 0.25f)
                        {
                            merble.StopCharging();
                        }
                    }
                }
                yield return null;
            }
            merbleBoss.FireMerbles();
            StopAllCoroutines();
        }
    }   
}