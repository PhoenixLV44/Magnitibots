using Interactable;
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
        private BoxCollider _boxCollider;
        public BoxCollider BoxCollider => _boxCollider;

        private void Start()
        {
            _lassoMask = LayerMask.GetMask("LassoTarget");
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.enabled = false;
        }

        public void StartMovement(Vector3 startPos,Vector3 target, float speed = 5)
        {
            StartCoroutine(MoveFoward(startPos, target, speed));
        }
        private IEnumerator MoveFoward(Vector3 startPos,Vector3 target, float speed = 5)
        {
            //Debug.Log("Start Position: " + startPos + " | Target Position: " + target);
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
            if (!_lassoAbility.Controller.LassoHooked)
            {
                if (other.CompareTag("LassoTarget"))
                {
                    Debug.Log("LassoTarget");
                    _lassoAbility.Controller.LassoHooked = true;
                    GameObject hookedObject = other.GetComponent<ItemRespawner>() ? other.gameObject : other.transform.parent.gameObject;


                    transform.position = hookedObject.transform.position;
                    Vector3 defaultScale = transform.localScale.y == 1? hookedObject.transform.localScale: new Vector3(hookedObject.transform.localScale.x, hookedObject
                        .transform.localScale.y * 2, hookedObject.transform.localScale.z);

                    _lassoAbility.TargetCursor.ActivateCursor(transform.position);
                    hookedObject.transform.parent = transform;

                    hookedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    hookedObject.transform.localScale = defaultScale;

                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    }
                    _lassoAbility.Controller.RangeIndicator.ChangeRangeSize((_lassoAbility.BaseRange * _lassoAbility.MaxPowerLevel) * 2);
                    _lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);
                    _boxCollider.enabled = true;
                }
                else if (other.CompareTag("Lever"))
                {
                    Debug.Log("Lever");
                    _lassoAbility.Controller.LassoHooked = true;
                    transform.position = other.transform.position;
                    _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                    _lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);
                }
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
                    _lassoAbility.Lever.Pullalble = true;
                    _lassoAbility.Controller.LassoHooked = true;
                    transform.position = hit.collider.transform.position;
                    _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                    _lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine); 
                    StopAllCoroutines();
                }
            }
        }
    }
}
