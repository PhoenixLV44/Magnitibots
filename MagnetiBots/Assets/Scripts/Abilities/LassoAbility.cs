using System;
using Interactable;
using System.Collections;
using System.Collections.Generic;
using Merbles;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace Ability
{
    public class Lasso : Parent
    {
        private GameObject _lassoLoopObject;
        public GameObject LassoLoopObject => _lassoLoopObject;
        private Ability.Object.LassoLoop _loopScript;

        private LayerMask _layerMask;

        private Interactable.Lever lever;
        public Interactable.Lever Lever => lever;
        
        [SerializeField] private float loopHeight;

        private IEnumerator _merbleLineCoroutine;
        public IEnumerator MerbleLineCoroutine => _merbleLineCoroutine;

        private bool loopBeingThrown;

        public bool LoopBeingThrown
        { get => loopBeingThrown; set => loopBeingThrown = value; }

        private void Start()
        {
            InitializeAbility();
        }

        public override IEnumerator Charge()
        {
            //Debug.Log("Start Lasso Charge");
            currentPowerLevel = basePowerLevel;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();
            
            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            Debug.Log("MAX POWER: " + maxPower);
            for (int i = 0; i < 5; i++)
            {
                if (!merbleBoss.ChargedMerbleList.Contains(merbleBoss.merbleList[i]) && !merbleBoss.merbleList[i].Charging)
                {
                    merbleBoss.merbleList[i].StartCharge(transform.position);
                }
            }

            int j = 0;
            //aimingGuide.SetActive(true);
            while (true)
            {
                /*
                if (currentPowerLevel < maxPowerLevel)
                {
                    currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                    
                    rangeIndicator.ChangeRangeSize((baseRange * currentPowerLevel));
                    
                    yield return new WaitForSecondsRealtime(chargeTimer);
                    merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                    
                    merbleBoss.merbleList[0].StartCharge(transform.position);
                }
                */
                currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                Debug.Log("Current PowerLevel: " + currentPowerLevel);
                rangeIndicator.ChangeRangeSize((baseRange * currentPowerLevel));
                
                merbleBoss.merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position))); 
                Merble[] merbleArray = merbleBoss.merbleList.ToArray();

                if (!merbleBoss.ChargedMerbleList.Contains(merbleArray[j]) && !merbleArray[j].Charging)
                {
                    merbleArray[j].StartCharge(transform.position);
                    if (j < maxPower)
                    {
                        j++;
                    }
                }
                
                yield return new WaitForSecondsRealtime(chargeTimer);
                
                /*if (currentPowerLevel < maxPower)
                {
                    foreach (var merble in merbleArray)
                    {

                            if (!merbleBoss.ChargedMerbleList.Contains(merble) && !merble.Charging)
                            {
                                merble.StartCharge(transform.position);
                            }
                    }
                }*/

                //yield return null;
            }
        }

        public override void Fire()
        {
            isCharging = false;
            
            Vector3 castPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            
            GameObject playerModel = transform.Find("PlayerModel").gameObject;
            Vector3 target = playerModel.transform.forward * baseRange * merbleBoss.ChargedMerbleList.Count;
            target.y = transform.position.y + 0.5f;

            if (currentPowerLevel > 0)
            {
                _lassoLoopObject.transform.rotation = playerModel.transform.rotation;
                _lassoLoopObject.transform.parent = null;
                _lassoLoopObject.SetActive(true);
                _loopScript.StartMovement(transform.position,target);
            }
            
            /*
            RaycastHit hitInfo;
            Vector3 hitPoint;
             if (Physics.SphereCast(castPoint, 0.5f, playerModel.transform.forward, out hitInfo, baseRange * currentPowerLevel, _layerMask))
            {
                //Debug.Log("GOT AN OBJECT");
                hitPoint = hitInfo.point;
                Vector3 position = new Vector3(hitPoint.x, _lassoLoopObject.transform.position.y, hitPoint.z);
                
                _lassoLoopObject.transform.position = hitPoint;
                _lassoLoopObject.transform.parent = null;
                _lassoLoopObject.SetActive(true);

                if (hitInfo.collider.CompareTag("Lever"))
                {
                    lever = hitInfo.collider.GetComponent<Lever>();
                    controller.RangeIndicator.DisableRangeIndicator();
                    controller.LassoHooked = true;
                }
                else
                {
                    targetCursor.ActivateCursor(_lassoLoopObject.transform.position);

                    hitInfo.collider.gameObject.transform.parent = _lassoLoopObject.transform;

                    hitInfo.collider.gameObject.transform.localPosition = Vector3.zero;
                    Rigidbody rb = hitInfo.collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                    }
                    controller.RangeIndicator.ChangeRangeSize((baseRange * maxPowerLevel) * 2);
                    controller.LassoHooked = true;
                }
                StartCoroutine(_merbleLine);
            }
            else
            {
                controller.RangeIndicator.DisableRangeIndicator();
                //Cursor.lockState =  CursorLockMode.None;
                //Debug.Log("MISS");
                if (!_lassoLoopObject.gameObject.activeSelf)
                {
                    merbleBoss.FireMerbles();
                }
            }*/
        }

        public void MoveLassoTarget(/*Vector2 direction*/)
        {
            targetCursor.MoveObjectToCursor(_lassoLoopObject);
        }
        
        public void UnhookLasso()
        {
            targetCursor.DeactivateCursor();
            Cursor.lockState = CursorLockMode.None;
            if (_lassoLoopObject.transform.childCount > 0)
            {
                GameObject loopedObject = _lassoLoopObject.transform.GetChild(0).gameObject;
                _lassoLoopObject.transform.parent = transform;
                Rigidbody rb = loopedObject.GetComponent<Rigidbody>();
                rb.useGravity = true;
                loopedObject.transform.parent = null;
            }
            
            _lassoLoopObject.SetActive(false);

            rangeIndicator.DisableRangeIndicator();

            controller.LassoHooked = false;
            StopCoroutine(_merbleLineCoroutine);
            merbleBoss.FireMerbles();
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            baseRange = 1f;
            basePowerLevel = 1;
            maxPowerLevel = 15;
            _lassoLoopObject = transform.Find("Lasso Loop").gameObject;
            _lassoLoopObject.SetActive(false);
            _layerMask = LayerMask.GetMask("LassoTarget");
            _merbleLineCoroutine = MerbleLine();
            _loopScript = _lassoLoopObject.AddComponent<Object.LassoLoop>();
            _loopScript.LassoAbility = this;
        }

        public void PullLever()
        {
            if (!lever.Activated)
            {
                lever.ActivateObject();
            }
            else if (lever.Activated && lever.canBeDeactivated)
            {
                lever.DeactivateObject();
            }
            lever = null;
            UnhookLasso();
        }

        private IEnumerator MerbleLine()
        {
            List<Merbles.Merble> chargedMerbleList;
            List<Merbles.Merble> merbleList;
            Vector2 distanceBetweenMerblesMinMax = new Vector2(1, 2);
            Transform pivotPoint = controller.Movement.model.GetChild(8);
            float speed = targetCursor.ObjectSpeed + 0.5f;
            while (true)
            {
                _lassoLoopObject.transform.LookAt(pivotPoint);
                _lassoLoopObject.transform.rotation = Quaternion.Euler(0, _lassoLoopObject.transform.eulerAngles.y, 0);
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                merbleList = merbleBoss.merbleList;

                Tuple<List<Merble>, List<Merble>> listReturn = CalculateMerblesNeeded(merbleList, chargedMerbleList);
                merbleList = listReturn.Item1;
                chargedMerbleList = listReturn.Item2;
                
                foreach (var merble in chargedMerbleList)
                {
                    merble.transform.parent = _lassoLoopObject.transform;
                    int index = chargedMerbleList.IndexOf(merble);
                    Vector3 pos = _lassoLoopObject.transform.position;
                    if (index == 0)
                        pos += _lassoLoopObject.transform.forward * (1.5f);
                    else
                        pos += _lassoLoopObject.transform.forward * (1.5f * (index + 1));
                    
                    merble.FloatTowardsObject(pos, index, Merble.AbilityEnum.Lasso, speed);
                }

                yield return null;
            }
        }

        private Tuple<List<Merble>, List<Merble>> CalculateMerblesNeeded(List<Merble> merbleList, List<Merble> chargedMerbleList)
        {
            Vector2 distanceBetweenMerblesMinMax = new Vector2(1, 2);
            Transform pivotPoint = controller.Movement.model.GetChild(8);
            
            float minDistance = chargedMerbleList.Count * distanceBetweenMerblesMinMax.x;
            float maxDistance = minDistance + distanceBetweenMerblesMinMax.y;
                
            merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

            float distance = Vector3.Distance(pivotPoint.position, _lassoLoopObject.transform.position);
            //Debug.Log("MIN: " +  minDistance + " | MAX: " + maxDistance +" | DISTANCE: " + distance);
                
            merbleBoss.merbleList = merbleList;
            if (distance > maxDistance)
            {
                if (!chargedMerbleList.Contains(merbleList[0]))
                {
                    merbleList[0].StartCharge(transform.position);
                    merbleList =  merbleBoss.merbleList;
                }
            }
            else if (distance <= minDistance)
            {
                if (!merbleList.Contains(chargedMerbleList.Last()))
                {
                    chargedMerbleList.Last().StopCharging();
                    chargedMerbleList = merbleBoss.ChargedMerbleList;
                }
            }
            return Tuple.Create(chargedMerbleList, chargedMerbleList);
        }
    }
}
