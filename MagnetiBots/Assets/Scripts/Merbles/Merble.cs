using UnityEngine;
using UnityEngine.Pool;

namespace Merbles
{
    public class Merble : MonoBehaviour
    {
        private ObjectPool<GameObject> _merblePool;
        public Merbles.Boss myBoss;

        public void SetPool(ObjectPool<GameObject> pool)
        {
            _merblePool = pool;

        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
        public void Charge(Transform target)
        {

        }
    }
}
