using System;
using System.Collections;
using UnityEngine;

namespace Ability.Object
{
    public class SmashBall : MonoBehaviour
    {
        [SerializeField] private float _powerLevel = 1;
        public float PowerLevel { get => _powerLevel; set => _powerLevel = value; }
        private Ability.Smash _smashAbility;
        public Ability.Smash SmashAbility { get => _smashAbility; set => _smashAbility = value; }
        [SerializeField] SphereCollider triggerCollider;
        public SphereCollider TriggerCollider => triggerCollider;

        private void OnTriggerEnter(Collider other)
        {
            triggerCollider.enabled = false;
            if (other.CompareTag("SmashTarget"))
            {
                SmashableTarget target = other.GetComponent<SmashableTarget>();
                target.DecreaseHealth(_powerLevel);
                if (target.Health <= 0)
                {
                    Destroy(target.gameObject);
                }
            }
            _smashAbility.DeactivateBall();
        }

        private void OnEnable()
        {
            _powerLevel = 1;
            //triggerCollider.enabled = false;
        }
        
    }
}
