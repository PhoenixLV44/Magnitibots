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
        private GameObject _lassoLoopObject;
        public GameObject LassoLoopObject => _lassoLoopObject;
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
        

        private bool _loopBeingThrown;

        public bool LoopBeingThrown
        {
            get => _loopBeingThrown;
            set => _loopBeingThrown = value;
        }

        private bool _attached;

        public bool Attached
        {
            get => _attached;
            set => _attached = value;
        }

        [SerializeField]private bool _pullMerblesBool;

        public bool PullMerblesBool
        {
            get => _pullMerblesBool;
            set => _pullMerblesBool = value;
        }
            
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
                GameObject playerModel = transform.Find("PlayerModel").gameObject;
                Vector3 target = transform.position;
                target += playerModel.transform.forward * (baseRange * merbleBoss.ChargedMerbleList.Count);
                //Debug.Log("Target: " + target);
                target.y = transform.position.y + 0.5f;
                _lassoLoopObject.transform.rotation = playerModel.transform.rotation;
                _lassoLoopObject.transform.parent = null;
                _lassoLoopObject.SetActive(true);
                _loopScript.StartMovement(_returnPoint.position, target);
                StartCoroutine(merbleLineCoroutine);
                controller.Animator.Play("Throw");
            }

            //StopCoroutine(Charge());
            merbleBoss.FireMerbles();
            currentPowerLevel = 0;
        }

        public void MoveLassoTarget()
        {
            targetCursor.MoveObjectToCursor(_lassoLoopObject, this);
        }

        public IEnumerator UnhookLasso()
        {
            targetCursor.DeactivateCursor();
            Cursor.lockState = CursorLockMode.None;

            _loopScript.BoxCollider.enabled = false;
            StartCoroutine(_loopScript.ReturnToStartPosition(_returnPoint.transform.position, 10));
            _pullMerblesBool = true;
            if (_lassoLoopObject.transform.childCount > 0)
            {
                foreach (Transform child in _lassoLoopObject.transform)
                {
                    if (child.CompareTag("LassoTarget") || child.CompareTag("Lever"))
                    {
                        GameObject loopedObject = child.gameObject;
                        _lassoLoopObject.transform.parent = transform;

                        if (child.CompareTag("LassoTarget"))
                        {
                            Rigidbody rb = loopedObject.GetComponent<Rigidbody>();
                            rb.useGravity = true;
                            rb.constraints = RigidbodyConstraints.None;
                        }
                        else if (child.CompareTag("Lever"))
                        {
                            PullLever();
                        }
                        loopedObject.transform.parent = null;
                        break;
                    }
                }
            }

            rangeIndicator.DisableRangeIndicator();

            yield return  new WaitUntil(() => !_pullMerblesBool);
            Debug.Log("Please Work");
            _lassoLoopObject.transform.parent = transform;
            _lassoLoopObject.SetActive(false);
            controller.LassoHooked = false;
            StopCoroutine(merbleLineCoroutine);
            merbleBoss.FireMerbles();
        }

        protected override void InitializeAbility()
        {
            base.InitializeAbility();
            baseRange = 1.5f;
            basePowerLevel = 1;
            maxPowerLevel = 15;
            _lassoLoopObject = transform.Find("Lasso Loop").gameObject;
            _lassoLoopObject.SetActive(false);
            _layerMask = LayerMask.GetMask("LassoTarget");
            merbleLineCoroutine = MerbleLine();
            _loopScript = _lassoLoopObject.AddComponent<Object.LassoLoop>();
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
            //StartCoroutine(UnhookLasso());
        }

        private IEnumerator MerbleLine()
        {
            List<Merbles.Merble> chargedMerbleList;
            List<Merbles.Merble> merbleList;
            Vector2 distanceBetweenMerblesMinMax = new Vector2(1, 2);
            StopCharging();
            float speed = targetCursor.ObjectSpeed + 0.5f;
            while (true)
            {
                //Debug.Log("Merble Line");
                _lassoLoopObject.transform.LookAt(_returnPoint);
                _lassoLoopObject.transform.rotation = Quaternion.Euler(0, _lassoLoopObject.transform.eulerAngles.y, 0);
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                merbleList = merbleBoss.merbleList;


                merbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));

                float distance = Vector3.Distance(_returnPoint.position, _lassoLoopObject.transform.position);
                merbleBoss.merbleList = merbleList;

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
                    merbleList = merbleBoss.merbleList;
                    /*if (!chargedMerbleList.Contains(merbleList[0]))
                    {
                    }*/
                }
                else if (distance / chargedCount < 1.5f)
                {
                    if (!merbleList.Contains(chargedMerbleList.Last()))
                    {
                        chargedMerbleList.Last().StopCharging();

                        int i = chargedMerbleList.Count;
                        //yield return new WaitUntil(() => merbleBoss.ChargedMerbleList.Count < i);
                        chargedMerbleList = merbleBoss.ChargedMerbleList;
                        merbleList = merbleBoss.merbleList;
                    }
                }

                float verticleDistance = _lassoLoopObject.transform.position.y - _returnPoint.transform.position.y;

                foreach (var merble in chargedMerbleList)
                {
                    merble.transform.parent = _lassoLoopObject.transform;
                    float index = chargedMerbleList.IndexOf(merble);
                    float count = chargedMerbleList.Count;
                    float divisor = index / count;

                    Vector3 pos = _lassoLoopObject.transform.position;

                    if (index == 0)
                        pos += _lassoLoopObject.transform.forward * (1.5f);
                    else
                        pos += _lassoLoopObject.transform.forward * (1.5f * (index + 1));

                    pos.y = _lassoLoopObject.transform.position.y -
                            (verticleDistance * (divisor));

                    merble.FloatTowardsObject(pos, index, Merble.AbilityEnum.Lasso, speed);
                }

                yield return null;
            }
        }
    }
}
