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

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
        }
        public void SetPool(ObjectPool<GameObject> pool)
        {
            _merblePool = pool;
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
            if (_isAlive)
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
        public void Charge(Transform target)
        {
            _agent.destination = target.position;
        }
    }
}
