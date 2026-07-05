using Ability.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Merbles;
using UnityEngine;

namespace Ability
{
    public class Lasso : Parent
    {
        private GameObject _lassoLoop;
        public GameObject LassoLoop => _lassoLoop;
        private Object.LassoLoop _loopScript;

        private GameObject _loopedObject;
        public GameObject LoopedObject {get => _loopedObject; set => _loopedObject = value;}

        private LayerMask _layerMask;

        private Interactable.Lever _lever;

        public Interactable.Lever Lever
        {
            get => _lever;
            set => _lever = value;
        }

        [SerializeField] private float loopHeight;

        public IEnumerator merbleLineCoroutine;
            
        private Transform _returnPoint;

        private void Start()
        {
            InitializeAbility();
        }

        public override IEnumerator Charge()
        {
            currentPowerLevel = 0;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();

            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) =>
                Vector3.Distance(a.transform.position, transform.position)
                    .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            Debug.Log("MAX POWER: " + maxPower);
            for (int i = 0; i < 5; i++)
            {
                if (!merbleBoss.ChargedMerbleList.Contains(merbleBoss.merbleList[i]) &&
                    !merbleBoss.merbleList[i].Charging && merbleBoss.merbleList.Count > 0)
                {
                    merbleBoss.merbleList[i].StartCharge(transform.position);
                    Globals.Managers.Audio.PlaySFX("ChargeMerble");
                }
            }

            int j = 0;
            while (true)
            {
                //Debug.Log("Charging");
                currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                //Debug.Log("Current PowerLevel: " + currentPowerLevel);
                rangeIndicator.ChangeRangeSize((baseRange * currentPowerLevel * 2));

                merbleBoss.merbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                Merble[] merbleArray = merbleBoss.merbleList.ToArray();

                if (merbleArray.Length > 0)
                {
                    merbleArray[j].StartCharge(transform.position);
                    if (j < maxPower)
                    {
                        //j++;
                    }
                }

                yield return new WaitForSecondsRealtime(chargeTimer);
            }
        }

        public override void Fire()
        {
            isCharging = false;
            if (merbleBoss.ChargedMerbleList.Count >= 1)
            {
                if (FindFirstObjectByType<Globals>() != null)
                {
                    Globals.Managers.Audio.PlaySFX("ThrowLasso");
                }
                GameObject playerModel = transform.Find("PlayerModel").gameObject;
                Vector3 target = transform.position;
                target += playerModel.transform.forward * (baseRange * merbleBoss.ChargedMerbleList.Count);
                //Debug.Log("Target: " + target);
                target.y = transform.position.y + 0.5f;
                _lassoLoop.transform.rotation = playerModel.transform.rotation;
                _lassoLoop.transform.parent = null;
                _lassoLoop.SetActive(true);
                _loopScript.StartMovement(_returnPoint.position, target);
                controller.Animator.Play("Throw");
                StartCoroutine(merbleLineCoroutine);
            }

            //StopCoroutine(Charge());
            merbleBoss.FireMerbles();
            currentPowerLevel = 0;
        }

        public void MoveLassoTarget()
        {
            targetCursor.ObjectToMove = _lassoLoop;
        }

        public IEnumerator UnhookLasso()
        {
            targetCursor.CanMoveCursor = false;
            _loopScript.BoxCollider.enabled = false;
            
            PuzzleCube puzzleCube = null;
            
            controller.Animator.Play("Pull");
            yield return new WaitForSeconds(controller.AnimController.PullAnimLength / 2);
            
            if (_loopedObject)
            {
                _lassoLoop.transform.parent = transform;
                if (_loopedObject.CompareTag("LassoTarget"))
                {
                    puzzleCube = _loopedObject.GetComponent<PuzzleCube>();
                    if (puzzleCube)
                    {
                        //puzzleCube.ResetTransform();
                        Debug.Log("Dropping Puzzle Cube");
                        puzzleCube.ChangeGravity(true);
                    }
                }
                else if (_loopedObject.CompareTag("Lever"))
                {
                    PullLever();
                }

                _loopedObject.transform.parent = null;
                _loopedObject = null;
            }
            while (Vector3.Distance(_lassoLoop.transform.position, controller.ReturnPoint.transform.position) > 0.1f)
            {
                _lassoLoop.transform.position = Vector3.MoveTowards(_lassoLoop.transform.position,controller.ReturnPoint.transform.position, 10 * Time.deltaTime);
                targetCursor.SetRayCastPosition(_lassoLoop.transform.position);
                yield return null;
            }

            if (puzzleCube)
            {
                puzzleCube.UnfreezeConstraints();
            }
            rangeIndicator.DisableRangeIndicator();
            
            _lassoLoop.transform.parent = transform;
            _lassoLoop.SetActive(false);
            
            controller.LassoHooked = false;
            
            StopCoroutine(merbleLineCoroutine);
            merbleBoss.FireMerbles();

            targetCursor.CanMoveCursor = true;
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            baseRange = 2f;
            basePowerLevel = 1;
            maxPowerLevel = 15;
            _lassoLoop = transform.Find("Lasso Loop").gameObject;
            _lassoLoop.SetActive(false);
            _layerMask = LayerMask.GetMask("LassoTarget");
            merbleLineCoroutine = MerbleLine();
            _loopScript = _lassoLoop.AddComponent<Object.LassoLoop>();
            _loopScript.LassoAbility = this;
            _returnPoint = GameObject.Find("ReturnPoint").transform;
        }

        public void PullLever()
        {
            if (!_lever.Activated)
            {
                _lever.ActivateObject();
            }
            else if (_lever.Activated && _lever.canBeDeactivated)
            {
                _lever.DeactivateObject();
            }

            if (!_lever.PlayerInRange)
            {
                _lever.Pullalble = false;
            }

            _lever = null;
        }

        private IEnumerator MerbleLine()
        {
            List<Merble> chargedMerbleList;
            List<Merble> unchargedMerbleList;
            List<Merble> masterList;
            Vector2 distanceBetweenMerblesMinMax = new Vector2(1, 2);
            StopCharging();
            float speed = targetCursor.ObjectSpeed + 0.5f;
            while (true)
            {
                //Debug.Log("Merble Line");
                _lassoLoop.transform.LookAt(_returnPoint);
                _lassoLoop.transform.rotation = Quaternion.Euler(0, _lassoLoop.transform.eulerAngles.y, 0);
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                unchargedMerbleList = merbleBoss.merbleList;
                

                unchargedMerbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));

                float distance = Vector3.Distance(_returnPoint.position, _lassoLoop.transform.position);
                merbleBoss.merbleList = unchargedMerbleList;

                float chargedCount = chargedMerbleList.Count;

                //Debug.Log("DISTANCE/CHARGECOUNT = " + distance/chargedCount);

                if (distance / chargedCount > 1.5f)
                {
                    if (merbleBoss.merbleList.Count > 0)
                    {
                        merbleBoss.merbleList[0].StartCharge(transform.position);
                    }

                    int i = chargedMerbleList.Count;
                    //yield return new WaitUntil(() => merbleBoss.ChargedMerbleList.Count > i);
                    chargedMerbleList = merbleBoss.ChargedMerbleList;
                    unchargedMerbleList = merbleBoss.merbleList;
                    /*if (!chargedMerbleList.Contains(merbleList[0]))
                    {
                    }*/
                }
                else if (distance / chargedCount < 1.5f)
                {
                    if (!unchargedMerbleList.Contains(chargedMerbleList.Last()))
                    {
                        chargedMerbleList.Last().StopCharging();

                        int i = chargedMerbleList.Count;
                        //yield return new WaitUntil(() => merbleBoss.ChargedMerbleList.Count < i);
                        chargedMerbleList = merbleBoss.ChargedMerbleList;
                        unchargedMerbleList = merbleBoss.merbleList;
                    }
                }

                float verticleDistance = _lassoLoop.transform.position.y - _returnPoint.transform.position.y;

                foreach (var merble in chargedMerbleList)
                {
                    merble.transform.parent = _lassoLoop.transform;
                    float index = chargedMerbleList.IndexOf(merble);
                    float count = chargedMerbleList.Count;
                    float divisor = index / count;

                    Vector3 pos = _lassoLoop.transform.position;

                    if (index == 0)
                        pos += _lassoLoop.transform.forward * (1.5f);
                    else
                        pos += _lassoLoop.transform.forward * (1.5f * (index + 1));

                    pos.y = _lassoLoop.transform.position.y -
                            (verticleDistance * (divisor));

                    merble.FloatTowardsObject(pos, index, Merble.AbilityEnum.Lasso, speed);
                }

                yield return null;
            }
        }
    }
}
