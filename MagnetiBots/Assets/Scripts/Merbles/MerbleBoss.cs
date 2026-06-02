using UnityEngine;
using UnityEngine.Pool;

namespace Merbles
{
    public class Boss : MonoBehaviour
    {
        Merbles.Collector collector;
        
        public int currentMerbles = 0;

        public GameObject merblePrefab;

        public ObjectPool<GameObject> Merbles { get { return _merbles; } private set { _merbles = value; } }
        private ObjectPool<GameObject> _merbles;
        public int defaultCapacity;
        public int maxSize;

        private void Start()
        {
            
            merblePrefab.GetComponent<Merble>().myBoss = this;

            collector = gameObject.AddComponent<Collector>();
            collector.boss = this;

            Merbles = new ObjectPool<GameObject>(
                createFunc: OnCreateMerble,
                actionOnGet: OnGetMerble,
                actionOnRelease: OnReleaseMerble,
                actionOnDestroy: OnDestroyMerble,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
                );
        }
        private GameObject OnCreateMerble()
        {
            return Instantiate(merblePrefab);
        }
        private void OnGetMerble(GameObject merble)
        {
            merble.SetActive(true);
        }
        private void OnReleaseMerble(GameObject merble)
        {
            merble.SetActive(false);
        }
        private void OnDestroyMerble(GameObject merble)
        {
            Destroy(merble);
        }
        public void RepopulateMerbles()
        {
            for (int i = 0; i < currentMerbles; i++)
            {
                _merbles.Get();
            }
        }
    }
}
