using System.Collections;
using UnityEngine;

namespace Ability.Object
{
    public class SmashableTarget : MonoBehaviour
    {
        enum HealthLevelEnum
        {
            Low,
            Medium,
            High
        }
        [SerializeField] private HealthLevelEnum healthLevel;

        [SerializeField] private int health;
        public int Health => health;
        private bool _canTakeDamage = true;
        private void Start()
        {
            switch (healthLevel)
            {
                case HealthLevelEnum.Low:
                    health = 1;
                    break;
                case HealthLevelEnum.Medium:
                    health = 2;
                    break;
                case HealthLevelEnum.High:
                    health = 3;
                    break;
            }
        }
        public void DecreaseHealth(float damage)
        {
            if (_canTakeDamage)
            {
                int damageInt = Mathf.RoundToInt(damage);
                health -= damageInt;
                Debug.Log("Health: " + health + " Damage: " + damageInt);
                if (damageInt <= 0)
                {
                    //gameObject.SetActive(false);
                }
                _canTakeDamage = false;
                StartCoroutine(EndHitStun());
            }

        }

        IEnumerator EndHitStun()
        {
            yield return new WaitForSeconds(0.1f);
            _canTakeDamage = true;
        }
    }
}
