using System.Collections;
using System.Collections.Generic;
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
            else if (other.CompareTag("Ground"))
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
            StopAllCoroutines();
        }

        public IEnumerator MoveMerbles()
        {
            List<Merbles.Merble> merbleList =  new List<Merbles.Merble>();
            while(gameObject.activeSelf)
            {
                if (_smashAbility.MerbleBoss.ChargedMerbleList.Count > 0)
                {
                    Debug.Log("Moving Merbles");
                    merbleList = _smashAbility.MerbleBoss.ChargedMerbleList;
                    for(int i = 0 ; i < merbleList.Count; i++)
                    {
                        if (!rb.useGravity)
                        {
                            merbleList[i].FloatTowardsObject(gameObject, i);
                        }
                        else
                        {
                            merbleList[i].FloatTowardsObject(gameObject, i, rb.linearVelocity.magnitude);
                        }
                    }
                }
                yield return null;
            }
        }
    }
}
