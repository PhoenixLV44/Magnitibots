using System;
using System.Collections;
using UnityEngine;

namespace Ability.Object
{
    public class LassoLoop : MonoBehaviour
    {
        private Ability.Lasso _lassoAbility;
        public Lasso LassoAbility {get => _lassoAbility; set => _lassoAbility = value; }

        private LayerMask _lassoMask;

        private void Start()
        {
            _lassoMask = LayerMask.GetMask("LassoTarget");
        }

        public void StartMovement(Vector3 startPos,Vector3 target, float speed = 5)
        {
            StartCoroutine(MoveFoward(startPos, target, speed));
        }
        private IEnumerator MoveFoward(Vector3 startPos,Vector3 target, float speed = 5)
        {
            Debug.Log("Start Position: " + startPos + " | Target Position: " + target);
            transform.position = startPos;
            _lassoAbility.LoopBeingThrown = true;
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            _lassoAbility.LoopBeingThrown = false;
            if (Vector3.Distance(transform.position, startPos) > Vector3.Distance(startPos, target))
            {
            }
            transform.position = startPos;
            gameObject.SetActive(false);
            _lassoAbility.MerbleBoss.FireMerbles();
        }

        private void OnTriggerEnter(Collider other)
        {
            StopAllCoroutines();
            if (other.CompareTag("LassoTarget"))
            {
                Debug.Log("LassoTarget");
                _lassoAbility.Controller.LassoHooked = true;

                transform.position = other.transform.position;
                _lassoAbility.TargetCursor.ActivateCursor(transform.position);
                other.transform.parent = transform;
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                }
                _lassoAbility.Controller.RangeIndicator.ChangeRangeSize((_lassoAbility.BaseRange * _lassoAbility.MaxPowerLevel) * 2);
                _lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);
            }
            else if (other.CompareTag("Lever"))
            {
                Debug.Log("Lever");
                _lassoAbility.Controller.LassoHooked = true;
                transform.position = other.transform.position;
                _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                _lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);    
            }
            else
            {
                _lassoAbility.MerbleBoss.FireMerbles();
            }

        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position,0.6f,  transform.forward, out hit, 1, _lassoMask))
            {
                if (hit.collider.CompareTag("Lever"))
                {
                    Debug.Log("Lever");
                    _lassoAbility.Lever = hit.collider.GetComponent<Interactable.Lever>();
                    _lassoAbility.Controller.LassoHooked = true;
                    transform.position = hit.collider.transform.position;
                    _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                    _lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine); 
                }
            }
        }
    }
}
