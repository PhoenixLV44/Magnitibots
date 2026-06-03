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
                    case FollowTypes.Coalition:
                        break;
                    case FollowTypes.Snake:
                        int index = myBoss.merbleList.IndexOf(this);
                        if (index == 0)
                        {
                            if (Vector3.Distance(transform.position, myBoss.transform.position) > 2f)
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
                            if (Vector3.Distance(transform.position, myBoss.merbleList[index-1].transform.position) > 1f)
                            {
                                _agent.isStopped = false;
                                _agent.destination = myBoss.merbleList[index - 1].transform.position;
                            }
                            else
                            {
                                _agent.isStopped = true;
                            }
                        }
                        break;
                    default:
                    case FollowTypes.Loose:
                        if (Vector3.Distance(transform.position, myBoss.transform.position) > 5f)
                        {
                            _agent.isStopped = false;
                            _agent.destination = myBoss.transform.position;
                        }
                        else
                        {
                            _agent.isStopped = true;
                        }
                        break;
                }
                
                
            }
        }
        public void StartCharge(Vector3 target)
        {
            Debug.Log("Waagh");
            myBoss.merbleList.Remove(this);
            StartCoroutine(Charge(target));
        }
        IEnumerator Charge(Vector3 target)
        {
            Debug.Log("Arrgh!");
            _isCharging = true;
            _agent.isStopped = false;
            _agent.destination = target;
            yield return new WaitUntil(() => Vector3.Distance(transform.position, target) < 1f);
            _merblePool.Release(gameObject);
        }
    }
}
