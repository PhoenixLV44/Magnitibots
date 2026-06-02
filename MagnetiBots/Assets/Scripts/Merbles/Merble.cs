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
                if (Vector3.Distance(transform.position, myBoss.transform.position) > 5f)
                {
                    _agent.isStopped = false;
                    _agent.destination = myBoss.transform.position;
                }
                else
                {
                    _agent.isStopped = true;
                }
            }
        }
        public void StartCharge(Vector3 target)
        {
            StartCoroutine(Charge(target));
        }
        IEnumerator Charge(Vector3 target)
        {
            Debug.Log("Arrgh!");
            _isCharging = true;
            _agent.destination = target;
            yield return new WaitUntil(() => Vector3.Distance(transform.position, target) < 0.5f);
            _merblePool.Release(gameObject);
        }
    }
}
