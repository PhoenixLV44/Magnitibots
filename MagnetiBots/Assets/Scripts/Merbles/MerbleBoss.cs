using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Merbles
{
    public class Boss : MonoBehaviour
    {
        Merbles.Collector collector;
        
        public int currentMerbles = 0;
        public int chargingMerbles = 0;
        public int chargedMerbles = 0;
        public List<Merble> merbleList;
        [SerializeField] private List<Merble> chargedMerblesList;
        public List<Merble> ChargedMerbleList{get => chargedMerblesList; set => chargedMerblesList = value; }
        
        [SerializeField] private List<Merble> _masterList = new List<Merble>();
        public List<Merble> MasterList => _masterList;

        public GameObject merblePrefab;
        public Merble.FollowTypes MerbleFollowType {get {return _merbleFollowType;} set { _merbleFollowType = value; } }
        private Merble.FollowTypes _merbleFollowType;

        public ObjectPool<GameObject> Merbles { get { return _merbles; } private set { _merbles = value; } }
        private ObjectPool<GameObject> _merbles;
        public int defaultCapacity;
        public int maxSize;

        private void Start()
        {
            merbleList = new List<Merble>();
            merblePrefab.GetComponent<Merble>().myBoss = this;
            
            chargedMerblesList = new List<Merble>();

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
            StartCoroutine(AssignMasterList());
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
        public void ChargeMerble(Vector3 target)
        {
            for (int i = 0; i < merbleList.Count; i++)
            {
                if (merbleList[i] != null)
                {
                    if (!merbleList[i].Charging)
                    {
                        merbleList[i].StartCharge(target);
                        chargingMerbles++;
                        break;
                    }
                }
                
            }
            
        }
        public void FireMerbles()
        {
            Merble[] merbleArray = chargedMerblesList.ToArray();
            for (int i = 0; i < merbleArray.Length; i++)
            {
                merbleArray[i].StopCharging();
            }
            chargingMerbles = 0;
            for (int i = 0; i < merbleList.Count; i++)
            {
                //_merbles.Get();
                if (merbleList[i].Charging)
                {
                    merbleList[i].StopCharging();
                }
            }
            chargedMerbles = 0;
            merbleList.Sort((a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position,transform.position)));
        }

        IEnumerator AssignMasterList()
        {
            while (true)
            {
                if (merbleList.Count > 0)
                {
                    foreach (var merble in merbleList)
                    {
                        if (!_masterList.Contains(merble))
                        {
                            _masterList.Add(merble);
                        }   
                    }
                }

                if (chargedMerblesList.Count > 0)
                {
                    foreach (var merble in chargedMerblesList)
                    {
                        if (!_masterList.Contains(merble))
                        {
                            _masterList.Add(merble);
                        }
                    }
                }

                if (_masterList.Count > 0)
                {
                    foreach (var merble in _masterList)
                    {
                        merble.transform.name = "Merble " + _masterList.IndexOf(merble);
                    }
                }
                yield return null;
            }
        }
    }
}
