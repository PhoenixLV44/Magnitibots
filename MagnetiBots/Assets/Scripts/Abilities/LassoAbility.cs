using Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class Lasso : Parent
    {
        private GameObject _lassoLoop;
        public GameObject LassoLoop => _lassoLoop;

        private LayerMask _layerMask;

        private Interactable.Lever lever;
        public Interactable.Lever Lever => lever;

        private Vector3 _lassoLoopScale;
        [SerializeField] private float loopHeight;
        
        private void Start()
        {
            InitializeAbility();
            activateInput = InputSystem.actions.FindAction("ActivateLasso");
            chargeInput = InputSystem.actions.FindAction("Charge");
        }

        public override void Activate()
        {
            //base.Activate();
            //Debug.Log("Activating Lasso Ability");
        }

        public override IEnumerator Charge()
        {
            //Debug.Log("Start Lasso Charge");
            currentPowerLevel = basePowerLevel;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();
            aimingGuide.SetActive(true);
            while (true)
            {
                if (aimingGuide.activeSelf)
                {
                    Debug.Log("Aiming Guid Active");
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
            
            if (Physics.SphereCast(castPoint, 0.5f, playerModel.transform.forward, out hitInfo, baseRange * currentPowerLevel, _layerMask))
            {
                //Debug.Log("GOT AN OBJECT");
                hitPoint = hitInfo.point;
                Vector3 position = new Vector3(hitPoint.x, _lassoLoop.transform.position.y, hitPoint.z);
                
                _lassoLoop.transform.position = hitPoint;
                _lassoLoop.transform.parent = null;
                _lassoLoop.SetActive(true);

                if (hitInfo.collider.CompareTag("Lever"))
                {
                    _lassoLoop.transform.localScale = _lassoLoopScale;
                    lever = hitInfo.collider.GetComponent<Lever>();
                    controller.RangeIndicator.DisableRangeIndicator();
                    controller.LassoHooked = true;
                }
                else
                {
                    Vector3 newLassoScale = new Vector3(hitInfo.collider.transform.localScale.x, _lassoLoopScale.y, hitInfo.collider.transform.localScale.z);
                    targetCursor.ActivateCursor(_lassoLoop.transform.position);

                    hitInfo.collider.gameObject.transform.parent = _lassoLoop.transform;

                    hitInfo.collider.gameObject.transform.localPosition = Vector3.zero;
                    Rigidbody rb = hitInfo.collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                    }
                    controller.RangeIndicator.ChangeRangeSize((baseRange * maxPowerLevel) * 2);
                    controller.LassoHooked = true;
                }
                StartCoroutine(FormLineOfMerbles());
            }
            else
            {
                controller.RangeIndicator.DisableRangeIndicator();
                //Cursor.lockState =  CursorLockMode.None;
                //Debug.Log("MISS");
            }
        }

        public void MoveLassoTarget(/*Vector2 direction*/)
        {
            targetCursor.MoveObjectToCursor(_lassoLoop);
        }
        
        public void UnhookLasso()
        {
            if (_lassoLoop.transform.childCount > 0)
            {
                GameObject loopedObject = _lassoLoop.transform.GetChild(0).gameObject;
                _lassoLoop.transform.parent = transform;
                Rigidbody rb = loopedObject.GetComponent<Rigidbody>();
                rb.useGravity = true;
                loopedObject.transform.parent = null;
            }
            
            _lassoLoop.SetActive(false);

            targetCursor.DeactivateCursor();

            //Cursor.lockState = CursorLockMode.None;

            controller.LassoHooked = false;
            StopCoroutine(FormLineOfMerbles());
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            baseRange = 5f;
            basePowerLevel = 1;
            maxPowerLevel = 3;
            _lassoLoop = transform.Find("Lasso Loop").gameObject;
            _lassoLoop.SetActive(false);
            _layerMask = LayerMask.GetMask("LassoTarget");
            _lassoLoopScale = _lassoLoop.transform.localScale;
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

        private IEnumerator FormLineOfMerbles()
        {
            List<Merbles.Merble> chargedMerbleList = merbleBoss.ChargedMerbleList;
            Vector2 distanceBetweenMerblesMinMax = new Vector2(1, 2);
            while (true)
            {
                Transform pivotPoint = controller.Movement.model.transform;
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                List<Merbles.Merble> merbleList = merbleBoss.merbleList;
                
                merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));

                if (Vector3.Distance(transform.position, _lassoLoop.transform.position) >
                    chargedMerbleList.Count * distanceBetweenMerblesMinMax.y)
                {
                    chargedMerbleList.Add(merbleList[0]);
                    merbleList.RemoveAt(0);
                }
                else if (Vector3.Distance(transform.position, _lassoLoop.transform.position) <=
                         chargedMerbleList.Count * distanceBetweenMerblesMinMax.x)
                {
                    chargedMerbleList[chargedMerbleList.Count - 1].transform.position = transform.position;
                    merbleList.Add(chargedMerbleList[chargedMerbleList.Count - 1]);
                    chargedMerbleList.RemoveAt(chargedMerbleList.Count - 1);
                }
                
                merbleBoss.ChargedMerbleList = chargedMerbleList;
                merbleBoss.merbleList = merbleList;
                
                Debug.Log("Blep");
                for (int i = chargedMerbleList.Count - 1; i >= 0; i--)
                {
                    /* From the player to the object
                    */
                     float rng = Random.Range(distanceBetweenMerblesMinMax.x, distanceBetweenMerblesMinMax.y);

                     

                     //Vector3 firstMerblePoint =/* controller.Movement.model.rotation **/transform.position + controller.Movement.model.transform.forward * 1.5f;
                    
                    Vector3 merblePoint = new Vector3(pivotPoint.position.x, pivotPoint.position.y,pivotPoint.position.z + (i * 1.5f));
                    merblePoint = pivotPoint.rotation * merblePoint;
                    
                    merblePoint = Quaternion.Euler(_lassoLoop.transform.position - transform.position) * merblePoint;
                    
                    Debug.DrawRay(transform.position, merblePoint, Color.red);
                    if (Vector3.Distance(merblePoint, transform.position) < Vector3.Distance(transform.position, _lassoLoop.transform.position))
                    {
                        //chargedMerbleList[i].transform.position =  Vector3.Lerp(chargedMerbleList[i].transform.position,merblePoint, 10 * Time.deltaTime);
                        chargedMerbleList[i].transform.position = merblePoint;
                    }
                    else
                    {
                        chargedMerbleList[i].transform.position = transform.position;
                    }
                    /*From object to player
                    //_lassoLoop.transform.LookAt(transform.position);
                    float rng = Random.Range(distanceBetweenMerblesMinMax.x, distanceBetweenMerblesMinMax.y);
                    Vector3 merblePoint = new Vector3(_lassoLoop.transform.position.x, transform.position.y, _lassoLoop.transform.position.z + (i * 1.5f));
                    Debug.Log("QUAT " +Quaternion.Euler(_lassoLoop.transform.position - transform.position));
                    merblePoint = Quaternion.Euler(_lassoLoop.transform.position - transform.position) * merblePoint;
                    if (Vector3.Distance(merblePoint, transform.position) < Vector3.Distance(transform.position, _lassoLoop.transform.position))
                    {
                        chargedMerbleList[i].transform.position = merblePoint;
                        //chargedMerbleList[i].transform.position =  Vector3.Lerp(chargedMerbleList[i].transform.position,merblePoint, 10 * Time.deltaTime);
                    }
                    else
                    {
                        chargedMerbleList[i].transform.position = transform.position;
                    }*/
                    yield return null;
                }
            }
        }
    }
}
