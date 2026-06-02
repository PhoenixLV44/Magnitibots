using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

namespace Ability
{
    public class Lasso : Parent
    {
        private float _baseRange = 5f;
        private int _basePowerLevel = 1;
        private int _maxPowerLevel = 5;
        private int _currentPowerLevel = 1;
        
        private float _moveLassoSpeed = 15f;
        private float _rotateLassoSpeed = 5f;
        
        private GameObject _lassoLoop;

        public GameObject LassoLoop => _lassoLoop;

        private LayerMask _layerMask;
        
        private TargetingCursor _targetCursor;
        private void Start()
        {
            activateInput = InputSystem.actions.FindAction("ActivateLasso");
            chargeInput = InputSystem.actions.FindAction("Charge");
            fireInput = InputSystem.actions.FindAction("Fire");
            
            _layerMask = LayerMask.GetMask("LassoTarget");
            
            _lassoLoop = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity, transform);
            _lassoLoop.GetComponent<SphereCollider>().enabled = false;
            _lassoLoop.SetActive(false);
            _lassoLoop.name = "Lasso Loop";
            
            _targetCursor = GetComponent<TargetingCursor>();
        }

        private void OnEnable()
        {
            StartCoroutine(Charge());
        }

        private void OnDisable()
        {
            Debug.Log("Disabling Lasso Ability");
            StopCoroutine(Charge());
        }

        public override void Activate()
        {
            //base.Activate();
            Debug.Log("Activating Lasso Ability");
        }

        public override IEnumerator Charge()
        {
            Debug.Log("Start Lasso Charge");
            float chargeTimer = 0.5f;
            while (true)
            {
                if (isCharging && _currentPowerLevel < _maxPowerLevel)
                {
                    yield return new WaitForSeconds(chargeTimer);
                    Debug.Log("Current Charge: " + _currentPowerLevel);
                    _currentPowerLevel++;
                }
                else if(!isCharging &&  _currentPowerLevel != _basePowerLevel)
                {
                    _currentPowerLevel = _basePowerLevel;
                }
                yield return null;
            }
        }

        public override void Fire()
        {
            if (isCharging)
            {
                isCharging = false;
            }
            RaycastHit hitInfo;
            Vector3 hitPoint;
            Vector3 castPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            if (Physics.SphereCast(castPoint, 0.5f, transform.forward, out hitInfo, _baseRange * _currentPowerLevel, _layerMask))
            {
                Debug.Log("GOT AN OBJECT");
                hitPoint = hitInfo.point;
                
                _lassoLoop.transform.position = hitPoint;
                _lassoLoop.SetActive(true);
                
                _targetCursor.ActivateCursor(_lassoLoop.transform.position);
                
                hitInfo.collider.gameObject.transform.parent = _lassoLoop.transform;
                
                gameObject.GetComponent<Player.Controller>().LassoHooked = true;
            }
            else
            {
                Debug.Log("MISS");
            }
        }

        public void MoveLassoTarget(/*Vector2 direction*/)
        {
            Vector3 currentPosition = transform.position;
            Vector3 lassoTargetPosition = _lassoLoop.transform.position;
            
            Vector3 distanceVector = lassoTargetPosition - currentPosition;
            distanceVector.y = 0;
            float distance = distanceVector.magnitude;

            if (distance <= _baseRange * _maxPowerLevel)
            {
                _lassoLoop.transform.rotation = Quaternion.identity;
                
                _lassoLoop.transform.position = _targetCursor.MoveCursor();
            }


        }
        
        public void UnhookLasso()
        {
            GameObject loopedObject = _lassoLoop.transform.GetChild(0).gameObject;
            loopedObject.transform.parent = null;
            
            _targetCursor.DeactivateCursor();
            _lassoLoop.SetActive(false);
        }
    }
}
