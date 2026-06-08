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
        
        private Vector3 _baseScale = new Vector3(1.5f, 1.5f, 1.5f);
        public Vector3 BaseScale => _baseScale;

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

        public void IncreasePowerLevel(float newPowerLevel)
        {
            _powerLevel = newPowerLevel;
            transform.localScale = _baseScale * (_powerLevel * 0.75f);
        }
        private void OnEnable()
        {
            _powerLevel = 1;
            transform.localScale = _baseScale;
            //triggerCollider.enabled = false;
        }
        
    }
}
