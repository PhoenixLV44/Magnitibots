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
        public string MerbleFollowType {get {return _merbleFollowType;} set { _merbleFollowType = value; } }
        private string _merbleFollowType;

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
            merble.GetComponent<Merble>().SetPool(Merbles);
            merble.GetComponent<Merble>().SetFollowType(MerbleFollowType);
            currentMerbles++;
            return merble;
        }
        private void OnGetMerble(GameObject merble)
        {
            merble.GetComponent<Merble>().SetPool(Merbles);
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
                if (merbleList[i] != null)
                {
                    if (!merbleList[i].Charging)
                    {
                        
                        merbleList[i].StartCharge(Vector3.zero);
                        break;
                    }
                }
                
            }
            
        }
        public void FireMerbles()
        {
            for (int i = 0; i < _merbles.CountInactive+1; i++)
            {
                _merbles.Get();
            }
        }
    }
}
