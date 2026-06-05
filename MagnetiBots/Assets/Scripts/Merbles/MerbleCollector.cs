using UnityEngine;
using UnityEngine.AI;

namespace Merbles
{
    public class Collector : MonoBehaviour
    {
        public Merbles.Boss boss;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MerbleCollect"))
            {
                Merble hitMerble = other.gameObject.GetComponent<Merble>();
                hitMerble.myBoss = boss;
                hitMerble.SetPool(boss.Merbles);
                hitMerble.SetFollowType(boss.MerbleFollowType);
                boss.currentMerbles++;
                hitMerble.GetComponent<NavMeshAgent>().avoidancePriority = 10+boss.currentMerbles;
            }
        }
    }
}
