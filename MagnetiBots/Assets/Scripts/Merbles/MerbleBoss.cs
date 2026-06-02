using UnityEngine;
using UnityEngine.Pool;

namespace Merbles
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] private GameObject merblePrefab;
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxSize;
        public ObjectPool<GameObject> _merbles;
        private void Awake()
        {
            merblePrefab.GetComponent<Merble>().myBoss = this;
            _merbles = new ObjectPool<GameObject>(
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

        }
        private void OnReleaseMerble(GameObject merble)
        {

        }
        private void OnDestroyMerble(GameObject merble)
        {
            Destroy(merble);
        }
    }
}
