using UnityEngine;

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
                boss.currentMerbles++;
            }
        }
    }
}
