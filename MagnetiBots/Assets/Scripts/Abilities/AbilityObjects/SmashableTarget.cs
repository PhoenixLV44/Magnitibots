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
        
        private ParticleSystem _dustParticles;
        public ParticleSystem DustParticles => _dustParticles;
        private GameObject _rock;
        public GameObject Rock => _rock;
        
        [SerializeField] BoxCollider[] colliders;
        public BoxCollider[] Colliders => colliders;

        [SerializeField] private Cat.Cat cat;
        public Cat.Cat Cat => cat;
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
            _rock = transform.GetChild(0).gameObject;
            _dustParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
            if (cat)
            {
                cat.IncreaseTriggersNeeded();
            }
        }
        public void DecreaseHealth(float damage)
        {
            if (_canTakeDamage)
            {
                _canTakeDamage = false;
                StartCoroutine(EndHitStun());
                Globals.Managers.Audio.PlaySFXRandom("RockSmashing", transform, 4,1);
                int damageInt = Mathf.RoundToInt(damage);
                health -= damageInt;
                Debug.Log("Health: " + health + " Damage: " + damageInt);
                _dustParticles.Play();
                if (damageInt <= 0)
                {
                    Debug.Log("Dead");
                    _rock.SetActive(false);
                    foreach (var boxCollider in colliders)
                    {
                        boxCollider.enabled = false;
                    }
                }
            }

        }

        IEnumerator EndHitStun()
        {
            yield return new WaitForSeconds(0.1f);
            _canTakeDamage = true;
        }
    }
}
