using System.Collections;
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

        enum FollowTypes
        {
            Loose,
            Snake,
            Coalition
        }
        private FollowTypes _followType;
        
        public bool Sentience { get { return _isAlive; }  set { _isAlive = value; } }
        private bool _isAlive = false;
        public bool Charging { get { return _isCharging; } private set {  _isCharging = value; } }
        private bool _isCharging = false;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            
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
        public void SetFollowType(string type)
        {
            switch (type)
            {
                case "Loose":
                    _followType = FollowTypes.Loose;
                    break;
                case "Coalition":
                    _followType = FollowTypes.Coalition;
                    break;
                case "Snake":
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
            if (_isAlive && !_isCharging)
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
        public void StartCharge(Vector3 target)
        {
            Debug.Log("Waagh");
            
            StartCoroutine(Charge(target));
        }
        IEnumerator Charge(Vector3 target)
        {
            Debug.Log("Arrgh!");
            _isCharging = true;
            _agent.isStopped = false;
            _agent.destination = target;
            yield return new WaitUntil(() => _agent.hasPath);
            yield return new WaitUntil(() => _agent.remainingDistance < 0.25f);
            myBoss.merbleList.Remove(this);
            _merblePool.Release(gameObject);
            myBoss.chargingMerbles--;
            myBoss.chargedMerbles++;
        }
        public void StopCharging()
        {
            _isCharging = false;
            _agent.ResetPath();
            StopAllCoroutines();
        }

        public void SnakeMovement()
        {
            int index = myBoss.merbleList.IndexOf(this);
            if (index == 0 || myBoss.merbleList[index-1].Charging==true)
            {
                if (Vector3.Distance(transform.position, myBoss.transform.position) > 1+_agent.speed/2)
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
                if (Vector3.Distance(transform.position, myBoss.merbleList[index - 1].transform.position) > 1+_agent.speed/5)
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
}
