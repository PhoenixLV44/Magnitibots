using Ability.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Merbles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability
{
    public class Lasso : Parent
    {
        private GameObject _lassoLoop;
        public GameObject LassoLoop => _lassoLoop;
        private Object.LassoLoop _loopScript;
        public LassoLoop LoopScript => _loopScript;

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
        [SerializeField] private bool lassoLaunched;

        private SuperJumpPoint _chargePoint;
        private Transform[] _merblePoints;
        private bool _returnToPlayer = false;

        private void Start()
        {
            InitializeAbility();
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
            //merbleLineCoroutine = MerbleLine();
            _loopScript = _lassoLoop.AddComponent<Object.LassoLoop>();
            _loopScript.LassoAbility = this;
            _returnPoint = GameObject.Find("ReturnPoint").transform;
            _chargePoint = transform.GetComponentInChildren<SuperJumpPoint>();
            _merblePoints = _chargePoint.MerblePoints;
            chargeInput = InputSystem.actions.FindAction("Charge");
        }

        public override void StartCharging()
        {
            //base.StartCharging();
            if (merbleBoss.MasterList.Count >= 1)
            {
                StartCoroutine(Charge());
            }
            controller.ChargingParticles.SetActive(true);
            StartCoroutine(MerbleLine());
        }

        public override void StopCharging()
        {
            if (chargeCoroutine != null)
            {
                if (merbleBoss.ChargedMerbleList.Count < 1)
                {
                    isCharging = false;
                    targetCursor.CanMoveCursor = true;
                    rangeIndicator.DisableRangeIndicator();
                    StopAllCoroutines();
                    LoopScript.StopAllCoroutines();
                    MerbleBoss.FireMerbles();
                }
                //Debug.Log("Stopping charging");
                //aimingGuide.SetActive(false);
                //currentPowerLevel = basePowerLevel;
                //rangeIndicator.DisableRangeIndicator();
                controller.ChargingParticles.SetActive(false);
                StopCoroutine(chargeCoroutine);
            }
        }

        public override IEnumerator Charge()
        {
            currentPowerLevel = 0;
            float chargeTimer = 0.5f;
            rangeIndicator.DisableRangeIndicator();
            //lassoLaunched = false;
            isCharging = true;
            controller.ChargingParticles.SetActive(true);
            int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
            merbleBoss.merbleList.Sort((a, b) =>
                Vector3.Distance(a.transform.position, transform.position)
                    .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
            yield return new WaitForSecondsRealtime(chargeTimer);
            //Debug.Log("MAX POWER: " + maxPower);
            if (!chargeInput.IsPressed())
            {
                isCharging = false;
                targetCursor.CanMoveCursor = true;
                StopAllCoroutines();
                _loopScript.StopAllCoroutines();
                merbleBoss.FireMerbles();
                yield break;
            }
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
            while (isCharging)
            {
                Debug.Log("Charging FROM CHARGE");
                currentPowerLevel = merbleBoss.ChargedMerbleList.Count;
                //Debug.Log("Current PowerLevel: " + currentPowerLevel);
                rangeIndicator.ChangeRangeSize((baseRange * currentPowerLevel * 2));

                merbleBoss.merbleList.Sort((a, b) =>
                    Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                Merble[] merbleArray = merbleBoss.merbleList.ToArray();

                if (currentPowerLevel < maxPower)
                {
                    merbleArray[j].StartCharge(transform.position);
                }

                yield return new WaitForSecondsRealtime(chargeTimer);
            }
        }

        public override void Fire()
        {
            isCharging = false;
            Debug.Log("Throw");
            if (merbleBoss.ChargedMerbleList.Count >= 1)
            {
                controller.Animator.SetTrigger("Throw");
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
                _loopScript.enabled = true;
                _lassoLoop.SetActive(true);
                _loopScript.StartMovement(_returnPoint.position, target);
                //StartCoroutine(merbleLineCoroutine);
            }
            else
            {
                merbleBoss.FireMerbles();
                StopAllCoroutines();
                _loopScript.StopAllCoroutines();
            }
            //StopCharging();
            StopCoroutine(Charge());
            lassoLaunched = true;
            //StopCoroutine(Charge());
            //merbleBoss.FireMerbles();
            //currentPowerLevel = 0;
        }

        public void MoveLassoTarget()
        {
            targetCursor.ObjectToMove = _lassoLoop;
        }

        public IEnumerator UnhookLasso()
        {
            targetCursor.ObjectToMove = null;
            targetCursor.CanMoveCursor = false;
            targetCursor.SetRayCastPosition(_returnPoint.position);
            targetCursor.DeactivateCursor();
            
            _loopScript.BoxCollider.enabled = false;
            PuzzleCube puzzleCube = null;
            if (_loopedObject)
            {
                if (_loopedObject.CompareTag("LassoTarget"))
                {
                    puzzleCube = _loopedObject.GetComponent<PuzzleCube>();

                }
                else if (_loopedObject.CompareTag("Lever"))
                {
                    PullLever();
                }

                _loopedObject.transform.parent = null;
                _loopedObject = null;
            }
            controller.Animator.Play("Pull");
            yield return new WaitForSeconds(controller.AnimController.PullAnimLength / 2);
            
            if (puzzleCube)
            {
                //Debug.Log("Dropping Puzzle Cube");
                puzzleCube.ChangeGravity(true);
            }
 
            lassoLaunched = false;
            yield return new WaitUntil(() => Vector3.Distance(_lassoLoop.transform.position, _returnPoint.position) < 1);
            Debug.Log("WAHOO");
               
            _lassoLoop.transform.position = _returnPoint.position;


            if (puzzleCube)
            {
                puzzleCube.UnfreezeConstraints();
            }
            rangeIndicator.DisableRangeIndicator();
            
            _lassoLoop.transform.parent = transform;
            _lassoLoop.SetActive(false);
            
            controller.LassoHooked = false;
            
            merbleBoss.FireMerbles();
            StopCoroutine(Charge());
            controller.Movement.CanLook = true;
            targetCursor.ActivateCursor();
            lassoLaunched = false;
            StopAllCoroutines();
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
            Debug.Log("Merble Line");
            List<Merble> chargedMerbleList;
            List<Merble> unchargedMerbleList;
            List<Merble> masterList;
            Vector2 distanceBetweenMerblesMinMax = new Vector2(1, 2);
            StopCharging();
            float speed = targetCursor.ObjectSpeed + 0.5f;
            lassoLaunched = false;
            while (!lassoLaunched)
            {
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                unchargedMerbleList = merbleBoss.merbleList;
                unchargedMerbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                if (chargedMerbleList.Count > 0)
                {
                    for (int i = 0; i < chargedMerbleList.Count; i++)
                    {
                        chargedMerbleList[i].FloatTowardsObject(_merblePoints[i].transform.position, i, Merble.AbilityEnum.Lasso);
                    }
                }
                yield return null;
            }

            float distance;
            int merblesNeeded;
            float distanceBetweenMerbles;
            float verticalDistance;
            while (lassoLaunched)
            {
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                unchargedMerbleList = merbleBoss.merbleList;
                unchargedMerbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                _lassoLoop.transform.LookAt(_returnPoint);
                _lassoLoop.transform.rotation = Quaternion.Euler(0, _lassoLoop.transform.eulerAngles.y, 0);
                
                int maxPower = maxPowerLevel >= merbleBoss.merbleList.Count ? merbleBoss.merbleList.Count : maxPowerLevel;
                if (chargedMerbleList.Count < maxPower)
                {
                    Debug.Log("CHARGING FROM MERBLE LINE");
                    unchargedMerbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                    if (unchargedMerbleList.Count > 0)
                    {
                        for (int i = 0; i < maxPower - merbleBoss.ChargedMerbleList.Count; i++)
                        {
                            merbleBoss.merbleList[i].StartCharge(transform.position);
                        }
                    }
                }
                distance = Vector3.Distance(_returnPoint.position, _lassoLoop.transform.position);
                merbleBoss.merbleList = unchargedMerbleList;
                
                merblesNeeded = Mathf.CeilToInt(distance / baseRange);
                
                distanceBetweenMerbles = distance/(float)merblesNeeded;

                verticalDistance = _lassoLoop.transform.position.y - _returnPoint.transform.position.y;

                for (int i = 0; i < chargedMerbleList.Count; i++)
                {
                    Merble merble = chargedMerbleList[i];
                    float count = chargedMerbleList.Count;
                    float divisor = i / count;
                    Vector3 pos = _lassoLoop.transform.position;
                    if (i < merblesNeeded)
                    {
                        if (i == 0)
                            pos += _lassoLoop.transform.forward * (distanceBetweenMerbles);
                        else
                            pos += _lassoLoop.transform.forward * (distanceBetweenMerbles * (i + 1));

                        pos.y = _lassoLoop.transform.position.y -
                                (verticalDistance * (divisor));
                    }
                    else
                    {
                        int j = i - merblesNeeded;
                        pos = _merblePoints[j].transform.position;
                    }
                    merble.FloatTowardsObject(pos, i, Merble.AbilityEnum.Lasso, speed);
                }
                
                yield return null;
            }
            _returnToPlayer = true;
            while (_returnToPlayer)
            {
                Debug.Log("returning to player");
                chargedMerbleList = merbleBoss.ChargedMerbleList;
                unchargedMerbleList = merbleBoss.merbleList;
                _lassoLoop.transform.position = Vector3.Slerp(_lassoLoop.transform.position, _returnPoint.position, speed * Time.deltaTime);
                
                distance = Vector3.Distance(_returnPoint.position, _lassoLoop.transform.position);
                merblesNeeded = Mathf.CeilToInt(distance / baseRange);
                distanceBetweenMerbles = distance/(float)merblesNeeded;
                verticalDistance = _lassoLoop.transform.position.y - _returnPoint.transform.position.y;
                for (int i = 0; i < chargedMerbleList.Count; i++)
                {
                    Merble merble = chargedMerbleList[i];
                    float count = chargedMerbleList.Count;
                    float divisor = i / count;
                    Vector3 pos = _lassoLoop.transform.position;
                    if (i < merblesNeeded)
                    {
                        /*if (i == 0)
                            pos += _lassoLoop.transform.forward * (distanceBetweenMerbles);
                        else
                            pos += _lassoLoop.transform.forward * (distanceBetweenMerbles * (i + 1));

                        pos.y = _lassoLoop.transform.position.y -
                                (verticalDistance * (divisor));*/
                        merble.FloatTowardsObject(_returnPoint.position, i, Merble.AbilityEnum.Lasso, speed);
                    }
                    else
                    {
                        merble.StopCharging();
                    }
                }
                if (merbleBoss.ChargedMerbleList.Count < 0)
                {
                    merbleBoss.FireMerbles();
                    _returnToPlayer = false;
                }
                yield return null;
            }
        }
        
        public override void Respawn()
        {
            base.Respawn();
            StartCoroutine(UnhookLasso());
            targetCursor.ObjectToMove = null;
            rangeIndicator.DisableRangeIndicator();
            lassoLaunched = false;
            _loopScript.StopAllCoroutines();
        }
    }
}
