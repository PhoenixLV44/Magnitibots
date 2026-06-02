using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Merbles
{
    public class Boss : MonoBehaviour
    {
        Merbles.Collector collector;
        
        public int currentMerbles = 0;
        public List<Merble> merbleList;

        public GameObject merblePrefab;

        public ObjectPool<GameObject> Merbles { get { return _merbles; } private set { _merbles = value; } }
        private ObjectPool<GameObject> _merbles;
        public int defaultCapacity;
        public int maxSize;

        private void Start()
        {
            merbleList = new List<Merble>();
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
            GameObject merble = Instantiate(merblePrefab);
            merbleList.Add(merble.GetComponent<Merble>());
            currentMerbles++;
            return merble;
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
            merbleList.Remove(merble.GetComponent<Merble>());
            Destroy(merble);
        }
        public void ChargeMerble()
        {
            for (int i = 0; i < merbleList.Count; i++)
            {
                if (merbleList[i] != null) continue;
                if(!merbleList[i].Charging)
                {
                    Debug.Log(i + "Ding");
                    merbleList[i].StartCharge(transform.position + (transform.forward*2));
                    break;
                }
                
            }
            
        }
        public void FireMerbles()
        {
            for (int i = 0; i < _merbles.CountInactive; i++)
            {
                _merbles.Get();
            }
        }
    }
}
