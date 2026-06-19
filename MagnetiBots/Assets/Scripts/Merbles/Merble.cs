using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace Merbles
{
    public class Merble : MonoBehaviour
    {
        public Merbles.Boss myBoss;
        private ObjectPool<GameObject> _merblePool;
        private NavMeshAgent _agent;
        public NavMeshAgent Agent => _agent;

        public enum FollowTypes
        {
            Loose,
            Snake,
            Coalition
        }
        private FollowTypes _followType;

        public bool Sentience { get { return _isAlive; } set { _isAlive = value; } }
        private bool _isAlive = false;
        public bool Charging { get { return _isCharging; } set { _isCharging = value; } }
        private bool _isCharging = false;

        [SerializeField] private float floatingSpeed;
        private bool floating;

        public bool Floating => floating;

        private float defaultOffset;
        [SerializeField]LayerMask groundLayer;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            floatingSpeed = _agent.speed;
            defaultOffset = _agent.baseOffset;
        }
        public void SetPool(ObjectPool<GameObject> pool)
        {
            _merblePool = pool;
            Charging = false;
            myBoss.merbleList.Add(this);
            Sentience = true;
            tag = "Merble";
            _agent.enabled = true;
        }
        public void SetFollowType(FollowTypes type)
        {
            switch (type)
            {
                case FollowTypes.Loose:
                    _followType = FollowTypes.Loose;
                    break;
                case FollowTypes.Coalition:
                    _followType = FollowTypes.Coalition;
                    break;
                case FollowTypes.Snake:
                    _followType = FollowTypes.Snake;
                    break;
                default:
                    _followType = FollowTypes.Loose;
                    break;
            }
        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
        private void Update()
        {
            //if (GroundCheck() && !floating)
            {
                if (_isAlive && !_isCharging && myBoss.merbleList.Count > 0 && !myBoss.ChargedMerbleList.Contains(this))
                {
                    switch (_followType)
                    {
                        //Fix slowdown
                        /*case FollowTypes.Coalition:
                            break;
                        */
                        case FollowTypes.Snake:
                            SnakeMovement();
                            break;
                        default:
                        case FollowTypes.Loose:

                            break;
                    }
                }
            }
            //else if(!GroundCheck() && !floating)
            {
                //transform.position = Vector3.MoveTowards(transform.position, _agent.destination, Time.deltaTime * _agent.speed);
            }
        }
        public void StartCharge(Vector3 target)
        {
            //Debug.Log("Waagh");

            StartCoroutine(Charge(target));
        }
        IEnumerator Charge(Vector3 target)
        {
            //Debug.Log("Arrgh!");
            _isCharging = true;
            _agent.isStopped = false;
            _agent.destination = target;
            yield return new WaitUntil(() => _agent.hasPath);
            yield return new WaitUntil(() => _agent.remainingDistance <= 0.5f);
            myBoss.merbleList.Remove(this);
            myBoss.ChargedMerbleList.Add(this);
            //_merblePool.Release(gameObject);
            
            _agent.enabled = false;
            myBoss.chargingMerbles--;
            myBoss.chargedMerbles++;
        }
        public void StopCharging()
        {
            _isCharging = false;
            floating = false;
            transform.position = myBoss.transform.position;
            _agent.enabled = true;
            _agent.destination = myBoss.transform.position;
            _agent.ResetPath();

            //Debug.Log("wow!");

            /*_agent.enabled = true;
            _agent.destination = myBoss.transform.position;
            _agent.ResetPath();*/
            myBoss.ChargedMerbleList.Remove(this);
            if (!myBoss.merbleList.Contains(this))
            {
                myBoss.merbleList.Add(this);
            }
            StopAllCoroutines();
        }

        public void SnakeMovement()
        {
            int index = myBoss.merbleList.IndexOf(this);
            if (index == 0 || myBoss.merbleList[index - 1].Charging == true)
            {
                if (Vector3.Distance(transform.position, myBoss.transform.position) > 1 + _agent.speed / 2)
                {
                    _agent.isStopped = false;
                    _agent.destination = myBoss.transform.position;
                }
                else
                {
                    _agent.isStopped = true;
                }
            }
            else if (index != -1)
            {
                if (Vector3.Distance(transform.position, myBoss.merbleList[index - 1].transform.position) > 1 + _agent.speed / 5)
                {
                    _agent.isStopped = false;
                    _agent.destination = myBoss.merbleList[index - 1].transform.position;
                }
                else
                {
                    //_agent.velocity = 0;
                    _agent.isStopped = true;
                }
            }
        }
        public void LooseMovement()
        {
            NavMeshHit hit;

            if (!_agent.Raycast(myBoss.transform.position, out hit))
            {
                if (Vector3.Distance(transform.position, myBoss.transform.position) > 2f)
                {
                    _agent.isStopped = false;
                    _agent.destination = myBoss.transform.position;
                }
                else
                {
                    _agent.isStopped = true;
                    _agent.velocity = Vector3.zero;
                }
            }
            else
            {

            }
        }

        public void FloatTowardsObject(Transform target, float index, float speed = 2.5f)
        {
            if (!floating)
            {
                _agent.enabled = false;
                //transform.parent = target.transform;
                floating = true;
            }
            Vector2 rngMinMax = new Vector2(-1.5f, 1.5f);
            if (index > 1)
            {
                rngMinMax.x -= (index / 10);
                rngMinMax.y += (index / 10);
            }
            
            Vector3 targetPos = new Vector3(target.transform.position.x + (Random.Range(rngMinMax.x, rngMinMax.y)), target.transform.position.y + (Random.Range(rngMinMax.x, rngMinMax.y)), target.transform.position.z + (Random.Range(rngMinMax.x, rngMinMax.y)));
            //Debug.Log("FLOATING");
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }

        public void CreateLine(GameObject target, float index, float distance)
        {
            
        }

        public IEnumerator UseGravity()
        {
            float t = 0;
            while (!_agent.enabled)
            {
                Vector3 dir = new Vector3(transform.position.x, transform.position.y - (9.8f * t),
                    transform.position.z);
                transform.position = dir;
                t += Time.deltaTime;
                yield return null;
            }
        }

        public IEnumerator ReturnToPlayer()
        {
            float distance = Vector3.Distance(transform.position, myBoss.transform.position);
            while ((distance) > 1)
            {
                //_agent.baseOffset = Mathf.Lerp(_agent.baseOffset, defaultOffset, _agent.speed * Time.deltaTime);
                transform.position = Vector3.Slerp(transform.position, myBoss.transform.position, _agent.speed * Time.deltaTime);
                distance = Vector3.Distance(transform.position, myBoss.transform.position);
                Debug.Log("Returning to player" + distance);
                
                yield return null;
            }

            floating = false;
            _agent.enabled = true;
            _agent.destination = myBoss.transform.position;
            _agent.ResetPath();
        }

        private bool GroundCheck()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, _agent.baseOffset + 1, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
