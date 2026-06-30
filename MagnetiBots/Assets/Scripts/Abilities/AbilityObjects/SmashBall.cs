using System;
using System.Collections;
using System.Collections.Generic;
using Merbles;
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
        
        private Vector3 _baseScale = new Vector3(1f, 1f, 1f);
        public Vector3 BaseScale => _baseScale;

        private List<Merbles.Merble> _merbleList;
        public List<Merbles.Merble> MerbleList { get => _merbleList; set => _merbleList = value; }
        
        private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
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
                else
                {  
                    _smashAbility.DeactivateBall();
                }

            }
            else if (other.CompareTag("Ground") && rb.linearVelocity.y < 0)
            {
                Debug.Log("Ground");
                _smashAbility.DeactivateBall();
                _smashAbility.MerbleBoss.FireMerbles();
            }
            else if (!other.CompareTag("Ground") && !other.CompareTag("SmashTarget") && rb.linearVelocity.y < 0)
            {
                _smashAbility.DeactivateBall();
            }
        }

        public void IncreasePowerLevel(float newPowerLevel)
        {
            _powerLevel = newPowerLevel;
            transform.localScale = _baseScale * (_powerLevel);
        }
        private void OnEnable()
        {
            _powerLevel = 1;
            transform.localScale = _baseScale;
            if (!rb)
            {
                rb = GetComponent<Rigidbody>();
            }
            //triggerCollider.enabled = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            StopAllCoroutines();
        }
        

        private void Update()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1, groundLayer))
            {
                if (hitInfo.collider.CompareTag("Ground"))
                {
                    Debug.Log("Ground");
                    _smashAbility.DeactivateBall();
                    _smashAbility.MerbleBoss.FireMerbles();
                }
            }
        }
    }
}
