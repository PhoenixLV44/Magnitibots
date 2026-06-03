using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class Lasso : Parent
    {
        private GameObject _lassoLoop;
        public GameObject LassoLoop => _lassoLoop;

        private LayerMask _layerMask;
        
        
        private void Start()
        {
            InitializeAbility();
            activateInput = InputSystem.actions.FindAction("ActivateLasso");
            chargeInput = InputSystem.actions.FindAction("Charge");
            
            _layerMask = LayerMask.GetMask("LassoTarget");
            
            _lassoLoop = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity);
            _lassoLoop.GetComponent<SphereCollider>().enabled = false;
            _lassoLoop.SetActive(false);
            _lassoLoop.transform.SetParent(transform);
            _lassoLoop.name = "Lasso Loop";
            
        }

        public override void Activate()
        {
            //base.Activate();
            Debug.Log("Activating Lasso Ability");
        }

        public override IEnumerator Charge()
        {
            //Debug.Log("Start Lasso Charge");
            currentPowerLevel = basePowerLevel;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();
            while (true)
            {
                rangeIndicator.ChangeRangeSize((baseRange * currentPowerLevel)* 2);

                yield return new WaitForSecondsRealtime(chargeTimer);

                if(currentPowerLevel < maxPowerLevel)
                    currentPowerLevel++;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public override void Fire()
        {
            isCharging = false;
            RaycastHit hitInfo;
            Vector3 hitPoint;
            Vector3 castPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            
            if (Physics.SphereCast(castPoint, 0.5f, transform.forward, out hitInfo, baseRange * currentPowerLevel, _layerMask))
            {
                Debug.Log("GOT AN OBJECT");
                hitPoint = hitInfo.point;
                hitPoint.y = transform.position.y;
                
                _lassoLoop.transform.position = hitPoint;
                _lassoLoop.SetActive(true);
                
                targetCursor.ActivateCursor(_lassoLoop.transform.position);
                
                hitInfo.collider.gameObject.transform.parent = _lassoLoop.transform;

                hitInfo.collider.gameObject.transform.localPosition = Vector3.zero;
                
                controller.LassoHooked = true;
                
                controller.RangeIndicator.ChangeRangeSize((baseRange * maxPowerLevel) * 2);
            }
            else
            {
                controller.RangeIndicator.DisableRangeIndicator();
                Cursor.lockState =  CursorLockMode.Confined;
                //Debug.Log("MISS");
            }
        }

        public void MoveLassoTarget(/*Vector2 direction*/)
        {
            Vector3 currentPosition = transform.position;
            Vector3 lassoTargetPosition = _lassoLoop.transform.position;
            
            Vector3 distanceVector = lassoTargetPosition - currentPosition;
            distanceVector.y = 0;
            float distance = distanceVector.magnitude;

            if (distance < baseRange * maxPowerLevel)
            {
                _lassoLoop.transform.rotation = Quaternion.identity;

                _lassoLoop.transform.position = new Vector3(targetCursor.MoveCursor().x, _lassoLoop.transform.position.y, targetCursor.MoveCursor().z);
            }
            else if (distance >= baseRange * maxPowerLevel)
            {
                float checkXAxis = distanceVector.x - currentPosition.x;
                float checkZAxis = distanceVector.z - currentPosition.z;

                Vector3 cursorDelta = targetCursor.GetCursorDelta();
                float cursorDeltaPolarity = cursorDelta.x * cursorDelta.z;
                if (checkXAxis > 0 && checkZAxis > 0)
                {
                    if (cursorDeltaPolarity < 0)
                    {
                        _lassoLoop.transform.position = new Vector3(targetCursor.MoveCursor().x, _lassoLoop.transform.position.y, targetCursor.MoveCursor().z);
                    }
                }
                else if (checkXAxis < 0 && checkZAxis < 0)
                {
                    if (cursorDeltaPolarity > 0)
                    {
                        _lassoLoop.transform.position = new Vector3(targetCursor.MoveCursor().x, _lassoLoop.transform.position.y, targetCursor.MoveCursor().z);
                    }
                }
                else if (checkXAxis > 0 && checkZAxis < 0)
                {
                    if (cursorDelta.x <= 0 && cursorDelta.z < 0)
                    {
                        _lassoLoop.transform.position = new Vector3(targetCursor.MoveCursor().x, _lassoLoop.transform.position.y, targetCursor.MoveCursor().z);
                    }
                }
                else if (checkXAxis < 0 && checkZAxis > 0)
                {
                    if (cursorDelta.x > 0 && cursorDelta.z <= 0)
                    {
                        _lassoLoop.transform.position = new Vector3(targetCursor.MoveCursor().x, _lassoLoop.transform.position.y, targetCursor.MoveCursor().z);
                    }
                }
            }
        }
        
        public void UnhookLasso()
        {
            GameObject loopedObject = _lassoLoop.transform.GetChild(0).gameObject;
            loopedObject.transform.parent = null;
            
            controller.LassoHooked = false;
            
            targetCursor.DeactivateCursor();
            _lassoLoop.SetActive(false);
        }

        public override void InitializeAbility()
        {
            base.InitializeAbility();
            baseRange = 5f;
            basePowerLevel = 1;
            maxPowerLevel = 3;
        }
    }
}
