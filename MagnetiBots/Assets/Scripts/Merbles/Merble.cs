using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Merbles
{
    public class Merble : MonoBehaviour
    {
        public Merbles.Boss myBoss;
        private ObjectPool<GameObject> _merblePool;
        private NavMeshAgent _agent;
        [SerializeField] LayerMask merbleMask;
        public NavMeshAgent Agent => _agent;
        
        Rigidbody _rb;

        public enum FollowTypes { Loose, Snake, Coalition }
        private FollowTypes _followType;
        
        public enum AbilityEnum{ None, Lasso, Smash, SuperJump}

        private AbilityEnum _currentAbilityEnum = AbilityEnum.None;
        public AbilityEnum CurrentAbilityEnum { get => _currentAbilityEnum; set => _currentAbilityEnum = value; }

        public bool Sentience { get { return _isAlive; } set { _isAlive = value; } }
        private bool _isAlive = false;
        public bool Charging { get { return _isCharging; } set { _isCharging = value; } }
        private bool _isCharging = false;

        [SerializeField] private float _defaultAcceleration;
        private bool _floating;

        public bool Floating => _floating;
        
        [SerializeField]LayerMask groundLayer;
        [SerializeField] private GameObject chargedParticles;
        [SerializeField] private GameObject collectParticles;
        public GameObject CollectParticles => collectParticles;

        [SerializeField] private Transform parent;

        Coroutine charge;
        Coroutine beep;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            _defaultAcceleration = _agent.acceleration;
            _rb = GetComponent<Rigidbody>();
            if (transform.parent != null)
            {
                parent = transform.parent;
            }

            if (chargedParticles)
            {
                chargedParticles.SetActive(false);
            }

            if (collectParticles)
            {
                collectParticles.SetActive(false);
            }
        }
        public void SetPool(ObjectPool<GameObject> pool)
        {
            _merblePool = pool;
            Charging = false;
            _agent.speed = myBoss.GetComponent<Player.Controller>().Movement.DefaultMoveSpeed;
            myBoss.merbleList.Add(this);
            Sentience = true;
            tag = "Merble";
            _agent.enabled = true;
            collectParticles.SetActive(true);
            beep = StartCoroutine(BeepBoop());
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
                            LooseMovement();
                            break;
                    }
                }

                _agent.speed = GetComponent<Renderer>().isVisible ? myBoss.GetComponent<Player.Controller>().Movement.DefaultMoveSpeed : myBoss.GetComponent<Player.Controller>().Movement.DefaultMoveSpeed * 2;
                if ((!GetComponent<Renderer>().isVisible &&
                     Vector3.Distance(transform.position, myBoss.transform.position) > 10) || Vector3.Distance(transform.position, myBoss.transform.position) > 20)
                {
                    _agent.speed = myBoss.GetComponent<Player.Controller>().Movement.DefaultMoveSpeed * 2;
                    _agent.acceleration = _defaultAcceleration * 2;
                    _agent.stoppingDistance = 5;
                }
                else
                {
                    _agent.speed = myBoss.GetComponent<Player.Controller>().Movement.DefaultMoveSpeed;
                    _agent.acceleration = _defaultAcceleration;
                    _agent.stoppingDistance = 1;
                }
                if (_agent.enabled)
                {
                    //Debug.Log("Agent" + transform.name+ " Speed: " + _agent.speed);
                }
            }
        }
        public void StartCharge(Vector3 target)
        {
            if (!myBoss.ChargedMerbleList.Contains(this))
            {
                charge = StartCoroutine(Charge(target));
            }
        }
        IEnumerator Charge(Vector3 target)
        {
            //Debug.Log(transform.name + "Charging");
            _isCharging = true;
            _agent.isStopped = false;
            _agent.destination = target;
            yield return new WaitUntil(() => _agent.hasPath);
            yield return new WaitUntil(() => Vector3.Distance(transform.position, myBoss.transform.position) <= 1);
            myBoss.merbleList.Remove(this);
            myBoss.ChargedMerbleList.Add(this);
            //_merblePool.Release(gameObject);

            if (chargedParticles)
            {
                chargedParticles.SetActive(true);
            }
            
            _agent.enabled = false;
            myBoss.CheckForDuplicates(myBoss.ChargedMerbleList);
        }
        public void StopCharging()
        {
            transform.parent = parent;
            _isCharging = false;
            _floating = false;
            if (_currentAbilityEnum == AbilityEnum.Smash)
            {
                transform.position = myBoss.transform.position;
            }
            _agent.enabled = true;
            _agent.destination = myBoss.transform.position;
            _agent.ResetPath();

            if (chargedParticles)
            {
                chargedParticles.SetActive(false);
            }

            //Debug.Log("wow!");

            /*_agent.enabled = true;
            _agent.destination = myBoss.transform.position;
            _agent.ResetPath();*/
            myBoss.ChargedMerbleList.Remove(this);
            if (!myBoss.merbleList.Contains(this))
            {
                myBoss.merbleList.Add(this);
            }
            myBoss.CheckForDuplicates(myBoss.merbleList);
            _currentAbilityEnum = AbilityEnum.None;
            StopCoroutine(charge);
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
            RaycastHit hit;

            if (!Physics.Raycast(transform.position, myBoss.transform.position, out hit, 40, merbleMask))
            {
                if (Vector3.Distance(transform.position, myBoss.transform.position) > 2f)
                {
                    _agent.destination = myBoss.transform.position;
                }
                else
                {
                    _agent.velocity = _agent.velocity/4;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, hit.transform.position) > 1f)
                {
                    _agent.destination = hit.transform.position;
                }
                else
                {
                    _agent.velocity = _agent.velocity / 4;
                }
            }
        }

        public void FloatTowardsObject(Vector3 vectorPos, float index, AbilityEnum currentAbility, float speed = 2.5f)
        {
            Vector2 rngMinMax;
            Vector3 targetPos;
            switch (currentAbility)
            {
                case AbilityEnum.Lasso:
                    rngMinMax = new Vector2(-0.5f, 0.5f);
                    targetPos = new Vector3(vectorPos.x + (Random.Range(rngMinMax.x, rngMinMax.y)), vectorPos.y + (Random.Range(rngMinMax.x, rngMinMax.y)), vectorPos.z + (Random.Range(rngMinMax.x, rngMinMax.y)));
                    transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * speed);
                    break;
                
                case AbilityEnum.Smash:
                    rngMinMax = new Vector2(-1.5f, 1.5f);
                    if (index > 1)
                    {
                        rngMinMax.x -= (index / 10);
                        rngMinMax.y += (index / 10);
                    }
            
                    targetPos = new Vector3(vectorPos.x + (Random.Range(rngMinMax.x, rngMinMax.y)), vectorPos.y + (Random.Range(rngMinMax.x, rngMinMax.y)), vectorPos.z + (Random.Range(rngMinMax.x, rngMinMax.y)));
                    //Debug.Log("FLOATING");
                    transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * speed);
                    break;
                case AbilityEnum.SuperJump:
                    transform.position = Vector3.Slerp(transform.position, vectorPos, Time.deltaTime * speed);
                    break;
                default:
                    Debug.LogError("Unknown AbilityEnum");
                    break;
                    
            }
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
            while (true)
            {
                _agent.enabled = true;
                if (!_agent.isOnNavMesh)
                {
                    _agent.enabled = false;
                }
                else
                {
                    _agent.destination = myBoss.transform.position;
                    _agent.ResetPath();
                    yield break;
                }
                
                float distance = Vector3.Distance(transform.position, myBoss.transform.position);
                //_agent.baseOffset = Mathf.Lerp(_agent.baseOffset, defaultOffset, _agent.speed * Time.deltaTime);
                transform.position = Vector3.Slerp(transform.position, myBoss.transform.position, _agent.speed * Time.deltaTime);
                distance = Vector3.Distance(transform.position, myBoss.transform.position);
                Debug.Log("Returning to player" + distance);
                
                yield return null;
            }

/*            floating = false;*/
        }

        public bool GroundCheck()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, _agent.baseOffset + 1, groundLayer))
            {
                //Debug.Log(hit.transform.name);
                StopCharging();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RespawnPlane"))
            {
                
            }
        }
        private IEnumerator BeepBoop()
        {
            if(Random.Range(1,20) == 1)
            {
                Globals.Managers.Audio.PlaySFXRandom("RobotAmbiance", transform, 11,0.5f);
            }
            yield return new WaitForSeconds(1);
            beep = StartCoroutine(BeepBoop());
        }
    }
}
