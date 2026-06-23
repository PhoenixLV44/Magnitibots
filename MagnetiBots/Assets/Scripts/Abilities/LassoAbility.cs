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
            //aimingGuide.SetActive(true);
            while (true)
            {
                //if (aimingGuide.activeSelf)
                {
                   // Debug.Log("Aiming Guid Active");
                }
                rangeIndicator.ChangeRangeSize((baseRange * currentPowerLevel));

                yield return new WaitForSecondsRealtime(chargeTimer);

                if (currentPowerLevel < maxPowerLevel)
                    currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public override void Fire()
        {
            isCharging = false;
            RaycastHit hitInfo;
            Vector3 hitPoint;
            
            Vector3 castPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            
            GameObject playerModel = transform.Find("PlayerModel").gameObject;
            Vector3 target = playerModel.transform.forward * baseRange * merbleBoss.ChargedMerbleList.Count;
            target.y = transform.position.y + 0.5f;
            
            _lassoLoopObject.transform.rotation = playerModel.transform.rotation;
            _lassoLoopObject.transform.parent = null;
            _lassoLoopObject.SetActive(true);
            _loopScript.StartMovement(transform.position,target);
            
            /*if (Physics.SphereCast(castPoint, 0.5f, playerModel.transform.forward, out hitInfo, baseRange * currentPowerLevel, _layerMask))
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
            baseRange = 5f;
            basePowerLevel = 1;
            maxPowerLevel = 3;
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

                float minDistance = chargedMerbleList.Count * distanceBetweenMerblesMinMax.x;
                float maxDistance = minDistance + distanceBetweenMerblesMinMax.y;
                
                merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

                float distance = Vector3.Distance(pivotPoint.position, _lassoLoopObject.transform.position);
                Debug.Log("MIN: " +  minDistance + " | MAX: " + maxDistance +" | DISTANCE: " + distance);
                
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
    }
}
