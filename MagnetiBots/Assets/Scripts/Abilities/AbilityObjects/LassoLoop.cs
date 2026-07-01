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
        
        private IEnumerator _moveFowardCoroutine;

        private void Start()
        {
            _lassoMask = LayerMask.GetMask("LassoTarget");
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.enabled = false;
        }

        public void StartMovement(Vector3 startPos,Vector3 target, float speed = 10)
        {
            StartCoroutine(MoveFoward(startPos, target, speed));
        }
        private IEnumerator MoveFoward(Vector3 startPos,Vector3 target, float speed = 10)
        {
            _moveFowardCoroutine =  MoveFoward(startPos, target, speed);
            _lassoAbility.PullMerblesBool = false;
            //Debug.Log("Start Position: " + startPos + " | Target Position: " + target);
            transform.position = startPos;
            _lassoAbility.LoopBeingThrown = true;
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                RaycastHit hit;

                if (Physics.SphereCast(transform.position,0.6f,  transform.forward, out hit, 1, _lassoMask) && !_lassoAbility.Lever)
                {
                    if (hit.collider.CompareTag("Lever"))
                    {
                        Debug.Log("Lever");
                        _lassoAbility.Lever = hit.collider.GetComponent<Interactable.Lever>();
                        _lassoAbility.Lever.Pullalble = true;
                        _lassoAbility.Controller.LassoHooked = true;
                        Vector3 pos = hit.collider.transform.position;
                        pos.y = _lassoAbility.transform.position.y;
                        transform.position = pos;
                        _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                        //_lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);
                        yield break;
                    }
                }
                
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            if (!_lassoAbility.Controller.LassoHooked)
            {
                _lassoAbility.PullMerblesBool = true;
                //StartCoroutine(ReturnToStartPosition(startPos, speed));
                _lassoAbility.StartCoroutine(_lassoAbility.UnhookLasso());
            }
        }

        public IEnumerator ReturnToStartPosition(Vector3 startPos, float speed)
        {
            yield return new WaitUntil(() => _lassoAbility.PullMerblesBool);
            _lassoAbility.TargetCursor.DeactivateCursor();
            StopCoroutine(_moveFowardCoroutine);
            _lassoAbility.Controller.Animator.Play("Pull");
            yield return new WaitForSeconds(_lassoAbility.Controller.AnimController.PullAnimLength / 2);
            while (Vector3.Distance(transform.position, startPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
                yield return null;
            }
            _lassoAbility.MerbleBoss.FireMerbles();
            _lassoAbility.PullMerblesBool = false;
            _lassoAbility.StopCoroutine(_lassoAbility.merbleLineCoroutine);
            if (!_lassoAbility.Controller.LassoHooked)
            {
                
            }
            //gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            StopCoroutine(_moveFowardCoroutine);
            if (!_lassoAbility.Controller.LassoHooked)
            {
                if (other.CompareTag("LassoTarget"))
                {
                    Debug.Log("LassoTarget");
                    _lassoAbility.Controller.LassoHooked = true;
                    GameObject hookedObject = other.GetComponent<ItemRespawner>() ? other.gameObject : other.transform.parent.gameObject;


                    transform.position = hookedObject.transform.position;
                    Vector3 defaultScale = transform.localScale.y == 1? hookedObject.transform.localScale: new Vector3(hookedObject.transform.localScale.x, hookedObject.transform.localScale.y * 2, hookedObject.transform.localScale.z);

                    _lassoAbility.TargetCursor.ActivateCursor(transform.position);
                    hookedObject.transform.parent = transform;

                    hookedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    hookedObject.transform.localScale = defaultScale;

                    PuzzleCube puzzleCube = other.GetComponent<PuzzleCube>();
                    if (puzzleCube != null)
                    {
                        puzzleCube.FreezeConstraints();
                        //puzzleCube.ResetTransform();
                    }
                    _lassoAbility.Controller.RangeIndicator.ChangeRangeSize((_lassoAbility.BaseRange * _lassoAbility.MaxPowerLevel) * 2);
                    //_lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);
                    _boxCollider.enabled = true;
                    //StartCoroutine(_lassoAbility.MoveLassoTarget());
                }
                else if (other.CompareTag("Lever"))
                {
                    Debug.Log("Lever");
                    _lassoAbility.Controller.LassoHooked = true;
                    transform.position = other.transform.position;
                    _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                    //_lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine);
                }
                else
                {
                    _lassoAbility.PullMerblesBool = true;
                }
            }
            else
            {
                //_lassoAbility.PullMerblesBool = true;
            }

        }

        private void Update()
        {
            /*RaycastHit hit;
            if (!_lassoAbility.Lever)
            {
                if (Physics.SphereCast(transform.position,0.6f,  transform.forward, out hit, 1, _lassoMask))
                {
                    StopCoroutine(_moveFowardCoroutine);
                    if (hit.collider.CompareTag("Lever"))
                    {
                        Debug.Log("Lever");
                        _lassoAbility.Lever = hit.collider.GetComponent<Interactable.Lever>();
                        _lassoAbility.Lever.Pullalble = true;
                        _lassoAbility.Controller.LassoHooked = true;
                        transform.position = hit.collider.transform.position;
                        _lassoAbility.Controller.RangeIndicator.DisableRangeIndicator();
                        //_lassoAbility.StartCoroutine(_lassoAbility.MerbleLineCoroutine); 
                    }
                }
            }*/
        }
    }
}
