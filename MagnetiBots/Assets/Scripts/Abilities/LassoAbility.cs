using Interactable;
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

        private Interactable.Lever lever;
        public Interactable.Lever Lever => lever;
        
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
            
            GameObject playerModel = transform.Find("PlayerModel").gameObject;
            
            if (Physics.SphereCast(castPoint, 0.5f, playerModel.transform.forward, out hitInfo, baseRange * currentPowerLevel, _layerMask))
            {
                //Debug.Log("GOT AN OBJECT");
                hitPoint = hitInfo.point;
                hitPoint.y = transform.position.y;
                
                _lassoLoop.transform.position = hitPoint;
                _lassoLoop.SetActive(true);

                if (hitInfo.collider.CompareTag("Lever"))
                {
                    lever = hitInfo.collider.GetComponent<Lever>();
                    controller.RangeIndicator.DisableRangeIndicator();
                    controller.LassoHooked = true;
                }
                else
                {
                    targetCursor.ActivateCursor(_lassoLoop.transform.position);

                    hitInfo.collider.gameObject.transform.parent = _lassoLoop.transform;

                    hitInfo.collider.gameObject.transform.localPosition = Vector3.zero;

                    controller.RangeIndicator.ChangeRangeSize((baseRange * maxPowerLevel) * 2);
                    controller.LassoHooked = true;
                }
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
            //Vector3 currentPosition = _lassoLoop.transform.position;
            //Vector3 lassoTargetPosition = targetCursor.MoveCursor();
            //lassoTargetPosition.y = currentPosition.y;
            
            //_lassoLoop.transform.position = Vector3.Lerp(currentPosition, lassoTargetPosition, Time.deltaTime);
            
        }
        
        public void UnhookLasso()
        {
            if (_lassoLoop.transform.childCount > 0)
            {
                GameObject loopedObject = _lassoLoop.transform.GetChild(0).gameObject;
                loopedObject.transform.parent = null;
            }
            
            _lassoLoop.SetActive(false);

            targetCursor.DeactivateCursor();

            //Cursor.lockState = CursorLockMode.None;

            controller.LassoHooked = false;
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
        }

        public void PullLever()
        {
            lever.ActivateObject();
            lever = null;
            UnhookLasso();
        }
    }
}
